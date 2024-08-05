using System.Collections.Generic;
using ET;
using GameConfig.Battle;
using TrueSync;

namespace GameLogic.Battle
{
    

    /// <summary>
    /// 赋给效果行动
    /// </summary>
    public class EffectAssignAction : LSEntity, IAwake, IActionExecution,IDestroy
    {
        /// 创建这个效果赋给行动的源能力
        public LSEntity SourceAbility { get; set; }
        /// 目标行动
        public IActionExecution TargetAction { get; set; }
        public AbilityEffect AbilityEffect { get; set; }
        //public AbilityItem AbilityItem { get; set; }
        public Effect Desc => AbilityEffect.Desc;
        /// 行动能力
        public Entity actionAbility { get; set; }
        /// 效果赋给行动源
        public EffectAssignAction sourceAssignAction { get; set; }

        public LSEntity ActionAbility { get; set; }

        /// 行动实体
        public Actor Creator { get; set; }
        /// 目标对象
        public Actor Target { get; set; }

        public bool IsSync { get; set; }
        public bool IsSnapshot { get; set; }

       
        public Entity AssignTarget { get; set; }
        
        public FP InputPoint { get; set; }
        
        //public AbilityItem AbilityItem { get; set; }
       
    }

    
    [EntitySystemOf(typeof(EffectAssignAction))]
    [LSEntitySystemOf(typeof(EffectAssignAction))]
    public static partial class EffectAssignActionSystem
    {
        [EntitySystem]
        public static void Destroy(this EffectAssignAction self)
        {
            self.Creator = null;
            self.Target = null;
            self.actionAbility = null;
            self.AssignTarget = null;
            self.ActionAbility = null;
            self.AbilityEffect = null;
            self.SourceAbility = null;
        }
        
        
        /// 前置处理
        private static void PreProcess(this EffectAssignAction self)
        {
            if (self.AssignTarget is Actor combatEntity)
            {
                self.Target = combatEntity;
            }
        }

        public static void AssignAction(this EffectAssignAction self)
        {
            //Log.Debug($"ApplyEffectAssign {EffectConfig}");
           

            foreach (var item in self.AbilityEffect.Components.Values)
            {
                if (item is IEffectTriggerSystem effectTriggerSystem)
                {
                    effectTriggerSystem.OnTriggerApplyEffect(self);
                }
            }

         
        }

        /// 后置处理
        private static void PostProcess(this EffectAssignAction self)
        {
            self.Creator.TriggerActionPoint(ActionPointType.AssignEffect, self);
            self.Target?.TriggerActionPoint(ActionPointType.ReceiveEffect, self);
        }

        public static void FinishAction(this EffectAssignAction self)
        {
            self.Dispose();
        }
        
        public static  void DoAction(this EffectAssignAction self)
        {
            self.PreProcess();
            self.AssignAction();
            self.PostProcess();
            self.FinishAction();
        }
    }
}