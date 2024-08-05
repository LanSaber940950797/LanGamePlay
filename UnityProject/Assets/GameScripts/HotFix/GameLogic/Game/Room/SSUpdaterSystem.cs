using ET;
using GameLogic.Battle;

namespace GameLogic
{
    [EntitySystemOf(typeof(SSUpdater))]
    public static partial class SSUpdaterSystem
    {
        [EntitySystem]
        private static void Awake(this SSUpdater self)
        {
            Room room = self.GetParent<Room>();
            self.MyId = room.Root().GetComponent<PlayerComponent>().PlayerId;
        }
        
        [EntitySystem]
        private static void Update(this SSUpdater self)
        {
            Room room = self.GetParent<Room>();
            //self.ClientUpdate(room);
            if (Room.IsMaster)
            {
                self.ServerUpdate(room);
            }
            else
            {
                self.ClientUpdate(room);
            }
        }

        private static void ServerUpdate(this SSUpdater self, Room room)
        {
           
            long timeNow = TimeInfo.Instance.ServerFrameTime();


            int frame = room.AuthorityFrame + 1;
            if (timeNow < room.FixedTimeCounter.FrameTime(frame))
            {
                return;
            }
           
            var oneFrameStates = self.GetOneFrameStateMessages(frame);
            ++room.AuthorityFrame;
            room.Update(oneFrameStates, room.AuthorityFrame);
            //self.InputState.States.Clear();
            OneFrameStatesMessage sendFrame = OneFrameStatesMessage.Create();
            sendFrame.Frame = room.AuthorityFrame;
            sendFrame.States = oneFrameStates;
            room.GetComponent<RoomSender>().Broadcast(sendFrame, true);
            
        }


        private static void ClientUpdate(this SSUpdater self, Room room)
        {
            long timeNow = room.ServerNow();
            Scene root = room.Root();

            int i = 0;
            while (true)
            {
                if (timeNow < room.FixedTimeCounter.FrameTime(room.PredictionFrame + 1))
                {
                    return;
                }
               
                // 最多只预测10帧
                if (room.PredictionFrame - room.AuthorityFrame > 5)
                {
                    return;
                }

                ++room.PredictionFrame;
                Log.Debug($"{room.Id} {room.PredictionFrame} {room.AuthorityFrame}");
                //获取当前帧状态缓存
               
                var oneFrameStates = self.GetOneFrameStateMessages(room.PredictionFrame);
                room.Update(oneFrameStates, room.PredictionFrame);
                
                //room.SendHash(room.PredictionFrame);
                
                room.SpeedMultiply = ++i;
                
                FrameMessage frameMessage = FrameMessage.Create();
                frameMessage.Frame = room.PredictionFrame;
                oneFrameStates.States.TryGetValue(self.MyId, out OneFrameState oneFrameState);
                frameMessage.State = oneFrameState ?? new OneFrameState();
                room.GetComponent<RoomSender>().SendRoomSvr(frameMessage);
                
                long timeNow2 = room.ServerNow();
                if (timeNow2 - timeNow > 5)
                {
                    break;
                }
            }
        }
        private static OneFrameStates GetOneFrameStateMessages(this SSUpdater self, int frame)
        {
            Room room = self.GetParent<Room>();
            var frameBuffer = room.StateFrameBuffer;
            
            if (frame <= room.AuthorityFrame)
            {
                return frameBuffer.FrameStates(frame);
            }
         
            // predict
            var predictionFrame = frameBuffer.FrameStates(frame);
            
            frameBuffer.MoveForward(frame);
            // if (frameBuffer.CheckFrame(room.AuthorityFrame))
            // {
            //     var authorityFrame = frameBuffer.FrameStates(room.AuthorityFrame);
            //     authorityFrame.CopyTo(predictionFrame);
            // }
            //predictionFrame.States[self.MyId] = self.InputState;
            self.InputState = new OneFrameState();
            return predictionFrame;
        }
    }
}