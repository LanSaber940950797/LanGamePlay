using ET;
using GameLogic.Battle;
using TrueSync;
using UnityEngine;

namespace GameLogic
{
    [ComponentOf(typeof(ActorView))]
    public partial class TopDownActorView : Entity, IAwake,IUpdate
    {
        public ActorView ActorView => GetParent<ActorView>();
        public Actor Actor => ActorView.Actor;
    }
    
    [EntitySystemOf(typeof(TopDownActorView))]
    public static partial class TopDownActorViewSystem
    {
        [EntitySystem]
        public static void Awake(this TopDownActorView self)
        {
           self.LodeMode().NoContext();
           self.Actor.ListenActionPoint(ActionPointType.PostReceiveDamage, OnPostReceiveDamage, self);
        }
        
        private static async ETTask LodeMode(this TopDownActorView self)
        {
            var viewComponent = self.ActorView.GetComponent<ViewComponent>();
            await viewComponent.LoadMode("Warrior2d");
            var transformComponent = self.Actor.GetComponent<TransformComponent>();
            viewComponent.Go.transform.position = transformComponent.Position.ToVector();
            viewComponent.Go.transform.forward = transformComponent.Forward.ToVector();
            viewComponent.ModelGo.transform.localPosition = new Vector3(0,0,0);
            self.ActorView.AddComponent<ActorAnimationComponent>();
        }

        [EntitySystem]
        public static void Update(this TopDownActorView self)
        {
            ActorView actorView = self.ActorView;
            Actor actor = actorView.Actor;
            if (actor.IsDisposed || actorView.IsDisposed)
            {
                return;
            }
            self.UpdataAnimation();
            var viewComponent = actorView.GetComponent<ViewComponent>();
            var transformComponent = actor.GetComponent<TransformComponent>();
            if (viewComponent.Go != null)
            {
                viewComponent.Go.transform.position = transformComponent.Position.ToVector();
                viewComponent.Go.transform.forward = transformComponent.Forward.ToVector();
            }
        }
        
        private  static void UpdataAnimation(this TopDownActorView self)
        {
            var viewComponent = self.ActorView.GetComponent<ViewComponent>();
            var transformComponent = self.Actor.GetComponent<TransformComponent>();
            var moveComponent = self.Actor.GetComponent<MoveComponent>();
            var sqr = moveComponent.Velocity.sqrMagnitude;
            if (sqr > FP.Epsilon)
            {
                self.ActorView.GetComponent<ActorAnimationComponent>()?.PlayMove();
                FP x = 0;
                
                if (moveComponent.Velocity.x > 0)
                {
                    x = 1;
                }
                else
                {
                    x = -1;
                }
                var forward = new TSVector(x,0,0);
                
                transformComponent.Forward = forward;
            }
            else
            {
                self.ActorView.GetComponent<ActorAnimationComponent>()?.PlayIde();
            }
          
        }

        private static void OnPostReceiveDamage(Entity entity, Entity actionEntity)
        {
            TopDownActorView self = entity.As<TopDownActorView>();
            self.Parent.GetComponent<ActorAnimationComponent>()?.PlayOnHurt();
        }

    }
}