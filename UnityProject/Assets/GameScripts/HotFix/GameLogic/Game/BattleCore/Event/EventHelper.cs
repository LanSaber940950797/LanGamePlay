using System;
using ET;

namespace GameLogic.Battle
{
    public static class EventHelper
    {
        public static void SendEvent(this Entity self, int eventId)
        {
            self.GetComponent<EventComponent>()?.EventDispatcher.SendEvent(eventId);
        }
        
        public static void SendEvent<TArg1>(this Entity self, int eventId, TArg1 arg1)
        {
            self.GetComponent<EventComponent>()?.EventDispatcher.SendEvent(eventId, arg1);
        }
        
        public static void SendEvent<TArg1, TArg2>(this Entity self, int eventId, TArg1 arg1, TArg2 arg2)
        {
            self.GetComponent<EventComponent>()?.EventDispatcher.SendEvent(eventId, arg1, arg2);
        }
        
        public static void SendEvent<TArg1, TArg2, TArg3>(this Entity self, int eventId, TArg1 arg1, TArg2 arg2, TArg3 arg3)
        {
            self.GetComponent<EventComponent>()?.EventDispatcher.SendEvent(eventId, arg1, arg2, arg3);
        }
        
        public static void SendEvent<TArg1, TArg2, TArg3, TArg4>(this Entity self, int eventId, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4)
        {
            self.GetComponent<EventComponent>()?.EventDispatcher.SendEvent(eventId, arg1, arg2, arg3, arg4);
        }

        private static EventComponent GetOrCreateEventComponet(this Entity self)
        {
            var componet = self.GetComponent<EventComponent>();
            if(componet == null)
            {
                componet = self.AddComponent<EventComponent>();
            }

            return componet;
        }
        public static void AddEventListener(this Entity self, int eventId, Action<Entity> eventCallback, Entity owner)
        {
            self.GetOrCreateEventComponet().EventDispatcher.AddEventListener(eventId, eventCallback, owner);
        }
        
        public static void AddEventListener<TArg1>(this Entity self, int eventId, Action<Entity,TArg1> eventCallback, Entity owner)
        {
            self.GetOrCreateEventComponet().EventDispatcher.AddEventListener(eventId, eventCallback, owner);
        }
        
        public static void AddEventListener<TArg1, TArg2>(this Entity self, int eventId, Action<Entity,TArg1, TArg2> eventCallback, Entity owner)
        {
            self.GetOrCreateEventComponet().EventDispatcher.AddEventListener(eventId, eventCallback, owner);
        }
        
        public static void AddEventListener<TArg1, TArg2, TArg3>(this Entity self, int eventId, Action<Entity,TArg1, TArg2, TArg3> eventCallback, Entity owner)
        {
            self.GetOrCreateEventComponet().EventDispatcher.AddEventListener(eventId, eventCallback, owner);
        }
        
        public static void AddEventListener<TArg1, TArg2, TArg3, TArg4>(this Entity self, int eventId, Action<Entity,TArg1, TArg2, TArg3, TArg4> eventCallback, Entity owner)
        {
            self.GetOrCreateEventComponet().EventDispatcher.AddEventListener(eventId, eventCallback, owner);
        }
        
        public static void RemoveEventListener(this Entity self,int eventId, Action<Entity> eventCallback, Entity owner)
        {
            self.GetComponent<EventComponent>()?.EventDispatcher.RemoveEventListener(eventId, eventCallback, owner);
        }
        
        public static void RemoveEventListener<TArg1>(this Entity self,int eventId, Action<Entity,TArg1> eventCallback, Entity owner)
        {
            self.GetComponent<EventComponent>()?.EventDispatcher.RemoveEventListener(eventId, eventCallback, owner);
        }
        
        public static void RemoveEventListener<TArg1, TArg2>(this Entity self,int eventId, Action<Entity,TArg1, TArg2> eventCallback, Entity owner)
        {
            self.GetComponent<EventComponent>()?.EventDispatcher.RemoveEventListener(eventId, eventCallback, owner);
        }
        
        public static void RemoveEventListener<TArg1, TArg2, TArg3>(this Entity self,int eventId, Action<Entity,TArg1, TArg2, TArg3> eventCallback, Entity owner)
        {
            self.GetComponent<EventComponent>()?.EventDispatcher.RemoveEventListener(eventId, eventCallback, owner);
        }
        
       
        
        public static void RemoveAllListenerByOwner(this Entity self, Entity owner)
        {
            self.GetComponent<EventComponent>()?.EventDispatcher.RemoveAllListenerByOwner(owner);
        }
    }
}