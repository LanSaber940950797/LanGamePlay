using Kurisu.AkiBT;

namespace GameLogic.Battle
{
    public abstract class SkillConditional : Conditional
    {
        protected SkillTreeSO skillTree;

        protected override void OnAwake()
        {
            skillTree = (SkillTreeSO)Tree;
        }
    }
}