using Kurisu.AkiBT;

namespace GameLogic.Battle
{
    [AkiInfo("Action:死亡")]
    [AkiLabel("BattleAI:死亡")]
    [AkiGroup("BattleAI")]
    public class DeadAction : Action
    {
        protected override Status OnUpdate()
        {
            return Status.Success;
        }
    }
   
}