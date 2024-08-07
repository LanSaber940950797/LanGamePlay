using ET;
using GameLogic.Battle;

namespace GameLogic
{
    [ComponentOf(typeof(LSWorld))]
    public class TopDownActorViewComponent : LSEntity, IAwake
    {
        
    }
    
    [EntitySystemOf(typeof(TopDownActorViewComponent))]
    public static partial class TopDownActorViewComponentSystem
    {
        [EntitySystem]
        public static void Awake(this TopDownActorViewComponent self)
        {
            self.LSWorld().AddEventListener<ActorView>(BattleEvent.ActorCreateView, OnActorCreate, self);
        }

        private static void OnActorCreate(Entity entity, ActorView actorView)
        {
            actorView.AddComponent<TopDownActorView>();
        }
        
    }
}