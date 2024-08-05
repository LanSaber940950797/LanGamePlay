using ET;
using TEngine;

namespace GameLogic.Battle
{
    [EntitySystemOf(typeof(EventComponent))]
    public static partial class EventComponentSystem
    {
        [EntitySystem]
        public static void Awake(this EventComponent self)
        {
            self.EventDispatcher = MemoryPool.Acquire<ActorEventDispatcher>();
        }
        [EntitySystem]
        public static void Destroy(this EventComponent self)
        {
            MemoryPool.Release(self.EventDispatcher);
            self.EventDispatcher = null;
        }
    }
}