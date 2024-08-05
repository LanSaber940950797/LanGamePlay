using Kurisu.AkiBT;
namespace GameLogic.Battle
{
    [AkiInfo("Action:退出技能,运行中的技能树需要该结点或相同功能的结点才能正常退出")]
    [AkiLabel("Skill:Exit")]
    [AkiGroup("Skill")]
    public class SkillExit : SkillAction
    {
        protected override Status OnUpdate()
        {
            skillTree.OnSkillExit();
            return  Status.Success;
        }
    }
}