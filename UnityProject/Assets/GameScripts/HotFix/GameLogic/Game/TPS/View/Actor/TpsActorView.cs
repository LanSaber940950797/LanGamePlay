using ET;
using TrueSync;
using UnityEngine;

namespace GameLogic.Battle
{
    [ComponentOf(typeof(ActorView))]
    public class TpsActorView : Entity,IAwake, IUpdate
    {
        public bool IsInitPos = false;
        public ThirdPersonSystem Controller;
        public StarterAssetsInputs Inputs;
        public Actor Actor => GetParent<ActorView>().Actor;
    }
    
    [EntitySystemOf(typeof(TpsActorView))]
    public static partial class TpsActorViewSystem
    {
        [EntitySystem]
        public static void Awake(this TpsActorView self)
        {
            self.IsInitPos = false;
            var viewComponent = self.AddComponent<ViewComponent, LSEntity, string>(self.Actor,null);
            self.LodeMode().NoContext();
            self.Actor.ListenActionPoint(ActionPointType.Move,OnActionMove, self);
            self.Actor.ListenActionPoint(ActionPointType.PreMove,OnActionPreMove, self);
        }
        
        [EntitySystem]
        public static void Update(this TpsActorView self)
        {
            var viewComponent = self.GetComponent<ViewComponent>();
            if (viewComponent.Go == null)
            {
                return;
            }

            if (self.IsInitPos)
            {
                var transformComponent = self.Actor.GetComponent<TransformComponent>();
                transformComponent.Position = viewComponent.Go.transform.position.ToTSVector();
            }
        }
        
        public static async ETTask LodeMode(this TpsActorView self)
        {
            var viewComponent = self.GetComponent<ViewComponent>();
            await viewComponent.LoadMode("Player");
            var transformComponent = self.Actor.GetComponent<TransformComponent>();
            viewComponent.Go.transform.position = transformComponent.Position.ToVector();
            viewComponent.Go.transform.rotation = transformComponent.Rotation.ToQuaternion();
            var root = self.Root();
            self.Controller = viewComponent.ModelGo.GetComponent<ThirdPersonSystem>();
            self.Inputs = viewComponent.ModelGo.GetComponent<StarterAssetsInputs>();
            self.Controller.enabled = false;
            await root.GetComponent<TimerComponent>().WaitAsync((200));
            self.Controller.enabled = true;
            self.IsInitPos = true;
        }

        private static void OnActionMove(Entity entity, Entity actionEntity)
        {
            TpsActorView self = entity.As<TpsActorView>();
            MoveAction action = actionEntity.As<MoveAction>();
            if (self.Controller == null)
            {
                return;
            }

           
            if (action.Snapshot.MoveType == (int)MoveType.MoveDir)
            {
                Vector3 dir = action.Snapshot.Velocity.ToVector();
                self.Inputs.MoveInput(new Vector2(dir.x, dir.z));
            }
            else if(action.Snapshot.MoveType == (int)MoveType.StopMove)
            {
                self.Inputs.MoveInput(new Vector2(0, 0));
            }
            else if(action.Snapshot.MoveType == (int)MoveType.Jump)
            {
                self.Inputs.jump = true;
            }
            
        }
        
        private static void OnActionPreMove(Entity entity, Entity actionEntity)
        {
            TpsActorView self = entity.As<TpsActorView>();
            MoveAction action = actionEntity.As<MoveAction>();
            if (self.Controller == null)
            {
                return;
            }
            if (!action.IsSnapshot)
            {
                action.Snapshot.Position = self.Controller.transform.position.ToTSVector();
                return;
            }
            
            //快照的话判断坐标偏移
            bool check = true;
            
            var curPos = self.Controller.transform.position.ToTSVector();
            var delta = curPos - action.Snapshot.Position;
            //水平
            var deltaH = new  TSVector(delta.x, 0, delta.z);
            var deltaHeight = delta.y;
            if (deltaH.sqrMagnitude > 0.5f || TSMath.Abs(deltaHeight) > 5f)
            {
                if (self.IsServer()) //是主玩家，用服务器坐标
                {
                    action.Snapshot.Position = curPos;
                }
                else
                {
                    self.Controller.transform.position = action.Snapshot.Position.ToVector();
                    Log.Error($"{self.Actor.Id} 移动异常，服务器坐标：{action.Snapshot.Position}，客户端坐标：{curPos}");
                }
            }
        }
    }
}