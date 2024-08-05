using System.IO;
using ET;

namespace GameLogic.Battle
{
     /// <summary>
    /// 治疗行动
    /// </summary>
    public class CreateActorAction : LSEntity, IAwake, IDestroy, IServerActionExecution
    {
        /// 行动能力
        public LSEntity ActionAbility { get; set; }
        /// 行动实体
        public Actor Creator { get; set; }
        /// 目标对象
        public Actor Target { get; set; }

        public bool IsSync { get; set; }
        public bool IsSnapshot { get; set; }
        
        public ActorCreateInfo CreateInfo;
        public StateCreateActor Snapshot;
    }

    [EntitySystemOf(typeof(CreateActorAction))]
    public static partial class CreateActorActionSystem
    {

        [EntitySystem]
        public static void Destroy(this CreateActorAction self)
        {
            self.Creator = null;
            self.Target = null;
            self.Snapshot = null;
            self.ActionAbility = null;
            self.CreateInfo = null;
        }
        
        public static void FinishAction(this CreateActorAction self)
        {
            self.Dispose();
        }

        //前置处理
        private static void PreProcess(this CreateActorAction self)
        {
            if (self.IsSnapshot)
            {
                self.Target = MemoryPackHelper.Deserialize(typeof(Actor), self.Snapshot.Data
                    , 0, self.Snapshot.Data.Length) as Actor;
            }
        }

        private static void ApplyCreateActor(this CreateActorAction self)
        {
            var actorComponent = self.LSWorld().GetComponent<ActorComponent>();
            actorComponent.CreateActor(self);
        }

        //后置处理
        private static void PostProcess(this CreateActorAction self)
        {
            self.LSWorld().SendEvent(BattleEvent.ActorCreate, self);
        }

        public static void DoAction(this CreateActorAction self)
        {
            self.PreProcess();
            self.ApplyCreateActor();
            self.PostProcess();
            self.SendFrameState();
            self.FinishAction();
        }
        
        private static void SendFrameState(this CreateActorAction self)
        {
            if (!self.IsSync)
            {
                return;
            }
            
            self.Snapshot.ActorId = self.Target.Id;
            MemoryBuffer memoryBuffer = new(1024);
            memoryBuffer.Seek(0, SeekOrigin.Begin);
            memoryBuffer.SetLength(0);
            MemoryPackHelper.Serialize(self.Target, memoryBuffer);
            memoryBuffer.Seek(0, SeekOrigin.Begin);
            self.Snapshot.Data = memoryBuffer.ToArray();
            self.SendFrameState(self.Snapshot);
           
        }
    }
}