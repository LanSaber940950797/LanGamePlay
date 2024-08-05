using Kurisu.AkiBT;

namespace GameLogic.Battle
{
    [AkiInfo("Action:朝目标移动")]
    [AkiLabel("BattleAI:朝目标移动")]
    [AkiGroup("BattleAI")]
    public class MoveTargetAction : MoveAIAction
    {
        protected override Status OnUpdate()
        {
            if (!actor.GetComponent<StatusComponent>().IsCanMove())
            {
                return Status.Failure;
            }
            
            // var ai = actor.GetComponent<ActorAIComponent>();
            // if (ai.target != null)
            // {
            //     MoveToTarget(ai.target.transformComponet.GetPosition());
            // }

            return Status.Success;
        }
    }
}