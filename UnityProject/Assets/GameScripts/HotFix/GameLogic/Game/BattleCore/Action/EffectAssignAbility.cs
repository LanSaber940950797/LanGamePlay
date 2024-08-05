using ET;

namespace GameLogic.Battle
{
    public class EffectAssignAbility : LSEntity, IActionAbility, IAwake
    {
        public Actor Owner { get { return GetParent<Actor>(); } set { } }
        public bool Enable { get; set; }


       
    }

    public static partial class EffectAssignAbilitySystem
    {
        public static bool TryMakeAction(this EffectAssignAbility self, out EffectAssignAction action)
        {
            if (BattleActionHelper.TryMakeActionInner(self, out action, false))
            {
                return true;
            }
            return false;
        }
    }
}