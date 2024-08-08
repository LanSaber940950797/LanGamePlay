using ET;
using GameLogic.Battle;

namespace GameLogic
{
    [ComponentOf(typeof(LSWorld))]
    public class TopDownActorComponent : LSEntity, IAwake, IDestroy
    {
        
    }
    
    [EntitySystemOf(typeof(TopDownActorComponent))]
    public static partial class TopDownActorComponentSystem
    {
        [EntitySystem]
        public static void Awake(this TopDownActorComponent self)
        {
            self.LSWorld().AddEventListener<CreateActorAction>(BattleEvent.ActorCreate, OnActorCreate, self);
        }
        
        [EntitySystem]
        public static void Destroy(this TopDownActorComponent self)
        {
            var room = self.GetParent<Room>();
            room.LSWorld.RemoveEventListener<CreateActorAction>(BattleEvent.ActorCreate, OnActorCreate, self);
        }

        private static void OnActorCreate(Entity entity,  CreateActorAction action)
        {
            var actor = action.Target;
            actor.AddComponent<TopDownActor>();
        }
        
    }
}