using ET;
using GameLogic.Battle;
using TrueSync;

namespace GameLogic
{
    [ComponentOf(typeof(Actor))]
    public partial class TopDownActor : LSEntity, IAwake
    {
        public Actor Actor => GetParent<Actor>();
    }
    
    [EntitySystemOf(typeof(TopDownActor))]
    public static partial class TopDownActorSystem
    {
        [EntitySystem]
        public static void Awake(this TopDownActor self)
        {
            Actor actor = self.Actor;
            var numeric = actor.GetComponent<NumericComponent>();
            numeric.SetNoEvent(NumericType.HpBase, 1000);
            numeric.SetNoEvent(NumericType.SpeedBase, 5000);
            numeric.SetNoEvent(NumericType.AttackBase, 50);
            actor.GetComponent<MoveComponent>().Speed = numeric.GetAsFP(NumericType.Speed);
            actor.GetComponent<TransformComponent>().Forward = TSVector.right;
            actor.AddComponent<RvoSteerComponent>();
            if (actor.ActorType == ActorType.Player)
            {
                actor.AddComponent<ActorAIComponent, string>("tree_ai_topdown_player");
                actor.AttachSkill(1001);
            }
            
            actor.ListenActionPoint(ActionPointType.Move,OnActionMove, self);
            //self.Actor.ListenActionPoint(ActionPointType.PreMove,OnActionPreMove, self);
        }

        private static void OnActionMove(Entity entity, Entity actionEntity)
        {
            TopDownActor self = entity.As<TopDownActor>();
            MoveAction action = actionEntity.As<MoveAction>();
            MoveComponent moveComponent = self.Actor.GetComponent<MoveComponent>();
            TransformComponent transformComponent = self.Actor.GetComponent<TransformComponent>();
            if (action.MoveType == (int)MoveType.MoveDir)
            {
                moveComponent.SetMaxSpeed(moveComponent.Speed);
                var det = action.Velocity.normalized * moveComponent.Speed * LSConstValue.UpdateInterval / 1000;
                transformComponent.Position += det;
                moveComponent.Velocity = action.Velocity;
                self.Actor.GetComponent<RvoSteerComponent>().ChangeAgentPosition(transformComponent.Position);
            }
            else if(action.MoveType == (int)MoveType.StopMove)
            {
                moveComponent.SetMaxSpeed(moveComponent.Speed);
                self.Actor.GetComponent<RvoSteerComponent>().Move(transformComponent.Position, moveComponent.MaxSpeed);
                moveComponent.SetMaxSpeed(0);
            }
            
        }
        
     

    }
}