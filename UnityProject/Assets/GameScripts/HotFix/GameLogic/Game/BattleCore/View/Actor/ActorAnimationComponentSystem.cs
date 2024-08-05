using ET;

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
        
        
        public static void OnPostReceiveDamage(Entity action)
        {
            // if (ActorAnimation == null)
            // {
            //     return;
            // }
            // ActorAnimation.PlayOnHurt();
        }


        
        // public void PlayAnimationClip(AnimationClip obj)
        // {
        //     if (ActorAnimation == null)
        //     {
        //         return;
        //     }
        //
        //     if (ActorAnimation is AnimationComponent comp)
        //     {
        //         comp.TryPlayFade(obj);
        //     }
        // }


        // public void PlayIde()
        // {
        //     if (ActorAnimation == null)
        //     {
        //         return;
        //     }
        //     if ((ActorAnimation.Tag & ActorAnimationTag.Idle) > 0)
        //     {
        //         return;
        //     }
        //
        //     ActorAnimation.Tag &= ~ActorAnimationTag.Move;
        //     ActorAnimation.Tag |= ActorAnimationTag.Idle;
        //     ActorAnimation.PlayIde();
        // }
        //
        // public void PlayMove()
        // {
        //     if (ActorAnimation == null)
        //     {
        //         return;
        //     }
        //     if ((ActorAnimation.Tag & ActorAnimationTag.Move) > 0)
        //     {
        //         return;
        //     }
        //
        //     ActorAnimation.Tag &= ~ActorAnimationTag.Idle;
        //     ActorAnimation.Tag |= ActorAnimationTag.Move;
        //     ActorAnimation.PlayMove();
        // }
        //
        // public void PlayAnimation(string animationName)
        // {
        //     if (ActorAnimation == null)
        //     {
        //         return;
        //     }
        //     ActorAnimation.PlayAnimation(animationName);
        // }
    }
}