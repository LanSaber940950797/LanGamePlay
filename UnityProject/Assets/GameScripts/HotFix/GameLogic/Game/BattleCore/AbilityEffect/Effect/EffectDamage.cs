using ET;
using GameConfig.Battle;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;


namespace GameLogic.Battle
{
    /// <summary>
    /// 对目标造成伤害
    /// P1 伤害值 1 固定值 2 攻击力千分比
    /// P2 p1类型 1 固定值 2 攻击力千分比
    /// p3 1 物理伤害 2 魔法伤害 3 真实伤害
    /// </summary>
    ///
    
    [MemoryPackable]
    public partial class EffectDamage : LSEntity, IAwake, IEffectTriggerSystem, ISerializeToEntity
    {
        //public Effect effect;
        
        [MemoryPackIgnore]
        [BsonIgnore]
        public AbilityEffect Effect => GetParent<AbilityEffect>();

        public void OnTriggerApplyEffect(LSEntity effectAssign)
        {
            EffectDamageSystem.OnTriggerApplyEffect(this, effectAssign);
        }
    }
    
    [EntitySystemOf(typeof(EffectDamage))]
    public static partial  class EffectDamageSystem
    {

        public static void OnTriggerApplyEffect(EffectDamage self, LSEntity effectAssign)
        {
            var assignAction = effectAssign as EffectAssignAction;
            if(self.GetParent<AbilityEffect>().Owner.GetComponent<DamageActionAbility>().TryMakeAction(out var damageAction))
            {
                damageAction.sourceAssignAction = assignAction;
                damageAction.Target = assignAction.Target;
                damageAction.DamageValue = self.GetDamageValue(assignAction.Creator);
                damageAction.DamageType = (DamageType)self.Effect.Desc.Param3;
                damageAction.DoAction();
            }
        }
        
        private static long GetDamageValue(this EffectDamage self, Actor actor)
        {
            if (self.Effect.Desc.Param1 == 1)
            {
                return self.Effect.Desc.Param2;
            }
            else if(self.Effect.Desc.Param1 == 2)
            {
                return actor.GetComponent<AttributeComponent>().GetAttribute(AttributeType.Attack) * self.Effect.Desc.Param2 / 1000;
            }

            return 0;
        }
    }
}