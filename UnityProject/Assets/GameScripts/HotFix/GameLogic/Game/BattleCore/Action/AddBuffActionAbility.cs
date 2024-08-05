using ET;

namespace GameLogic.Battle
{
    public class AddBuffActionAbility : LSEntity, IActionAbility
    {
        public Actor Owner { get => GetParent<Actor>(); set{} }
        public bool Enable { get; set; }
        
    }
    
    [EntitySystemOf(typeof(AddBuffActionAbility))]
    public static  partial class AddBuffActionAbilitySystem
    {
        public static bool TryMakeAction(this AddBuffActionAbility self, out AddBuffAction action)
        {
            if (BattleActionHelper.TryMakeActionInner(self, out action, true))
            {
                action.Snapshot = new StateAddBuff();
                return  true;
            }
            return false;
           
        }
        public static void OnFrameState(this AddBuffActionAbility self, StateAddBuff state)
        {
            if (BattleActionHelper.TryMakeActionInner(self, out AddBuffAction action, false))
            {
                action.Snapshot = state;
                action.IsSnapshot = true;
                action.DoAction();
            }
        }
  
    }
}