using ET;
using TrueSync;

namespace GameLogic.Battle
{
    public class MoveAbility : LSEntity, IActionAbility, IAwake,IDestroy
    {
        public Actor Owner { get { return GetParent<Actor>(); } set { } }
        public bool Enable { get; set; }
    }
    
    [EntitySystemOf(typeof(MoveAbility))]
    public static partial class MoveAbilitySystem
    {
        public static bool TryMakeAction(this MoveAbility self, out MoveAction action)
        {
            if (BattleActionHelper.TryMakeActionInner(self, out action, true))
            {
                action.Snapshot = new StateMove();
                return  true;
            }
            return false;
        }
        public static void OnFrameState(this MoveAbility self, StateMove state)
        {
            if (BattleActionHelper.TryMakeActionInner(self, out MoveAction action, false))
            {
                action.Snapshot = state;
                action.IsSnapshot = true;
                action.DoAction();
            }
        }
    }
}