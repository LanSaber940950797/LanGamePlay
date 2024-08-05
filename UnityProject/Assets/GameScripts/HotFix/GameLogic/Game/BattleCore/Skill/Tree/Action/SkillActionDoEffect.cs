using Kurisu.AkiBT;
using UnityEngine;

namespace GameLogic.Battle
{
    [AkiInfo("Action:执行技能效果")]
    [AkiLabel("Skill:DoEffect")]
    [AkiGroup("Skill")]
    public class SkillActionDoEffect : SkillAction
    {
        [AkiLabel("目标")]
        [SerializeField]
        public SharedSTObject<Actor> target;
        [SerializeField]
        public SharedInt effectIndex;
        
        public override void Awake()
        {
            base.Awake();
            InitVariable(target);
            InitVariable(effectIndex);
        }
        protected override Status OnUpdate()
        {
            var execute = skillTree.Execute;
            var actor = target.Value;
            if (actor == null)
            {
                return Status.Success;
            }
            
            var effectAssign = execute.AbilityEntity.GetComponent<AbilityEffectComponent>().CreateAssignAction(actor, effectIndex.Value);
            effectAssign.DoAction();
            return Status.Success;
        }
    }
}