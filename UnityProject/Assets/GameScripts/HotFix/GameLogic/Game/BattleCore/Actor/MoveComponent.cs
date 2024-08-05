using ET;
using TrueSync;

namespace GameLogic.Battle
{
    
    public  class MoveComponent : LSEntity,IAwake,ILSUpdate
    {
        public Actor Actor => GetParent<Actor>();
        public TSVector Velocity;
        
        public FP Speed;
        public FP MaxSpeed;
        private EntityRef<MoveAction> action;

        public MoveAction Action
        {
            get
            {
                return action;
            }
            set
            {
                action = value;
            }
        }

      
    }
    
    [EntitySystemOf(typeof(MoveComponent))]
    [LSEntitySystemOf(typeof(MoveComponent))]
    public static  partial class MoveComponentSystem
    {
        [EntitySystem]
        public static void Awake(this MoveComponent self)
        {
            //self.Actor.AddEventListener<StateFrame>(FrameStateEvent.StateMove, StateEventMove, self);
            self.Speed = 0.25f;
            self.MaxSpeed = self.Speed;
        }
        
        [LSEntitySystem]
        public static void LSUpdate(this MoveComponent self)
        {
            // if (self.MaxSpeed <= 0)
            // {
            //     return;
            // }
            //
            // if (self.Velocity.sqrMagnitude > FP.Epsilon)
            // {
            //     var velocity = self.Velocity.normalized * self.MaxSpeed * LSConstValue.UpdateInterval / 1000;
            //     self.Actor.SendEvent(BattleEvent.Move, velocity);
            //     self.Velocity = TSVector.zero;
            // }
        }

        

        public static void MoveDir(this MoveComponent self, TSVector dir)
        {
            if(self.Parent.GetComponent<MoveAbility>().TryMakeAction(out var action))
            {
                if (self.Action != null)
                {
                    self.Action.FinishAction();
                    self.Action = action;
                }
                action.Snapshot.Velocity = dir;
                action.Snapshot.MoveType = (int)MoveType.MoveDir;
                //action.MaxSpeed = self.MaxSpeed;
                action.DoAction();
            }
        }
        
        public static void Move(this MoveComponent self, TSVector targetPos)
        {
            TransformComponent transform = self.Actor.GetComponent<TransformComponent>();
            var dir = targetPos - transform.Position;
            self.MoveDir(dir);
        }

        public static void ForceToTarget(this MoveComponent self, TSVector target)
        {
            self.Parent.GetComponent<TransformComponent>().Position = target;
            self.Parent.SendEvent(BattleEvent.ForceChangePosition, target);
        }

        public static void StopMove(this MoveComponent self)
        {
            if(self.Parent.GetComponent<MoveAbility>().TryMakeAction(out var action))
            {
                if (self.Action != null)
                {
                    self.Action.FinishAction();
                    self.Action = action;
                }
                //action.po = self.Parent.GetComponent<TransformComponent>().Position;
                action.Snapshot.MoveType = (int)MoveType.StopMove;
                action.DoAction();
            }
        }
        
        public static void Jump(this MoveComponent self)
        {
            if(self.Parent.GetComponent<MoveAbility>().TryMakeAction(out var action))
            {
                if (self.Action != null)
                {
                    self.Action.FinishAction();
                    self.Action = action;
                }
                //action.po = self.Parent.GetComponent<TransformComponent>().Position;
                action.Snapshot.MoveType = (int)MoveType.Jump;
                action.DoAction();
            }
        }
    }
}