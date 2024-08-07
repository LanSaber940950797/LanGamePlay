using ET;

namespace GameLogic
{
    [MessageHandler(SceneType.Room)]
    public class OneFrameStatesMessageHandler : MessageHandler<Room, OneFrameStatesMessage>
    {
        protected override  async ETTask Run(Room room, OneFrameStatesMessage message)
        {
            using var _ = message ; // 方法结束时回收消息
           
            var states = message.States;
            Log.Debug($"OneFrameInputs: {room.AuthorityFrame + 1} {states.ToJson()}");
                        
            var frameBuffer = room.StateFrameBuffer;
            var myId = room.Root().GetComponent<PlayerComponent>().PlayerId;
            ++room.AuthorityFrame;
            // 服务端返回的消息比预测的还早
            if (room.AuthorityFrame > room.PredictionFrame)
            {
                var predictionStates = frameBuffer.FrameStates(room.AuthorityFrame);
                states.CopyTo(predictionStates);
                // input.CopyTo(authorityFrame);
            }
            else
            {
                // 服务端返回来的消息，跟预测消息对比
                var predictionStates = frameBuffer.FrameStates(room.AuthorityFrame);
                // 对比失败有两种可能，
                // 1是别人的输入预测失败，这种很正常，
                // 2 自己的输入对比失败，这种情况是自己发送的消息比服务器晚到了，服务器使用了你的上一次输入
                // 回滚重新预测的时候，自己的输入不用变化
                predictionStates.States.TryGetValue(myId, out OneFrameState predictionState);
                predictionState ??= new OneFrameState();
                states.States.TryGetValue(myId, out OneFrameState authorityState);
                authorityState ??= new OneFrameState();
                if (!FrameStateHelper.Check(predictionState, authorityState))
                {
                    Log.Debug($"frame diff: {predictionState} {authorityState}");
                    states.CopyTo(predictionStates);
                    // 回滚到frameBuffer.AuthorityFrame
                    Log.Debug($"roll back start {room.AuthorityFrame}");
                    SSUpdaterHelper.Rollback(room, room.AuthorityFrame);
                    Log.Debug($"roll back finish {room.AuthorityFrame}");
                }
                else // 对比成功
                {
                    states.CopyTo(predictionStates);
                    //Log.Error($"frame Update: {states} {predictionState}");
                    room.TpsUpdate(predictionStates, room.AuthorityFrame, myId);
                }
            }
            await ETTask.CompletedTask;
        }
    }
}