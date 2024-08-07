using ET;
using GameLogic.Battle;

namespace GameLogic
{
    public static class TopDownRoomSystem
    {
        public static void TopDownInit(this Room self, long startTime, int frame = -1, LSWorld serverWorld = null)
        {
            BattleConstValue.WorldType = BattleWorldType.TwoDimensional;
            self.Init(startTime, frame, serverWorld);

            LSWorld lsWorld = self.LSWorld;
            
            //加载逻辑组件
            lsWorld.AddComponent<TopDownActorComponent>();
            lsWorld.AddComponent<RvoWorldComponent>();
            //加载前端组件
            self.AddComponent<InputControlComponent>();
          
        }

        public static void TopDownUpdate(this Room self)
        {
            self.LSWorld.Update();
        }
    }
}