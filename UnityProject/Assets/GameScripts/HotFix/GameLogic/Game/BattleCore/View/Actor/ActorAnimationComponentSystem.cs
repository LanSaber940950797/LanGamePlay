using ET;
using UnityEngine;

namespace GameLogic.Battle
{
    [EntitySystemOf(typeof(ActorAnimationComponent))]
    public static partial class ActorAnimationComponentSystem
    {
        [EntitySystem]
        public static void Awake(this ActorAnimationComponent self)
        {
            var viewComponent = self.Parent.GetComponent<ViewComponent>();
            var it = viewComponent.ModelGo.GetComponent<AnimationComponent>();
            if (it != null)
            {
                self.ActorAnimation = it;
            }
            
        }


        [EntitySystem]
        public static void Destroy(this ActorAnimationComponent self)
        {
           
        }
        
        
        public static void PlayOnHurt(this ActorAnimationComponent self)
        {
            if (self.ActorAnimation == null)
            {
                return;
            }
            self.ActorAnimation.PlayOnHurt();
        }


        
        public static void PlayAnimationClip(this ActorAnimationComponent self, AnimationClip obj)
        {
            if (self.ActorAnimation == null)
            {
                return;
            }
        
            if (self.ActorAnimation is AnimationComponent comp)
            {
                comp.TryPlayFade(obj);
            }
        }


        public static void PlayIde(this ActorAnimationComponent self)
        {
            if (self.ActorAnimation == null)
            {
                return;
            }
            if ((self.ActorAnimation.Tag & ActorAnimationTag.Idle) > 0)
            {
                return;
            }
        
            self.ActorAnimation.Tag &= ~ActorAnimationTag.Move;
            self.ActorAnimation.Tag |= ActorAnimationTag.Idle;
            self.ActorAnimation.PlayIde();
        }
        
        public static void PlayMove(this ActorAnimationComponent self)
        {
            if (self.ActorAnimation == null)
            {
                return;
            }
            if ((self.ActorAnimation.Tag & ActorAnimationTag.Move) > 0)
            {
                return;
            }
        
            self.ActorAnimation.Tag &= ~ActorAnimationTag.Idle;
            self.ActorAnimation.Tag |= ActorAnimationTag.Move;
            self.ActorAnimation.PlayMove();
        }
        
        public static void PlayAnimation(this ActorAnimationComponent self, string animationName)
        {
            if (self.ActorAnimation == null)
            {
                return;
            }
            self.ActorAnimation.PlayAnimation(animationName);
        }
    }
}