using ET;
using Kurisu.AkiBT;

namespace GameLogic.Battle
{
    [AkiInfo("Action:跳跃")]
    [AkiLabel("BattleAI:跳跃")]
    [AkiGroup("BattleAI")]
    public class JumpAction : MoveAIAction
    {
        protected override Status OnUpdate()
        {
            var moveComponent = actor.GetComponent<MoveComponent>();
            moveComponent.Jump();
            return Status.Success;
        }
    }
}