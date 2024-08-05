using ET;

namespace GameLogic.Battle
{
    public class DamageActionAbility : LSEntity, IActionAbility, IAwake,IDestroy
    {
        public Actor Owner { get { return GetParent<Actor>(); } set { } }
        public bool Enable { get; set; }


       
    }
    
    public static partial class DamageActionAbilitySystem
    {
        public static bool TryMakeAction(this DamageActionAbility self, out DamageAction action)
        {
            if (BattleActionHelper.TryMakeActionInner(self, out action, true))
            {
                action.Snapshot = new StateDamage();
                return  true;
            }
            return false;
        }
        public static void OnFrameState(this DamageActionAbility self, StateDamage stateState)
        {
            if (BattleActionHelper.TryMakeActionInner(self, out DamageAction action, false))
            {
                action.Snapshot = stateState;
                action.IsSnapshot = true;
                action.DoAction();
            }
        }
    }
}