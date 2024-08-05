// using System.Collections.Generic;
// using Sog.ECS;
// using UnityEngine;
// using Component = Sog.ECS.Component;
//
// namespace GameLogic.Battle
// {
//     public class BulletAIComponent : Component,IUpdateLogicFrameComponent
//     {
//         public ActorUnit bullet => Entity as ActorUnit;
//         public SpellTargetType targetType;
//         public ActorUnit target;
//         public Vector3 targetPos;
//         public TargetSideType targetSideType;
//         public BulletComponent bulletComponent;
//         public bool isDestryWhenDoEffect = false;
//         public bool isHit;
//         public override void Awake(object initData)
//         {
//             isDestryWhenDoEffect = false;
//             isHit = false;
//             bulletComponent = bullet.GetComponent<BulletComponent>();
//         }
//
//         public void Init(SpellTargetType type,TargetSideType targetSideType, ActorUnit target, Vector3 pos)
//         {
//             this.targetType = type;
//             this.targetSideType = targetSideType;
//             this.target = target;
//             this.targetPos = pos;
//         }
//
//         public void UpdateLogicFrame(int logicTimeMs)
//         {
//             if (!isHit)
//             {
//                 var targets = FindTargets();
//                 foreach (var it in targets)
//                 {
//                     DoEffect(it);
//                 }
//
//                 if (targets.Count > 0)
//                 {
//                     isHit = true;
//                     if (isDestryWhenDoEffect)
//                     {
//                         bullet.OnDead();
//                     }
//                 }
//             }
//             
//         }
//
//         private void DoEffect(ActorUnit target)
//         {
//             var effectComp = bulletComponent.AbilityEntity.GetComponent<AbilityEffectComponent>();
//             for (int i = 1; i <= effectComp.abilityEffects.Count; i++)
//             {
//                 var assignAction = effectComp.CreateAssignAction(target, i);
//                 assignAction.AssignEffect();
//             }
//         }
//
//         public List<ActorUnit> FindTargets()
//         {
//             List<ActorUnit> targets = new List<ActorUnit>();
//             if (targetType == SpellTargetType.Ponit)
//             {
//                 var p1 = bullet.transformComponet.GetPosition();
//                 var list = ActorSelectHelper.GetActors(bullet, true, targetSideType, true);
//                 foreach (var actor in list)
//                 {
//                     //距离
//                     if (actor.transformComponet.OverlapsWithTransform(bullet.transformComponet))
//                     {
//                         targets.Add(actor);
//                     }
//                 }
//             }
//
//             return targets;
//         }
//     }
// }