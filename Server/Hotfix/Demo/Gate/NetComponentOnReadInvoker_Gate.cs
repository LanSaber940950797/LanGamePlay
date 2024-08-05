using System;

namespace ET.Server
{
    [Invoke((long)SceneType.Gate)]
    public class NetComponentOnReadInvoker_Gate: AInvokeHandler<NetComponentOnRead>
    {
        public override void Handle(NetComponentOnRead args)
        {
            HandleAsync(args).NoContext();
        }

        private async ETTask HandleAsync(NetComponentOnRead args)
        {
            Session session = args.Session;
            object message = args.Message;
            Scene root = args.Session.Root();
            // 根据消息接口判断是不是Actor消息，不同的接口做不同的处理,比如需要转发给Chat Scene，可以做一个IChatMessage接口
            switch (message)
            {
                case ISessionMessage:
                {
                    MessageSessionDispatcher.Instance.Handle(session, message);
                    break;
                }
                // case FrameMessage frameMessage:
                // {
                //     Player player = session.GetComponent<SessionPlayerComponent>().Player;
                //     ActorId roomActorId = player.GetComponent<PlayerRoomComponent>().RoomActorId;
                //     frameMessage.PlayerId = player.Id;
                //     root.GetComponent<MessageSender>().Send(roomActorId, frameMessage);
                //     break;
                // }
                case A2RoomInner_Message actorRoom:
                {
                    Player player = session.GetComponent<SessionPlayerComponent>().Player;
                   
                    if (actorRoom.OpCode == GameOuter.C2Room_JoinRoom)
                    {
                        G2Match_GetRoomInfo g2MatchGetRoomInfo = G2Match_GetRoomInfo.Create();
                        g2MatchGetRoomInfo.RoomId = actorRoom.RoomId;
                        StartSceneConfig startSceneConfig = StartSceneConfigCategory.Instance.Match;
                        
                        var match2GGetRoom = await session.Root().GetComponent<MessageSender>().Call(startSceneConfig.ActorId, g2MatchGetRoomInfo) as Match2G_GetRoomInfo;
                        if (match2GGetRoom.Error != 0)
                        {
                            var joinRoom = MessageSerializeHelper.Deserialize(OpcodeType.Instance.GetType(actorRoom.OpCode), actorRoom.MessageObj, 0, actorRoom.MessageObj.Length) as C2Room_JoinRoom;
                            var room2CJoinRoom = MessageHelper.CreateResponse(joinRoom.GetType(), joinRoom.RpcId, match2GGetRoom.Error);

                            A2RoomInner_Message resp = A2RoomInner_Message.Create();
                            resp.OpCode = OpcodeType.Instance.GetOpcode(room2CJoinRoom.GetType());
                            resp.TargetPlayerId = player.Id;
                            resp.MessageObj = MessageSerializeHelper.Serialize(room2CJoinRoom as MessageObject);
                            session.Send(resp);
                            return;
                        }
                        player.RemoveComponent<PlayerRoomComponent>();
                        player.AddComponent<PlayerRoomComponent>().RoomActorId = match2GGetRoom.ActorId;
                    }
                    ActorId roomActorId = player.GetComponent<PlayerRoomComponent>().RoomActorId;
                    actorRoom.PlayerId = player.Id;
                    root.GetComponent<MessageSender>().Send(roomActorId, actorRoom);
                    break;
                }
                case ILocationMessage actorLocationMessage:
                {
                    long unitId = session.GetComponent<SessionPlayerComponent>().Player.Id;
                    root.GetComponent<MessageLocationSenderComponent>().Get(LocationType.Unit).Send(unitId, actorLocationMessage);
                    break;
                }
                case ILocationRequest actorLocationRequest: // gate session收到actor rpc消息，先向actor 发送rpc请求，再将请求结果返回客户端
                {
                    long unitId = session.GetComponent<SessionPlayerComponent>().Player.Id;
                    int rpcId = actorLocationRequest.RpcId; // 这里要保存客户端的rpcId
                    long instanceId = session.InstanceId;
                    IResponse iResponse = await root.GetComponent<MessageLocationSenderComponent>().Get(LocationType.Unit).Call(unitId, actorLocationRequest);
                    iResponse.RpcId = rpcId;
                    // session可能已经断开了，所以这里需要判断
                    if (session.InstanceId == instanceId)
                    {
                        session.Send(iResponse);
                    }
                    break;
                }
                case IRequest actorRequest:  // 分发IActorRequest消息，目前没有用到，需要的自己添加
                {
                    break;
                }
                case IMessage actorMessage:  // 分发IActorMessage消息，目前没有用到，需要的自己添加
                {
                    break;
                }
				
                default:
                {
                    throw new Exception($"not found handler: {message}");
                }
            }
        }
    }
}