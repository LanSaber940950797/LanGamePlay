
using ET;
using TEngine;
using UnityEngine;


namespace GameLogic.Battle
{
    [ComponentOf(typeof(ActorView))]
    public class ActorAnimationComponent : Entity,IAwake, IDestroy
    {
       
       
        public IActorAnimation _actorAnimation;

        public IActorAnimation ActorAnimation;
        
        
        public void OnPostReceiveDamage(Entity action)
        {
            if (ActorAnimation == null)
            {
                return;
            }
            ActorAnimation.PlayOnHurt();
        }


        
        public void PlayAnimationClip(AnimationClip obj)
        {
            if (ActorAnimation == null)
            {
                return;
            }

            if (ActorAnimation is AnimationComponent comp)
            {
                comp.TryPlayFade(obj);
            }
        }


        public void PlayIde()
        {
            if (ActorAnimation == null)
            {
                return;
            }
            if ((ActorAnimation.Tag & ActorAnimationTag.Idle) > 0)
            {
                return;
            }

            ActorAnimation.Tag &= ~ActorAnimationTag.Move;
            ActorAnimation.Tag |= ActorAnimationTag.Idle;
            ActorAnimation.PlayIde();
        }

        public void PlayMove()
        {
            if (ActorAnimation == null)
            {
                return;
            }
            if ((ActorAnimation.Tag & ActorAnimationTag.Move) > 0)
            {
                return;
            }

            ActorAnimation.Tag &= ~ActorAnimationTag.Idle;
            ActorAnimation.Tag |= ActorAnimationTag.Move;
            ActorAnimation.PlayMove();
        }

        public void PlayAnimation(string animationName)
        {
            if (ActorAnimation == null)
            {
                return;
            }
            ActorAnimation.PlayAnimation(animationName);
        }
    }
}