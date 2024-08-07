﻿using ET;

namespace GameLogic.Battle
{
    [EntitySystemOf(typeof(ActorViewComponent))]
    public static partial class ActorViewComponentSystem
    {
        [EntitySystem]
        public static void Awake(this ActorViewComponent self)
        {
            self.InitActorView();
            var room = self.GetParent<Room>();
            room.LSWorld.AddEventListener<Actor, ActorCreateInfo>(BattleEvent.ActorCreate, OnActorCreate, self);
        }
        
        [EntitySystem]
        public static void Destroy(this ActorViewComponent self)
        {
            var room = self.GetParent<Room>();
            room.LSWorld.RemoveEventListener<Actor, ActorCreateInfo>(BattleEvent.ActorCreate, OnActorCreate, self);
        }

        private static void InitActorView(this ActorViewComponent self)
        {
            var room = self.GetParent<Room>();
            LSWorld lsWorld = room.LSWorld;
            var actorComponent = lsWorld.GetComponent<ActorComponent>();
            var actors = actorComponent.GetAllActors();
            foreach (Actor actor in actors)
            {
                if (actor.ActorType == ActorType.System)
                {
                    continue;
                }
                self.AddChildWithId<ActorView, Actor>(actor.Id, actor);
            }
        }

        public static void OnActorCreate(Entity entity, Actor actor, ActorCreateInfo info)
        {
            var self = entity.As<ActorViewComponent>();
            self.AddChildWithId<ActorView, Actor>(actor.Id, actor);
        }
        
    }
}