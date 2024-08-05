using System.Collections.Generic;
using ET;
using MemoryPack;

namespace GameLogic.Battle
{
    /// <summary>
    /// 释放技能行动
    /// </summary>
    public class SpellActionAbility : LSEntity, IAwake, IDestroy, IActionAbility
    {
        public Actor Owner
        {
            get { return GetParent<Actor>();}
            set {}
        }
        public bool Enable { get; set; }


   
    }

    [EntitySystemOf(typeof(SpellActionAbility))]
    public static partial class SpellActionAbilitySystem
    {
        
        public static bool TryMakeAction(this SpellActionAbility self, out SpellAction action, bool isForce = false)
        {
            if (BattleActionHelper.TryMakeActionInner(self, out action, true, isForce))
            {
                action.Snapshot = new StateCastSkill();
                return true;
            }
            return false;
        }
        public static void OnFrameState(this SpellActionAbility self, StateCastSkill state)
        {
            if (BattleActionHelper.TryMakeActionInner(self, out SpellAction action, false))
            {
                action.Snapshot = state;
                action.IsSnapshot = true;
                action.DoAction();
            }
        }
    }
    
    

}