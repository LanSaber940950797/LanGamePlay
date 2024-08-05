using System;
using ET;

namespace GameLogic
{
    [EntitySystemOf(typeof(RoomSender))]
    public static partial class RoomSenderSystem
    {
        public static void Send(this RoomSender self, long id, IRoomMessage message)
        {
            self.SendInner(id, message).NoContext();
        }

        private static int GetRpcId(this RoomSender self)
        {
            return ++self.RpcId;
        }
        
        private static async ETTask SendInner(this RoomSender self, long id, IRoomMessage message)
        {
            Fiber fiber = self.Fiber();
            using (await fiber.Root.GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.RoomMessage, id))
            {
                self.SendClientSender(id, message);
            }
        }

        private static void SendClientSender(this RoomSender self, long id, IRoomMessage message)
        {
            message.TargetPlayerId = id;
            var roomMsg = RoomHeler.SerializeMessage(message);
            roomMsg.PlayerId = message.PlayerId;
            roomMsg.TargetPlayerId = id;
            roomMsg.RoomId = self.Room.Id;
            self.Root().GetComponent<ClientSenderComponent>().Send(roomMsg);
        }
        
        public static async ETTask<IRoomResponse> Call(this RoomSender self, long id, IRoomRequest request,  bool needException = true)
        {
            int rpcId = self.GetRpcId();
            request.RpcId = rpcId;
            
            Type requestType = request.GetType();

            IResponse response;
            await self.SendInner(id, request);
          
            ActorId actorId = new ActorId(0, 0, id);            
            MessageSenderStruct messageSenderStruct = new(actorId, requestType, needException);
            self.requestCallback.Add(rpcId, messageSenderStruct);
            Fiber fiber = self.Fiber();
            async ETTask Timeout()
            {
                await fiber.Root.GetComponent<TimerComponent>().WaitAsync(ProcessInnerSender.TIMEOUT_TIME);

                if (!self.requestCallback.Remove(rpcId, out MessageSenderStruct action))
                {
                    return;
                }
                
                if (needException)
                {
                    action.SetException(new Exception($"actor sender timeout: {requestType.FullName}"));
                }
                else
                {
                    IResponse response = MessageHelper.CreateResponse(requestType, rpcId, ErrorCore.ERR_Timeout);
                    action.SetResult(response);
                }
            }
            
            Timeout().NoContext();
            
            long beginTime = TimeInfo.Instance.ServerFrameTime();

            response = await messageSenderStruct.Wait();
            
            long endTime = TimeInfo.Instance.ServerFrameTime();

            long costTime = endTime - beginTime;
            if (costTime > 200)
            {
                Log.Warning($"actor rpc time > 200: {costTime} {requestType.FullName}");
            }
            
            return response as IRoomResponse;
        }

        public static async ETTask HandleMessage(this RoomSender self,  A2RoomInner_Message messageInfo)
        {
            if (messageInfo.MessageObj == null)
            {
                return;
            }
            
            var message = RoomHeler.DeserializeMessage(messageInfo);
            
            Log.Info($"Room receive message: {message}");
            if (message is IRoomMessage roomMsg)
            {
                roomMsg.PlayerId = messageInfo.PlayerId;
                roomMsg.TargetPlayerId = messageInfo.TargetPlayerId;
            }

            if (message is IRoomResponse response)
            {
                self.HandleResponse(response);
                return;
            }
            Fiber fiber = self.Fiber();
            ActorId actorId = self.Parent.GetActorId();
            
            if (message is IRoomRequest request)
            {
                using (await fiber.Root.GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.RoomMessage, messageInfo.PlayerId))
                {
                    int rpcId = request.RpcId;
                    IRoomResponse resp = await fiber.Root.GetComponent<ProcessInnerSender>().Call(actorId, request, needException: true) as IRoomResponse;
                    resp.RpcId = rpcId;
                    self.SendClientSender(messageInfo.PlayerId, resp);
                }
                
            }
            else
            {
                fiber.Root.GetComponent<ProcessInnerSender>().Send(actorId, message);
            }
            
        }
        
        private static void HandleResponse(this RoomSender self, IRoomResponse response)
        {
            if (!self.requestCallback.Remove(response.RpcId, out MessageSenderStruct actorMessageSender))
            {
                return;
            }
            //Log.Debug($"RpcId: {response.RpcId} receive response: {response}");
            Run(actorMessageSender, response);
        }
        
        private static void Run(MessageSenderStruct self, IResponse response)
        {
            if (response.Error == ErrorCore.ERR_MessageTimeout)
            {
                self.SetException(new RpcException(response.Error, $"Rpc error: request, 注意Actor消息超时，请注意查看是否死锁或者没有reply: actorId: {self.ActorId} {self.RequestType.FullName}, response: {response}"));
                return;
            }

            if (self.NeedException && ErrorCore.IsRpcNeedThrowException(response.Error))
            {
                self.SetException(new RpcException(response.Error, $"Rpc error: actorId: {self.ActorId} request: {self.RequestType.FullName}, response: {response}"));
                return;
            }

            self.SetResult(response);
        }
        
        public static void Reply(this RoomSender self, long id, IRoomResponse message)
        {
            self.Send(id, message);
        }
        
        
        
        public static void SendRoomSvr(this RoomSender self,  IRoomMessage message)
        {
            self.Send(self.Room.MasterId, message);
        }
        
        public static async ETTask<IRoomResponse> CallRoomSvr(this RoomSender self,  IRoomRequest request)
        {
            IRoomResponse res = await self.Call(self.Room.MasterId, request);
            return res;
        }
        
        //广播
        public static void Broadcast(this RoomSender self, IRoomMessage message, bool isNoMaster = false)
        {
            // 广播的消息不能被池回收
            (message as MessageObject).IsFromPool = false;
            var room = self.Room;
            Fiber fiber = self.Fiber();
            foreach (long playerId in room.PlayerIds)
            {
                message.TargetPlayerId = playerId;
                if (message.TargetPlayerId == room.MasterId)
                {
                    if (isNoMaster)
                    {
                        continue;
                    }
                    fiber.Root.GetComponent<ProcessInnerSender>().Send(room.GetActorId(), message);
                }
                else
                {
                    self.Send(playerId, message);
                }
            }
        }
    }
}