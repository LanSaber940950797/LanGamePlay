using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ET;
using GameConfig;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using TEngine;
using TrueSync;


namespace GameLogic.Battle
{
  
    public class SpellCastParam
    {
        public SkillAbility skill;
        public SpellTargetType type; // 1 对目标 2 对位置
       
        public Actor target;
        public long targetId;
        public TSVector pos;
        public TSVector attackDir;
        public TSVector attackPos;
    }
    /// <summary>
    /// 技能能力实体类
    /// </summary>
    [ChildOf(typeof(SkillComponent))]
    [MemoryPackable]
    public partial class SkillAbility : LSEntity, IAwake<int, int>, ILSUpdate, IAbilityEntity,ISerializeToEntity,IDeserialize
    {
        [MemoryPackIgnore]
        [BsonIgnore]
        public Actor Owner
        {
            get { return Parent.GetParent<Actor>();}
            set{} 
        }
        [MemoryPackIgnore]
        [BsonIgnore]
        public Actor ParentActor { get => Parent.GetParent<Actor>(); }
        public bool Enable { get; set; }

        [MemoryPackIgnore] 
        [BsonIgnore] 
        public SpellDesc Desc;

        public int CoolDownTime;
        public int CurrentCoolDownTime;
        public int LastCheckCDTime;
        //配置
        //技能id
        public int SkillId;
        public int Level;
        //是否正在施法
        public bool Spelling;
        
        [MemoryPackIgnore]
        [BsonIgnore]
        public SkillExecute SkillExecute;

        public int MaxHitTimes;
        public int CurrentHitTimes;
        public int HitInterval;
        
    }

    [EntitySystemOf(typeof(SkillAbility))]
    [LSEntitySystemOf(typeof(SkillAbility))]
    public static partial class SkillAbilitySystem
    {
        [EntitySystem]
        public static void Awake(this SkillAbility self, int skillId, int skillLevel)
        {
            SpellDesc desc = ConfigSystem.Instance.Tables.TbSpell.Get(skillId, skillLevel);
            self.Desc = desc;
            self.SkillId = skillId;
            self.Level = skillLevel;
            self.MaxHitTimes = (int)Math.Max(1, desc.MaxHit);
            self.CoolDownTime = self.Desc.CoolDownTime;
            //添加效果组件
            var effectDesc = ConfigSystem.Instance.Tables.TbEffect.Get(desc.Id, desc.Level);
            self.AddComponent<AbilityEffectComponent, EffectDesc>(effectDesc);
            self.AddComponent<SkillTreeComponent, string>(desc.TreeName);
        }
        
        public static SkillExecute CreateExecution(this SkillAbility self)
        {
            self.SkillExecute = self.Owner.AddChild<SkillExecute, SkillAbility>(self);
            if (self.HitInterval > 0)
            {
                self.CurrentHitTimes++;
                if (self.CurrentHitTimes > self.MaxHitTimes)
                {
                    self.CurrentHitTimes = 1;
                }
            }
            else
            {
                self.CurrentHitTimes = 1;
            }

            self.HitInterval = self.Desc.HitInterval;
            //初始化执行体
            self.SetCoolDown();
            self.GetComponent<SkillTreeComponent>().Init(self, self.SkillExecute);
            return self.SkillExecute;
        }
        
        public static bool CheckCondition(this SkillAbility self, SpellCastParam castParam)
        {
            if (self.IsInCoolDown())
            {
                return false;
            }

            return true;
        }
        
        public static void SetCoolDown(this SkillAbility self)
        {
            self.CurrentCoolDownTime = self.CoolDownTime;
        }
        
        public static bool IsSpelling(this SkillAbility self)
        {
            return self.Spelling;
        }
        
        
        public static bool IsInCoolDown(this SkillAbility self)
        {
            return self.CurrentCoolDownTime > 0;
        }
        
        public static void ChangeCoolDownSpeed(this SkillAbility self, int speed)
        {
           
            self.CoolDownTime = self.Desc.CoolDownTime * speed / 1000;
            if (self.CurrentCoolDownTime > self.CoolDownTime)
            {
                self.CurrentCoolDownTime = self.CoolDownTime;
            }
        }
        
        [LSEntitySystem]
        public static void LSUpdate(this SkillAbility self)
        {
            if (self.IsSpelling())
            {
                return;
            }

            var nowTime = self.FixFrameTime();
            if (nowTime <= self.LastCheckCDTime)
            {
                return;
            }
            var sub = nowTime - self.LastCheckCDTime;
            self.LastCheckCDTime = nowTime;
            if (self.CurrentCoolDownTime > 0)
            {
                self.CurrentCoolDownTime -= sub;
                if (self.CurrentCoolDownTime < 0)
                {
                    self.CurrentCoolDownTime = 0;
                }
            }
            
            if(self.HitInterval > 0)
            {
                self.HitInterval -= sub;
                if (self.HitInterval < 0)
                {
                    self.HitInterval = 0;
                }
            }
        }

        public static bool IsReady(this SkillAbility self)
        {
            return self.Spelling == false && self.IsInCoolDown() == false;
        }

        [EntitySystem]
        public static void Deserialize(this SkillAbility self)
        {
            self.Desc = ConfigSystem.Instance.Tables.TbSpell.Get(self.SkillId, self.Level);
        }
    }
}