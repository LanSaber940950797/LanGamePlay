using ET;
using GameLogic.Battle;

namespace GameLogic
{
    
    public static partial class TpsRoomSystem
    {
        public static void TpsInit(this Room self, long startTime, int frame = -1, LSWorld serverWorld = null)
        {
            self.Init(startTime, frame, serverWorld);
            if (serverWorld == null)
            {
                //不是同步过来的数据，那么要创建逻辑组件
                serverWorld.AddComponent<FrameStateComponent>();
            }
            
            //加载前端组件
            self.AddComponent<InputControlComponent>();
            self.AddComponent<SSUpdater>();
        }

        public static void TpsUpdate(this Room self, OneFrameStates oneFrameStates, int frame, long noUpdateId = -1)
        {
            LSWorld lsWorld = self.LSWorld;
            FrameStateComponent frameStateComponent = lsWorld.GetComponent<FrameStateComponent>();
            frameStateComponent.Update(oneFrameStates, frame, noUpdateId);
            lsWorld.Frame = frame;
            var inputControlComponent = self.GetComponent<InputControlComponent>();
            lsWorld.Update();
            inputControlComponent.LSLateUpdate();
            frameStateComponent.LateUpdate(oneFrameStates, self.MyId);
        }
    }
}