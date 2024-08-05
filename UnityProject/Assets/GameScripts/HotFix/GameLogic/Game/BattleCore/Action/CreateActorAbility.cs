using ET;

namespace GameLogic.Battle
{
    public class CreateActorAbility : LSEntity, IAwake, IActionAbility
    {
        public Actor Owner { get { return GetParent<Actor>(); } set { } }
        public bool Enable { get; set; }
    }
    
    [EntitySystemOf(typeof(CreateActorAbility))]
    public static  partial class CreateActorAbilitySystem
    {
        public static bool TryMakeAction(this CreateActorAbility self, out CreateActorAction action)
        {
            if (BattleActionHelper.TryMakeActionInner(self, out action, true))
            {
                action.Snapshot = new StateCreateActor();
                return  true;
            }
            return false;
        }


      

        public static void OnFrameState(this CreateActorAbility self, StateCreateActor state)
        {
            if (BattleActionHelper.TryMakeActionInner(self, out CreateActorAction action, false))
            {
                action.Snapshot = state;
                action.IsSnapshot = true;
                action.DoAction();
            }
        }
  
    }
}