using ET;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using Log = TEngine.Log;

namespace GameLogic.Battle
{
    /// <summary>
    /// 治疗效果组件
    /// </summary>
    
    [MemoryPackable]
    public partial class EffectCure : LSEntity, IAwake, IEffectTriggerSystem, ISerializeToEntity
    {
        
        public void OnTriggerApplyEffect(LSEntity effectAssign)
        {
            EffectCureSystem.OnTriggerApplyEffect(this, effectAssign);
        }
    }
    
    [EntitySystemOf(typeof(EffectCure))]
    public  static partial class EffectCureSystem
    {


        public static void OnTriggerApplyEffect(EffectCure self, LSEntity effectAssign)
        { 
            Log.Debug($"EffectCureComponent OnTriggerApplyEffect");
            var effectAssignAction = effectAssign.As<EffectAssignAction>();
            if (self.GetParent<AbilityEffect>().Owner.GetComponent<CureActionAbility>().TryMakeAction(out var action))
            {
                action.SourceAssignAction = effectAssignAction;
                action.Target = effectAssignAction.Target;
                action.DoAction();
            }
        }
    }
}