using System.Collections.Generic;
using ET;
using MemoryPack;

namespace GameLogic.Battle
{
  
    public partial class SpellAction : LSEntity, IAwake, ILSUpdate, IServerActionExecution,IDestroy
    {
       
        public LSEntity ActionAbility { get; set; }
        //public EffectAssignAction SourceAssignAction { get; set; }
       
        public Actor Creator { get; set; }
       
        public Actor Target { get; set; }

        public bool IsSync { get; set; }
        public bool IsSnapshot { get; set; }

        
        public SkillAbility SkillAbility { get; set; }
        
        public SkillExecute SkillExecute { get; set; }
       
        public List<EntityRef<Actor>> SkillTargets { get; set; } = new List<EntityRef<Actor>>();
        public StateCastSkill Snapshot { get; set; }


        public SpellCastParam SpellCastParam;
        
    }
    
    [EntitySystemOf(typeof(SpellAction))]
    [LSEntitySystemOf(typeof(SpellAction))]
    public static partial class SpellActionSystem
    {

        [EntitySystem]
        public static void Destroy(this SpellAction self)
        {
            self.Creator = null;
            self.Target = null;
            self.SkillExecute = null;
            self.SkillAbility = null;
            self.ActionAbility = null;
            self.SkillTargets.Clear();
            self.Snapshot = null;
        }
        public static void FinishAction(this SpellAction self)
        {
            self.Dispose();
        }
        
        private static void PreProcess(this SpellAction self)
        {
            self.Creator.TriggerActionPoint(ActionPointType.PreSpell, self);
        }
        
        private static void SpellSkill(this SpellAction self)
        {
           
            self.SkillExecute = self.SkillAbility.CreateExecution();
            self.SkillExecute.ViewName = self.SkillAbility.ViewName;
            if (self.SkillTargets.Count > 0)
            {
                self.SkillExecute.SkillTargets.AddRange(self.SkillTargets);
            }
            self.SkillExecute.SpellCastParam = self.SpellCastParam;
            self.SkillExecute.BeginExecute();
        }
        
        [LSEntitySystem]
        public static void LSUpdate(this SpellAction self)
        {
            if (self.SkillExecute != null)
            {
                if (self.SkillExecute.IsDisposed)
                {
                    self.PostProcess();
                    self.FinishAction();
                }
            }
        }

        //后置处理
        private static void PostProcess(this SpellAction self)
        {
            self.Creator.TriggerActionPoint(ActionPointType.PostSpell, self);
        }

        public static void DoAction(this SpellAction self)
        {
            self.PreProcess();
            self.SpellSkill();
            self.SendFrameState();
        }

        private static void SendFrameState(this SpellAction self)
        {
            if (!self.IsSync)
            {
                return;
            }

            // StateFrame state = new StateFrame();
            // state.Frame = self.Frame();
            // state.Type = FrameStateType.StateCastSkill;
            // ref var data = ref state.Data.StateCreateActor;
            // data.ActorId = self.Creator.Id;
            // self.SendFrameState(state);
            // todo 待实现
        }

       
    }
}