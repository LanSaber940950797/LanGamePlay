using ET;

namespace GameLogic
{
    [MessageHandler(SceneType.Room)]
    public class FrameMessageHandler : MessageHandler<Room, FrameMessage>
    {
        protected override async ETTask Run(Room room, FrameMessage message)
        {
            using FrameMessage _ = message;  // 让消息回到池中
            StateFrameBuffer frameBuffer = room.StateFrameBuffer;
            if (message.Frame % (1000 / LSConstValue.UpdateInterval) == 0)
            {
                long nowFrameTime = room.FixedTimeCounter.FrameTime(message.Frame);
                int diffTime = (int)(nowFrameTime - TimeInfo.Instance.ServerFrameTime());

                Room2C_AdjustUpdateTime room2CAdjustUpdateTime = Room2C_AdjustUpdateTime.Create();
                room2CAdjustUpdateTime.DiffTime = diffTime;
                room.GetComponent<RoomSender>().Send(message.PlayerId, room2CAdjustUpdateTime);
            }

            if (message.Frame < room.AuthorityFrame)  // 小于AuthorityFrame，丢弃
            {
                Log.Warning($"FrameMessage < AuthorityFrame discard: {message}");
                return;
            }

            if (message.Frame > room.AuthorityFrame + 10)  // 大于AuthorityFrame + 10，丢弃
            {
                Log.Warning($"FrameMessage > AuthorityFrame + 10 discard: {message}");
                return;
            }
            
            var oneFrameStates = frameBuffer.FrameStates(message.Frame);
            if (oneFrameStates == null)
            {
                Log.Error($"FrameMessageHandler get frame is null: {message.Frame}, max frame: {frameBuffer.MaxFrame}");
                return;
            }
            oneFrameStates.States[message.PlayerId] = message.State;

            await ETTask.CompletedTask;
        }
    }
}