using ET;
using TrueSync;

namespace GameLogic.Battle
{
    public enum MoveType{
        None = 0,
        MoveDir = 1,
        StopMove = 2,
        Jump = 3,
    }
    
    
     /// <summary>
    /// 治疗行动
    /// </summary>
    [ChildOf]
    public class MoveAction : LSEntity, IAwake, IActionExecution, IDestroy
    {
        /// 行动能力
        public LSEntity ActionAbility { get; set; }
        /// 行动实体
        public Actor Creator { get; set; }
        /// 目标对象
        public Actor Target { get; set; }

        public bool IsSync { get; set; }
        public bool IsSnapshot { get; set; }
        public StateMove Snapshot;

        public TrueSync.TSVector Velocity
        {
            get=> Snapshot.Velocity;
            set=> Snapshot.Velocity = value;
        }

        public TrueSync.TSVector Position
        {
            get=> Snapshot.Position;
            set=> Snapshot.Position = value;
        }
        public int MoveType
        {
            get=> Snapshot.MoveType;
            set=> Snapshot.MoveType = value;
        }
    }

    [EntitySystemOf(typeof(MoveAction))]
    public static partial class MoveActionSystem
    {
        [EntitySystem]
        public static void Destroy(this MoveAction self)
        {
            self.Creator = null;
            self.Target = null;
            self.ActionAbility = null;
            self.Snapshot = null;
        }
        
        public static void FinishAction(this MoveAction self)
        {
            self.Dispose();
        }

        //前置处理
        private static void PreProcess(this MoveAction self)
        {
            self.Creator.TriggerActionPoint(ActionPointType.PreMove, self);
        }

        private static void ApplyAction(this MoveAction self)
        {
            self.Creator.TriggerActionPoint(ActionPointType.Move, self);
        }

        //后置处理
        private static void PostProcess(this MoveAction self)
        {
           
        }

        public static void DoAction(this MoveAction self)
        {
            self.PreProcess();
            self.ApplyAction();
            self.PostProcess();
            self.SendFrameState();
            self.FinishAction();
        }
        
        private static void SendFrameState(this MoveAction self)
        {
            if (!self.IsSync)
            {
                return;
            }
            self.SendFrameState(self.Snapshot);
        }

      
    }
}