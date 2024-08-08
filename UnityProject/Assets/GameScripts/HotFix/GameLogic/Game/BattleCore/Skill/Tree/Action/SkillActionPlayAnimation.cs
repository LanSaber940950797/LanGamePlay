using Kurisu.AkiBT;
using UnityEngine;

namespace GameLogic.Battle
{
    [AkiInfo("Action:播放动画")]
    [AkiLabel("Skill:播放动画")]
    [AkiGroup("Skill")]
    public class SkillActionPlayAnimation : SkillAction
    {
        public SharedSTObject<Actor> actor;
        public SharedTObject<AnimationClip> animationClip;
        public SharedString animationName;
        
        public override void Awake()
        {
            base.Awake();
            InitVariable(actor);
            InitVariable(animationClip);
        }
        protected override Status OnUpdate()
        {
            Actor actorUnit = actor.Value;
            if (actorUnit == null)
            {
                actorUnit = this.skillTree.Owner;
            }
            var clip = animationClip.Value;
            ActorView actorView = actorUnit.GetActorView();
            if (actorView == null)
            {
                return Status.Failure;
            }
            if (clip != null)
            {
                actorView.GetComponent<ActorAnimationComponent>().PlayAnimationClip(clip);
            }
            else
            {
                actorView.GetComponent<ActorAnimationComponent>().PlayAnimation(animationName.Value);
            }
            
            return Status.Success;
        }
    }
}