using ET;

namespace GameLogic.Battle
{
    [ComponentOf(typeof(Room))]
    public class TpsActorViewComponent : Entity, IAwake
    {
        
    }
    
    [EntitySystemOf(typeof(TpsActorViewComponent))]
    public static partial class TpsActorViewComponentSystem
    {
        [EntitySystem]
        public static void Awake(this TpsActorViewComponent self)
        {
            self.Room().AddEventListener<ActorView>(BattleEvent.ActorCreateView, OnActorCreateView, self);
        }

        private static void OnActorCreateView(Entity entity, ActorView actorView)
        {
            actorView.AddComponent<TpsActorView>();
        }
    }
}