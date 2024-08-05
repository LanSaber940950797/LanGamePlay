using ET;


namespace GameLogic.Battle
{
   public class CureActionAbility : LSEntity, IActionAbility, IAwake,IDestroy
   {
        public Actor Owner { get { return GetParent<Actor>(); } set { } }
        public bool Enable { get; set; }


        
   }

    [EntitySystemOf(typeof(CureActionAbility))]
    public static  partial class CureActionAbilitySystem
    {
        [EntitySystem]
        public  static void Awake(this CureActionAbility self)
        {
            //self.Parent.AddEventListener<StateFrame>(FrameStateEvent.GetEventId(FrameStateType.StateCure), OnStateCure, self);
        }

   

        [EntitySystem]
        public static void Destroy(this CureActionAbility self)
        {
            //self.Parent?.RemoveEventListener<StateFrame>(FrameStateEvent.GetEventId(FrameStateType.StateCure), OnStateCure, self);
        }
        public static bool TryMakeAction(this CureActionAbility self, out CureAction action)
        {
            if (BattleActionHelper.TryMakeActionInner(self, out action, true))
            {
                action.Snapshot = new StateCure();
                return  true;
            }
            return false;
        }
        public static void OnFrameState(this CureActionAbility self, StateCure stateStateCure)
        {
            if (BattleActionHelper.TryMakeActionInner(self, out CureAction action, false))
            {
                action.Snapshot = stateStateCure;
                action.IsSnapshot = true;
                action.DoAction();
            }
        }
    }
}