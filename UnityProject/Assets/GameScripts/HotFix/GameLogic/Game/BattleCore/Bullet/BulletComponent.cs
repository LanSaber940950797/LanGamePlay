// using Sog;
// using Sog.ECS;
//
// namespace GameLogic.Battle
// {
//     public class BulletComponent : Component,IUpdateLogicFrameComponent
//     {
//         
//         //能力执行体
//         public IAbilityExecute AbilityExecution { get; set; }
//         //能力实体
//         public Entity AbilityEntity => AbilityExecution.abilityEntity;
//         public IAbilityEntity Ability => AbilityEntity as IAbilityEntity;
//         public float Duration;
//         public ActorUnit Bullet => Entity as ActorUnit;
//
//         public override void Awake(object initData)
//         {
//             Duration = 0;
//             AbilityExecution = initData as IAbilityExecute;;
//         }
//         
//         public  void UpdateLogicFrame(int logicTimeMs)
//         {
//             if (Duration <= 0)
//             {
//                 return;
//             }
//             
//             Duration -= GameTime.LogicTimeOneFrame;
//             if (Duration <= 0)
//             {
//                 Bullet.OnDead();
//             }
//         }
//     }
// }