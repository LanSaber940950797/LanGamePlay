using System;
using ET;
using TEngine;
namespace GameLogic.Battle
{
    [EntitySystemOf(typeof(ActionPointComponent))]
    public  static partial class ActionPointComponentSystem
    {
        [EntitySystem]
        public static void Awake(this ActionPointComponent self)
        {
            self.EventDispatcher = MemoryPool.Acquire<ActorEventDispatcher>();
        }
        [EntitySystem]
        public static void Destroy(this ActionPointComponent self)
        {
            MemoryPool.Release(self.EventDispatcher);
            self.EventDispatcher = null;
        }
        public static void ListenActionPoint(this Actor self, ActionPointType actionPointType, Action<Entity, Entity> action, Entity owner)
        {
            self.GetComponent<ActionPointComponent>()?.AddListener(actionPointType, action, owner);
        }

        public static void UnListenActionPoint(this Actor self, ActionPointType actionPointType, Action<Entity, Entity> action, Entity owner)
        {
            self.GetComponent<ActionPointComponent>()?.RemoveListener(actionPointType, action, owner);
        }

        public static void TriggerActionPoint(this Actor self, ActionPointType actionPointType, Entity action)
        {
            self.GetComponent<ActionPointComponent>()?.TriggerActionPoint(actionPointType, action);
        }
        
        
        public static void AddListener(this ActionPointComponent self, ActionPointType actionPointType, Action<Entity, Entity> action, Entity owner)
        {
            self.EventDispatcher.AddEventListener<Entity>((int)actionPointType, action, owner);
        }

        public static void RemoveListener(this ActionPointComponent self, ActionPointType actionPointType, Action<Entity, Entity> action,  Entity owner)
        {
            self.EventDispatcher.RemoveEventListener<Entity>((int)actionPointType, action, owner);
        }

       

        public static void TriggerActionPoint(this ActionPointComponent self, ActionPointType actionPointType, Entity actionExecution)
        {
            self.EventDispatcher.SendEvent<Entity>((int)actionPointType, actionExecution);
        }

       
    }
}