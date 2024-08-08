using Kurisu.AkiBT;

namespace GameLogic.Battle
{
    [AkiInfo("Action:idle")]
    [AkiLabel("BattleAI:idle")]
    [AkiGroup("BattleAI")]
    public class IdleAction : MoveAIAction
    {
        
        protected override Status OnUpdate()
        {
            actor.GetComponent<MoveComponent>().DoMoveAction((int)MoveType.StopMove);
            return Status.Success;
        }
    }
}