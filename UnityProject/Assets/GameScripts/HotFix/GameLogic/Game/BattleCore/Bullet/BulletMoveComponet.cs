// using Sog;
// using Sog.ECS;
//
// namespace GameLogic.Battle
// {
//     public class BulletMoveComponet : Component,IUpdateLogicFrameComponent
//     {
//         public float speed;
//         public ActorUnit bullet => Entity as ActorUnit;
//        
//         public void UpdateLogicFrame(int logicTimeMs)
//         {
//             var dir = bullet.transformComponet.GetForward().normalized * speed * GameTime.LogicTimeOneFrame;
//             bullet.transformComponet.Translate(dir);
//         }
//     }
// }