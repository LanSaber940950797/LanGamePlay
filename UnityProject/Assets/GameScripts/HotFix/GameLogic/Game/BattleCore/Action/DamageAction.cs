
using ET;
using Log = TEngine.Log;


namespace GameLogic.Battle
{

    
   
    public class DamageAction : LSEntity, IAwake, IServerActionExecution,IDestroy
    {
        public DamageActionAbility DamageAbility => ActionAbility as DamageActionAbility;
        /// 伤害数值
        public long DamageValue { get; set; }
        //实际造成伤害
        public long RealDamageValue { get; set; }
        /// 是否是暴击
        public bool IsCritical { get; set; }
        
        //伤害类型
        public DamageType DamageType { get; set; }
        
        /// 行动能力
        public LSEntity ActionAbility { get; set; }
        /// 效果赋给行动源
        public EffectAssignAction sourceAssignAction { get; set; }

        

        /// 行动实体
        public Actor Creator { get; set; }
        /// 目标对象
        public Actor Target { get; set; }

        public bool IsSync { get; set; }
        public bool IsSnapshot { get; set; }
        public StateDamage Snapshot { get; set; }
    }
    
    [EntitySystemOf(typeof(DamageAction))]
    public static partial class DamageActionSystem
    {
        [EntitySystem]
        public  static void Destroy(this DamageAction self)
        {
            self.Creator = null;
            self.Target = null;
            self.Snapshot = null;
        }
        
        public static void FinishAction(this DamageAction self)
        {
            self.Dispose();
        }

        /// 前置处理
        private static void PreProcess(this DamageAction self)
        {
            self.CalculateDamage();

            //触发 造成伤害前 行动点
            self.Creator?.TriggerActionPoint(ActionPointType.PreCauseDamage, self);
            //触发 承受伤害前 行动点
            self.Target?.TriggerActionPoint(ActionPointType.PreReceiveDamage, self);
        }
        
        //计算最终伤害
        private static void CalculateDamage(this DamageAction self)
        {
            self.RealDamageValue = self.DamageValue;
        }
        

        /// 应用伤害
        public static void ApplyDamage(this DamageAction self)
        {
            self.PreProcess();

            Log.Debug($"DamageAction ApplyDamage");
            
            var cur = self.Target.GetComponent<AttributeComponent>().GetAttribute(AttributeType.Hp);
            cur -= self.RealDamageValue;
            if (cur <= 0)
            {
                cur = 0;
            }
            self.Target.GetComponent<AttributeComponent>().SetAttribute(AttributeType.Hp, cur);
            self.PostProcess();

            // if (self.Target.CheckDead())
            // {
            //     self.Target.OnDead();
            // }

           
        }

        /// 后置处理
        private static void PostProcess(this DamageAction self)
        {
            //触发 造成伤害后 行动点
            self.Creator?.TriggerActionPoint(ActionPointType.PostCauseDamage, self);
            //触发 承受伤害后 行动点
            self.Target?.TriggerActionPoint(ActionPointType.PostReceiveDamage, self);
        }

        public static void DoAction(this DamageAction self)
        {
            self.ApplyDamage();
            self.SendFrameState();
            self.FinishAction();
        }
        
        private static void SendFrameState(this DamageAction self)
        {
            if (!self.IsSync)
            {
                return;
            }

            //todo 发送帧同步
        }
    }
}