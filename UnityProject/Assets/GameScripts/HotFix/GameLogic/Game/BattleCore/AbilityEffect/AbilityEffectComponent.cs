using System.Collections.Generic;
using ET;
using GameConfig;
using GameConfig.Battle;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;


namespace GameLogic.Battle
{
    [MemoryPackable]
    public partial class AbilityEffectComponent : LSEntity,IAwake<EffectDesc>,ISerializeToEntity,IDeserialize
    {
        [MemoryPackIgnore] 
        [BsonIgnore] 
        public EffectDesc Desc;

       
        
        public int EffectId;
        public int EffectLevel;
        public bool Enable;

    }
    
    [EntitySystemOf(typeof(AbilityEffectComponent))]
    public static  partial class AbilityEffectComponentSystem
    {
        [EntitySystem]
        public static void Awake(this AbilityEffectComponent self, EffectDesc desc)
        {
            self.Desc = desc;
            self.EffectId = desc.Id;
            self.EffectLevel = desc.Level;
            for (int i = 0; i < desc.Effects.Count; i++)
            {
                self.AddChild<AbilityEffect, EffectDesc, int>(desc, i);
            }
        }

        public static AbilityEffect GetAbilityEffect(this AbilityEffectComponent self, int index)
        {
            foreach (AbilityEffect it in self.Children.Values)
            {
                if (it.EffectIdx == index)
                {
                    return it;
                }
            }
            return null;
        }
        
        public static EffectAssignAction CreateAssignAction(this AbilityEffectComponent self, Actor target, int index)
        {
            var abilityEffect = self.GetAbilityEffect(index);
            if (abilityEffect == null)
            {
                Log.Error($"abilityEffect is null,index:{index}");
                return null;
            }
            var effectAssign = abilityEffect.CreateAssignAction(target);
            effectAssign.Target = target;
            return effectAssign;
        }

        [EntitySystem]
        public static void Deserialize(this AbilityEffectComponent self)
        {
            self.Desc = ConfigSystem.Instance.Tables.TbEffect.Get(self.EffectId, self.EffectLevel);
        }
    }
}