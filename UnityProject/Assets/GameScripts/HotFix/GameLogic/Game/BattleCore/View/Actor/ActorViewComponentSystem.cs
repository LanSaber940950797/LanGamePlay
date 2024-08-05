using ET;

namespace GameLogic.Battle
{
    [EntitySystemOf(typeof(ActorViewComponent))]
    public static partial class ActorViewComponentSystem
    {
        [EntitySystem]
        public static void Awake(this ActorViewComponent self)
        {
            var room = self.GetParent<Room>();
            room.AddEventListener<Actor, ActorCreateInfo>(BattleEvent.ActorCreateView, OnActorCreate, self);
        }

        [EntitySystem]
        public static void Destroy(this ActorViewComponent self)
        {
            var room = self.GetParent<Room>();
            room.RemoveEventListener<Actor, ActorCreateInfo>(BattleEvent.ActorCreateView, OnActorCreate, self);
        }

        public static void OnActorCreate(Entity entity, Actor actor, ActorCreateInfo info)
        {
            var self = entity.As<ActorViewComponent>();
            var actorView = self.AddChildWithId<ActorView, Actor>(actor.Id, actor);
        }
        
    }
}