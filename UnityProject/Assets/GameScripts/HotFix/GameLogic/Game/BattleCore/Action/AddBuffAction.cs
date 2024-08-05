using ET;
using GameConfig;
using Log = TEngine.Log;

namespace GameLogic.Battle
{
    
    
     /// <summary>
    /// 施加状态行动
    /// </summary>
    public class AddBuffAction : LSEntity, IAwake, IServerActionExecution, IDestroy
    {
        public LSEntity SourceAbility { get; set; }
        
        public BuffAbility Buff { get; set; }

        /// 行动能力
        public LSEntity ActionAbility { get; set; }

        public Actor Creator { get; set; }
        public Actor Target { get; set; }
        public bool IsSync { get; set; }
        public bool IsSnapshot { get; set; }


        /// 效果赋给行动源
        public EffectAssignAction SourceAssignAction { get; set; }
        /// 行动实体

        public int BuffId;//要添加的buffid
        public int AddLayer; //要添加的buff层数
        public StateAddBuff Snapshot;
       

        
    }
     
    [EntitySystemOf(typeof(AddBuffAction))]
    public static partial class AddBuffActionSystem
    {
        [EntitySystem]
        public static void Destroy(this AddBuffAction self)
        {
            self.Snapshot = null;
            self.Creator = null;
            self.Target = null;
        }
        
        
        public static void FinishAction(this AddBuffAction self)
        {
            self.Dispose();
        }

        //前置处理
        private static void PreProcess(this AddBuffAction self)
        {
           
        }

        private static void ApplyAction(this AddBuffAction self)
        {
           
            // var buff = self.Target.buffComponent.GetBuff(self.BuffId);
            // if (buff != null)
            // {
            //     buff.RefreshBuff(self.AddLayer);
            //     return;
            // }
            //
            // buff = self.Target.AttachAbility<BuffAbility>(Desc);
            // buff.owner = self.Creator;
            // buff.TryActivateAbility();
           
        }

        public static void DoAction(this AddBuffAction self)
        {
            self.PreProcess();
            self.ApplyAction();
            self.PostProcess();
            self.SendFrameState();
            self.FinishAction();
        }
        
        //后置处理
        private static void PostProcess(this AddBuffAction self)
        {
            self.Creator.TriggerActionPoint(ActionPointType.PostGiveBuff, self);
            self.Target.TriggerActionPoint(ActionPointType.PostReceiveBuff, self);
        }
        
        private static void SendFrameState(this AddBuffAction self)
        {
            if (!self.IsSync)
            {
                return;
            }

            self.Snapshot.BuffId = self.BuffId;
            self.Snapshot.TargetId = self.Target.Id;
            self.Snapshot.Layer = self.AddLayer;
            self.SendFrameState(self.Snapshot);

        }
    }
}