using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using DG.Tweening;
using Sirenix.OdinInspector;
using TEngine;

namespace GameLogic.Battle
{
    [Flags]
    public enum ActorAnimationTag
    {
        None = 0,
        Idle = 1,
        Move = 1 << 1,
        Attack = 1 << 2,
    }
   
    public interface IActorAnimation
    {
        public ActorAnimationTag Tag { get; set; }
        public void PlayIde();
        public void PlayMove();

        public void PlayOnHurt();
        public void PlayAnimation(string animationName);
    }
    public class AnimationComponent : MonoBehaviour, IActorAnimation
    {
        public Animancer.AnimancerComponent AnimancerComponent;
        public AnimationClip IdleAnimation;
        public AnimationClip RunAnimation;
        public AnimationClip JumpAnimation;
        public AnimationClip AttackAnimation;
        public AnimationClip SkillAnimation;
        public AnimationClip StunAnimation;
        public AnimationClip DamageAnimation;
        public AnimationClip DeadAnimation;
        public AnimationClip[] AnimationClips;
        public string[] AnimationClipNames;

        private ActorAnimationTag animationTag = 0;

        public float Speed { get; set; } = 1f;

        private Dictionary<string, AnimationClip> animationClipDic = new Dictionary<string, AnimationClip>();
        private void Start()
        {

            AnimancerComponent.Animator.fireEvents = false;
            AddAnimationClip(IdleAnimation);
            AddAnimationClip(RunAnimation);
            AddAnimationClip(JumpAnimation);
            AddAnimationClip(AttackAnimation);
            AddAnimationClip(SkillAnimation);
            AddAnimationClip(StunAnimation);
            AddAnimationClip(DamageAnimation);
            foreach (var item in AnimationClips)
            {
                AddAnimationClip(item);
            }
        }

        private void AddAnimationClip(AnimationClip clip)
        {
            if (clip != null)
            {
                animationClipDic.Add(clip.name, clip);
                AnimancerComponent.States.CreateIfNew(clip);
            }
        }


        public ActorAnimationTag Tag { get; set; }

        public void PlayIde()
        {
           
            PlayFade(IdleAnimation);
        }

        public void PlayMove()
        {
            PlayFade(RunAnimation);
        }

        public void PlayOnHurt()
        {
            if (DamageAnimation != null)
            {
                PlayFade(DamageAnimation);
            }
        }

        public void PlayAnimation(string animationName)
        {
            if (animationClipDic.TryGetValue(animationName, out var clip))
            {
                PlayFade(clip);
            }
        }

        // public void Play(AnimationClip clip)
        // {
        //     Log.Debug($"Play anim {clip.name}");
        //     var state = AnimancerComponent.States.GetOrCreate(clip);
        //     state.Speed = Speed;
        //     AnimancerComponent.Play(state);
        //
        // }

        private void OnAnimationEnd()
        {
            if ((animationTag & ActorAnimationTag.Move) > 0)
            {
                PlayFade(RunAnimation);
            }
            else
            {
                PlayFade(IdleAnimation);
            }
        }

        private void PlayFade(AnimationClip clip)
        {
            var state = AnimancerComponent.States.GetOrCreate(clip);
            state.Speed = Speed;
            state.Time = 0;
            AnimancerComponent.Play(state, 0.25f);
            if (clip != IdleAnimation && clip != RunAnimation)
            {
                state.Events.OnEnd = OnAnimationEnd;
            }
        }

        public void TryPlayFade(AnimationClip clip)
        {
            var state = AnimancerComponent.States.GetOrCreate(clip);
            state.Speed = Speed;
            state.Time = 0;
            if ((animationTag & ActorAnimationTag.Move) > 0)
            {
                AnimancerComponent.Stop(RunAnimation);
            }
            else
            {
                AnimancerComponent.Stop(IdleAnimation);

                AnimancerComponent?.Play(state, 0.25f);
                if (clip != IdleAnimation && clip != RunAnimation)
                {
                    state.Events.OnEnd = OnAnimationEnd;
                }
            }
        }
    }
}
