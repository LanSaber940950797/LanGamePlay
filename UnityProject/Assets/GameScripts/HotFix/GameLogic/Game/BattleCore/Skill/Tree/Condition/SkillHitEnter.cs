using System;
using Kurisu.AkiBT;
using UnityEngine;

namespace GameLogic.Battle
{
    [AkiInfo("Conditional:技能打击进入")]
    [AkiLabel("Skill:技能打击")]
    [AkiGroup("Skill")]
    public class SkillHitEnter : SkillConditional
    {
        protected SkillAbility skillAbility;

        [AkiLabel("要求打击次数")]
        [SerializeField]
        public int hitTimes;

        protected override void OnAwake()
        {
            base.OnAwake();
            skillAbility = skillTree.Ability as SkillAbility;
        }
        
        protected override bool IsUpdatable()
        {
            return hitTimes == skillAbility.CurrentHitTimes;
        }
    }
}