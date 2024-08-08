using System;
using Kurisu.AkiBT;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameLogic.Battle
{
    [AkiInfo("Action:获取技能效果的目标")]
    [AkiLabel("Skill:GetEffectActor")]
    [AkiGroup("Skill")]
    public class SkillActionGetEffectActor : SkillAction
    {
        [SerializeField, Tooltip("目标类型")]
        public EffectiveActorType effectiveActorType;

        [SerializeField, Tooltip("目标")] 
        public SharedSTObject<Actor> actorShared;
        
      


        protected override Status OnUpdate()
        {
            GetEffectActor();
            return Status.Success;
        }
        
        protected void GetEffectActor()
        {
            var execute = skillTree.Execute;
            Actor actor = null;
            if (effectiveActorType == EffectiveActorType.Caster)
            {
                actor = execute.Owner;
            }
            else if(effectiveActorType == EffectiveActorType.Owner)
            {
                actor = execute.Ability.Owner;
            }
            else if(execute is SkillExecute skillExecute)
            {
                actor = skillExecute.Target;
            }

            if (actor == null)
            {
                return;
            }
            
            actorShared.Value = actor;
        }
    }
}