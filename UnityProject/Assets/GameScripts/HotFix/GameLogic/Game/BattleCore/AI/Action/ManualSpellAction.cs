using System;
using ET;
using Kurisu.AkiBT;
using UnityEngine;
namespace GameLogic.Battle
{
    
    [AkiInfo("Action:手动施法")]
    [AkiLabel("BattleAI:手动施法")]
    [AkiGroup("BattleAI")]
    public  class ManualSpellAction : BattleAIAction
    {
        [AkiLabel("技能ID")]
        public int SkillId;
        protected override Status OnUpdate()
        {
            var skillComponent = this.actor.GetComponent<SkillComponent>();
            var skill = skillComponent.GetSkill(SkillId);
            if (skill == null)
            {
                return Status.Failure;
            }
            var input = actor.LSWorld().Parent.GetComponent<InputControlComponent>();
            input.AttackDir = actor.GetComponent<TransformComponent>().Position.ToVector() - input.AttackPos;
            SpellCastParam castParam = new SpellCastParam()
            {
                skill = skill,
                type = SpellTargetType.None,
                pos = input.AttackPos.ToTSVector(),
                attackDir = input.AttackDir.ToTSVector(),
            };
            if (skillComponent.TrySpell(skill, castParam))
            {
                return Status.Success;
            }
            
            return  Status.Failure;
        }
    }
}