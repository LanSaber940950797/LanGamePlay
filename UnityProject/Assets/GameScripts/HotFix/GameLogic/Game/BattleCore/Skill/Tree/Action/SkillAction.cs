using Kurisu.AkiBT;
namespace GameLogic.Battle
{
    public abstract class SkillAction : Action
    {
        protected SkillTreeSO skillTree;

        public override void Awake()
        {
            skillTree = (SkillTreeSO)Tree;
        }
        
    }
}