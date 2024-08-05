using ET;

namespace GameLogic.Battle
{
     /// <summary>
    /// 治疗行动
    /// </summary>
    public class CureAction : LSEntity, IAwake, IServerActionExecution,IDestroy
    {
       
        /// 治疗数值
        public long CureValue { get; set; }

        /// 行动能力
        public LSEntity ActionAbility { get; set; }
        
        /// 效果赋给行动源
        public EffectAssignAction SourceAssignAction { get; set; }
        /// 行动实体
        public Actor Creator { get; set; }
        /// 目标对象
        public Actor Target { get; set; }

        public bool IsSync { get; set; }
        public bool IsSnapshot { get; set; }
        public StateCure Snapshot;

    }
    
    [EntitySystemOf(typeof(CureAction))]
    public static partial class CureActionSystem
    {
        [EntitySystem]
        public static void Destroy(this CureAction self)
        {
            self.Creator = null;
            self.Target = null;
            self.Snapshot = null;
        }
        
        public static void FinishAction(this CureAction self)
        {
            self.Dispose();
        }

        //前置处理
        private static void PreProcess(this CureAction self)
        {
            
        }

        private static void ApplyAction(this CureAction self)
        {
           
            
            self.Target?.GetComponent<AttributeComponent>().ModifyAttribute(AttributeType.Hp, self.CureValue);
            
        }

        //后置处理
        private static void PostProcess(this CureAction self)
        {
            self.Creator?.TriggerActionPoint(ActionPointType.PostGiveCure, self);
            self.Target?.TriggerActionPoint(ActionPointType.PostReceiveCure, self);
        }

        public static void DoAction(this CureAction self)
        {
            self.PreProcess();
            self.ApplyAction();
            self.PostProcess();
            self.SendFrameState();
            self.FinishAction();
        }
        
        private static void SendFrameState(this CureAction self)
        {
            if (!self.IsSync)
            {
                return;
            }

            // 发送帧同步数据
            self.Snapshot = new StateCure();
        }
    }
}