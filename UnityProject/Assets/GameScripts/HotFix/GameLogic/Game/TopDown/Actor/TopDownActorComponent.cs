using ET;
using GameLogic.Battle;

namespace GameLogic
{
    [ComponentOf(typeof(LSWorld))]
    public class TopDownActorComponent : LSEntity, IAwake
    {
        
    }
    
    [EntitySystemOf(typeof(TopDownActorComponent))]
    public static partial class TopDownActorComponentSystem
    {
        [EntitySystem]
        public static void Awake(this TopDownActorComponent self)
        {
            self.LSWorld().AddEventListener<Actor, ActorCreateInfo>(BattleEvent.ActorCreate, OnActorCreate, self);
        }

        private static void OnActorCreate(Entity entity, Actor actor, ActorCreateInfo info)
        {
            actor.AddComponent<TopDownActor>();
        }
        
    }
}