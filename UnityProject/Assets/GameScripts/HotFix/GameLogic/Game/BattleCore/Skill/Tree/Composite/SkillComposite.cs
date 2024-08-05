using UnityEngine;
using Kurisu.AkiBT;
namespace GameLogic.Battle
{
    [AkiInfo("Composite:技能")]
    [AkiLabel("Skill:SkillComposite")]
    [AkiGroup("Skill")]
    public class SkillComposite : Composite
    {
        protected SkillTreeSO skillTree;
        protected override void OnAwake() {
            skillTree = (SkillTreeSO)Tree;
        }


        protected override Status OnUpdate()
        {
            return Status.Success;
        }
    }
}