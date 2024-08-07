using ET;
using GameLogic.Battle;

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
            var numeric = self.Actor.GetComponent<NumericComponent>();
            numeric.SetNoEvent(NumericType.HpBase, 1000);
            numeric.SetNoEvent(NumericType.SpeedBase, 5000);
            numeric.SetNoEvent(NumericType.AttackBase, 50);

            self.Actor.AddComponent<RvoSteerComponent>();
            
            
            self.Actor.ListenActionPoint(ActionPointType.Move,OnActionMove, self);
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
                self.Actor.GetComponent<RvoSteerComponent>().MoveByDir(action.Velocity, moveComponent.MaxSpeed);
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