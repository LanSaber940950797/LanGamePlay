using ET;
using TEngine;
using TrueSync;
using UnityEngine;
using Log = ET.Log;

namespace GameLogic.Battle
{
    [EntitySystemOf(typeof(ActorView))]
    public static partial class ActorViewSystem
    {
        public static ActorView GetActorView(this Actor self)
        {
            var wolrd = self.LSWorld();
            return wolrd.Parent.GetComponent<ActorViewComponent>().GetChild<ActorView>(self.Id);
        }
        
        [EntitySystem]
        public static void Awake(this ActorView self, Actor actor)
        {
            self.Actor = actor;
            self.SendEvent().NoContext();
        }

        private static async ETTask SendEvent(this ActorView self)
        {
            //下一帧再发出事件，让逻辑层先加载
            await self.Root().GetComponent<TimerComponent>().WaitFrameAsync();
            self.Room().SendEvent(BattleEvent.ActorCreateView, self);
        }
        
    }
}