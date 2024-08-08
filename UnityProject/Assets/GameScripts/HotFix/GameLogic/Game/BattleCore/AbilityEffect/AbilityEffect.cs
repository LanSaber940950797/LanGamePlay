using System;
using System.Collections.Generic;
using ET;
using GameConfig;
using GameConfig.Battle;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using Log = TEngine.Log;

namespace GameLogic.Battle
{

    public interface IEffectTriggerSystem
    {
        void OnTriggerApplyEffect(LSEntity effectAssign);
    }
    
    /// <summary>
    /// 能力效果，如伤害、治疗、施加状态等这些和技能数值、状态相关的效果
    /// </summary>
    ///
    
    [MemoryPackable]
    public partial class AbilityEffect : LSEntity, IDestroy, IAwake<EffectDesc, int>,ISerializeToEntity
    {
      
        public bool Enable;
        //public Entity OwnerAbility => Parent;
        
        [MemoryPackIgnore]
        [BsonIgnore]
        public Actor Owner => (this.Parent.Parent as IAbilityEntity).Owner;
        
        //public EffectSourceType EffectSourceType { get; set; }
        //public EffectTriggerEventBind TriggerEventBind { get; set; }
        //public bool IsNeedTarget { get; set; }
        
        [MemoryPackIgnore]
        [BsonIgnore]
        public Effect Desc
        {
            get
            {
                if (_desc == null)
                {
                    var effect = ConfigSystem.Instance.Tables.TbEffect.Get(EffectId, EffectLevel);
                    if (effect == null)
                    {
                        Log.Error($"EffectDesc {EffectId} {EffectLevel} not exist");
                        return null;
                    }
                    _desc = effect.Effects[EffectIdx];
                }
                return  _desc;
            }
            set
            {
                _desc = value;
            }
        }
        [MemoryPackIgnore]
        [BsonIgnore]
        public Effect _desc;
        public int EffectId;
        public int EffectLevel;
        public int EffectIdx;
    }
    
    [EntitySystemOf(typeof(AbilityEffect))]
    [LSEntitySystemOf(typeof(AbilityEffect))]
    public static partial class AbilityEffectSystem
    {
        private static Dictionary<EffectType, Type> EffectComponetList = null;
        [EntitySystem]
        public static void Awake(this AbilityEffect self, EffectDesc desc, int idx)
        {
            self.EffectId = desc.Id;
            self.EffectLevel = desc.Level;
            self.EffectIdx = idx;
            self.Desc = desc.Effects[idx - 1];
            self.ViewName = $"{desc.Name}_{idx}";
            self.AddEffectComponent(self.Desc.EffectType);
        }

        private static void AddEffectComponent(this AbilityEffect self, EffectType effectType)
        {
            switch (effectType)
            {
                case EffectType.Damage:
                    self.AddComponent<EffectDamage>();
                    break;
                case EffectType.Cure:
                    self.AddComponent<EffectCure>();
                    break;
                default:
                    Log.Error("AbilityEffect.Awake no effectType {0}", effectType);
                    break;
            }
        }

        public static EffectAssignAction CreateAssignAction(this AbilityEffect self, Entity targetEntity)
        {
            if (self.Owner.GetComponent<EffectAssignAbility>().TryMakeAction(out var action))
            {
                action.AssignTarget = targetEntity;
                action.SourceAbility = self.GetParent<LSEntity>();
                action.AbilityEffect = self;
            }
            return action;
        }
        
    }
}