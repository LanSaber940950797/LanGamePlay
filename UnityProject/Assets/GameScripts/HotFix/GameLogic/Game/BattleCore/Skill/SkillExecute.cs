using System.Collections.Generic;
using ET;
using GameConfig.Battle;


namespace GameLogic.Battle
{
    public enum SkillExecutionState
    {
        None,
        Prepare, //施法准备
        Attack, //攻击
        Hit, //命中
        End, //结束
    }
    /// <summary>
    /// 技能执行体，执行体就是控制角色表现和技能表现的，包括角色动作、移动、变身等表现的，以及技能生成碰撞体等表现
    /// </summary>
    public  partial class SkillExecute : LSEntity, IAwake<SkillAbility>, ILSUpdate, IAbilityExecute
    {
        public Actor Target ;
        public Entity abilityEntity { get; set; }
        public LSEntity AbilityEntity { get; set; }
        public Actor Owner { get; set; }
        public IAbilityEntity Ability { get => abilityEntity as SkillAbility; set{} }

        //执行体所属的技能
        public SkillAbility SkillAbility => abilityEntity as SkillAbility;
        
        //目标列表
        public List<EntityRef<Actor>> SkillTargets = new List<EntityRef<Actor>>();
        
        public SkillExecutionState State = SkillExecutionState.None;

        public long OriginTime;
        public bool ActionOccupy = true;
        public SpellCastParam SpellCastParam;
        //public BattleTimer Timer;
       
        public void BeginExecute()
        {
            SkillExecuteSystem.BeginExecute(this);
        }

        public void EndExecute()
        {
            SkillExecuteSystem.EndExecute(this);
        }
      
    }
    
    [EntitySystemOf(typeof(SkillExecute))]
    public static partial class SkillExecuteSystem
    {
        [EntitySystem]
        public static void Awake(this SkillExecute self, SkillAbility abilityEntity)
        {
            
            self.AbilityEntity = abilityEntity;
            self.Owner = self.GetParent<Actor>();
            self.OriginTime = self.FixFrameTime();
            self.State = SkillExecutionState.None;
        }

        

        public static void BeginExecute(SkillExecute self)
        {
            if (self.SkillAbility != null)
            {
                self.SkillAbility.Spelling = true;
            }
            self.SkillAbility.GetComponent<SkillTreeComponent>().Enable = true;
        }

        public static void EndExecute(SkillExecute self)
        {
            self.SkillAbility.Spelling = false;
            self.SkillAbility.GetComponent<SkillTreeComponent>().Enable = false;
            self.SkillTargets.Clear();
            self.Dispose();
        }
    }
   
}