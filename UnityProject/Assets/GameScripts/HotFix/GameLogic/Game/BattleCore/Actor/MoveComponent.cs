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
            self.Speed = 0.5f;
            self.MaxSpeed = self.Speed;
        }
        
        
        public static void SetMaxSpeed(this MoveComponent self, FP maxSpeed)
        {
            self.MaxSpeed = maxSpeed;
            self.Parent.GetComponent<RvoSteerComponent>()?.RefreshRVOAgentSpeed(maxSpeed);
        }

        public static void DoMoveAction(this MoveComponent self, int moveType, TSVector velocity = default)
        {
            if(self.Parent.GetComponent<MoveAbility>().TryMakeAction(out var action))
            {
                if (self.Action != null)
                {
                    self.Action.FinishAction();
                    self.Action = action;
                }
                action.Velocity = velocity;
                action.MoveType = moveType;
                action.DoAction();
            }
        }
    }
}