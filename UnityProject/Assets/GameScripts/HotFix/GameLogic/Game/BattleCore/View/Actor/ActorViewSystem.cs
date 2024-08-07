using ET;
using TEngine;
using TrueSync;
using UnityEngine;
using Log = ET.Log;

namespace GameLogic.Battle
{
    [EntitySystemOf(typeof(ActorView))]
    public static partial class ActorViewSystem
    {
        public static ActorView GetActorView(this Actor self)
        {
            var wolrd = self.LSWorld();
            return wolrd.Parent.GetComponent<ActorViewComponent>().GetChild<ActorView>(self.Id);
        }
        
        [EntitySystem]
        public static void Awake(this ActorView self, Actor actor)
        {
            self.Actor = actor;
            self.IsInitPos = false;
            var viewComponent = self.AddComponent<ViewComponent, LSEntity, string>(actor,null);
            self.LodeMode().NoContext();
            self.Actor.ListenActionPoint(ActionPointType.Move,OnActionMove, self);
            self.Actor.ListenActionPoint(ActionPointType.PreMove,OnActionPreMove, self);
        }

       


        [EntitySystem]
        public static void Update(this ActorView self)
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
        
        public static async ETTask LodeMode(this ActorView self)
        {
            var viewComponent = self.GetComponent<ViewComponent>();
            await viewComponent.LoadMode("Player");
            var transformComponent = self.Actor.GetComponent<TransformComponent>();
            self.Controller = viewComponent.ModelGo.GetComponent<ThirdPersonSystem>();
            self.Inputs = viewComponent.ModelGo.GetComponent<StarterAssetsInputs>();
            self.Controller.enabled = false;
           
            viewComponent.Go.transform.position = transformComponent.Position.ToVector();
            viewComponent.Go.transform.rotation = transformComponent.Rotation.ToQuaternion();
            var root = self.Root();
            await root.GetComponent<TimerComponent>().WaitAsync((200));
            self.Controller.enabled = true;
            self.IsInitPos = true;
        }

        private static void OnActionMove(Entity entity, Entity actionEntity)
        {
            ActorView self = entity.As<ActorView>();
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
            ActorView self = entity.As<ActorView>();
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
                if (self.IsMaster()) //是主玩家，用服务器坐标
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

        // public static Vector3 GetTargetDir(this ActorView self, Vector3 input)
        // {
        //     if (self.Controller == null)
        //     {
        //         return  Vector3.zero;
        //     }
        //
        //     return self.Controller.GetTargetDirection(input);
        // }


    }
}