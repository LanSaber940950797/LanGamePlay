using Kurisu.AkiBT;
namespace GameLogic.Battle
{
    public abstract class BattleAIAction : Action
    {
        protected Actor actor;

        public override void Awake()
        {
            var tree = (BattleAITreeSO)Tree;
            actor = tree.Actor;
        }
    }
}