using ET;
using GameLogic.Battle;

namespace GameLogic
{
    [ComponentOf(typeof(Room))]
    public class TopDownActorViewComponent : Entity, IAwake,IDestroy
    {
        private EntityRef<ActorView> playerView;

        public ActorView PlayerView
        {
            get => playerView;
            set => playerView = value;
        }
    }
    
    [EntitySystemOf(typeof(TopDownActorViewComponent))]
    public static partial class TopDownActorViewComponentSystem
    {
        [EntitySystem]
        public static void Awake(this TopDownActorViewComponent self)
        {
            self.Room().AddEventListener<ActorView>(BattleEvent.ActorCreateView, OnActorCreateView, self);
        }
        
        [EntitySystem]
        public static void Destroy(this TopDownActorViewComponent self)
        {
            self.PlayerView = null;
            self.Room().RemoveEventListener<ActorView>(BattleEvent.ActorCreateView, OnActorCreateView, self);
        }
        
        
        private static void OnActorCreateView(Entity entity, ActorView actorView)
        {
            TopDownActorViewComponent self = entity.As<TopDownActorViewComponent>();
            actorView.AddComponent<TopDownActorView>();
            if (actorView.Actor.ActorType == ActorType.Player)
            {
                self.PlayerView = actorView;
            }
        }
        
    }
}