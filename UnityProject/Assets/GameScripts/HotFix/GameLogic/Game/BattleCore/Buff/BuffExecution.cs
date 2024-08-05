
using ET;

namespace GameLogic.Battle
{
    /// <summary>
    /// 状态能力执行体
    /// </summary>
    public  class BuffExecution : LSEntity, IAbilityExecute, IAwake<BuffAbility>, ILSUpdate
    {
        public LSEntity AbilityEntity { get; set; }
        public Actor Owner { get; set; }

        public IAbilityEntity Ability { get; set; }
       

        public BuffAbility Buff;
        public int TriggerTimes;
        public int CanTriggerTimes;

        //触发行为
        public IActionExecution TriggerAction;
        public float? TriggerIntervalRemainDuration;
        public bool IsNeedRefresh;

        public int LastUpdateTime;
        
        public void EndExecute()
        {
            BuffExecutionSystem.EndExecute(this);
        }

    }
    
    
    [EntitySystemOf(typeof(BuffExecution))]
    [LSEntitySystemOf(typeof(BuffExecution))]
    public static partial class BuffExecutionSystem
    {
        [EntitySystem]
        public static void Awake(this BuffExecution self, BuffAbility buff)
        {
            self.Buff = buff;
            self.AbilityEntity = buff;
            self.Ability = buff;
            self.Owner = buff.GetParent<Actor>();
            self.TriggerTimes = 0;
            self.CanTriggerTimes = 0;
            self.TriggerIntervalRemainDuration = null;
        }

        public static void BeginExecute(this BuffExecution self)
        {
            //self.Buff.GetComponent<AbilityEffectComponent>().Enable = true;
            if (self.Buff.SkillTreeComponent != null)
            {
                self.Buff.SkillTreeComponent.Enable = true;
            }
           
            if ((self.Buff.Desc.TriggerMode & EBuffTriggerMethod.OnAction) > 0)
            {
                self.Owner.ListenActionPoint(self.Buff.Desc.TriggerAction, OnBuffTriggerAction, self);
            }
            
            if ((self.Buff.Desc.TriggerMode & EBuffTriggerMethod.Repeat) > 0)
            {
                self.TriggerIntervalRemainDuration = self.Buff.Desc.TriggerInterval;
            }
            
            if ((self.Buff.Desc.RemoveMode & EBuffRemoveMethod.OnAction) > 0)
            {
                self.Owner.ListenActionPoint(self.Buff.Desc.TriggerAction, OnBuffRemoveAction, self);
            }
        }

        public static void EndExecute(BuffExecution self)
        {
            self.Buff.Dispose();
        }

        [EntitySystem]
        public static void LSUpdate(this BuffExecution self)
        {
            var nowTime = self.FixFrameTime();
            if (nowTime <= self.LastUpdateTime)
            {
                return;
            }
            var deltaTime = nowTime - self.LastUpdateTime;
            self.Buff.Duration -= deltaTime;
            if (self.Buff.Duration <= 0)
            {
                if (self.Buff.Desc.DecreaseLayer > 0)
                {
                    self.Buff.Layer -= self.Buff.Desc.DecreaseLayer;
                }
                else
                {
                    self.Buff.Layer = 0;
                }

                self.Buff.Duration = self.Buff.Desc.Duration;
            }

            if (self.Buff.Layer <= 0
                || ((self.Buff.Desc.RemoveMode & EBuffRemoveMethod.OnMaxLayer) > 0 && self.Buff.Layer == self.Buff.Desc.MaxLayer)
                || ((self.Buff.Desc.RemoveMode & EBuffRemoveMethod.OnMaxLayer) > 0 && self.TriggerTimes >= self.Buff.Desc.MaxTriggerTimes))
            {
                self.EndExecute();
                return;
            }

            if (self.TriggerIntervalRemainDuration != null)
            {
                if (self.TriggerIntervalRemainDuration > 0) {
                    self.TriggerIntervalRemainDuration -= deltaTime;
                } 
                else {
                    self.TriggerIntervalRemainDuration = self.Buff.Desc.TriggerInterval;
                    self.DoTrigger();
                }
            }
            
        }

        private static void OnBuffTriggerAction(Entity entity, Entity action)
        {
            BuffExecution self = entity.As<BuffExecution>();
            self.TriggerAction = action as IActionExecution;
            self.DoTrigger();
        }
        
        private static void OnBuffRemoveAction(Entity entity, Entity action)
        {
            BuffExecution self = entity.As<BuffExecution>();
            self.EndExecute();
        }

        private static void DoTrigger(this BuffExecution self)
        {
            self.CanTriggerTimes++;
        }

        public static void OnFinishTrigger(this BuffExecution self)
        {
            self.TriggerTimes++;
        }

        public static bool IsCanTrigger(this BuffExecution self)
        {
            return self.CanTriggerTimes > self.TriggerTimes;
        }

        public static void OnChangeLayer(this BuffExecution self)
        {
            self.IsNeedRefresh = true;
        }
    }
}