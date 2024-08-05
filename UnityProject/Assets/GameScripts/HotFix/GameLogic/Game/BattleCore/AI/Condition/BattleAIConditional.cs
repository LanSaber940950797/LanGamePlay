using Kurisu.AkiBT;

namespace GameLogic.Battle
{
    public abstract class BattleAIConditional : Conditional

    {
        protected Actor actor;
        protected override void OnAwake()
        {
            var tree = (BattleAITreeSO)Tree;
            actor = tree.Actor;
        }
        
    }
}