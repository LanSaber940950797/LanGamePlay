using Kurisu.AkiBT;

namespace GameLogic.Battle
{
    public class SkillDecorator : Decorator
    {
        protected SkillTreeSO skillTree;

        protected override void OnStart()
        {
            skillTree = (SkillTreeSO)Tree;
        }
    }
}