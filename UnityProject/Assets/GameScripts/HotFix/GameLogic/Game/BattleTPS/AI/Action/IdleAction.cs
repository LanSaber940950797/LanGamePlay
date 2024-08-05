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
            var pos = actor.GetComponent<TransformComponent>().Position;
            //MoveToTarget(pos);
            actor.GetComponent<MoveComponent>().StopMove();
            return Status.Success;
        }
    }
}