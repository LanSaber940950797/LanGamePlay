
using Kurisu.AkiBT;
using UnityEngine;

namespace GameLogic.Battle
{
    public  class SkillTreeSO : BehaviorTreeSO
    {
        public virtual bool UseTree => true;
        [SerializeField, HideInInspector] private SkillTreeSO externalSkill;

        [Multiline, SerializeField, AkiLabel("技能描述")]
        protected string description;

        public string Description => description;
        public SkillTreeSO ExternalBehaviorTree => externalSkill;
       
       
        public IAbilityEntity Ability { get; set; }
        public IAbilityExecute Execute { get; set; }
        public Actor Owner => Ability.Owner;

        public bool IsRuning { get;  set; } = false;
        
        public void OnSkillExit()
        {
            if (!IsRuning)
            {
                return;
            }
            Execute?.EndExecute();
            IsRuning = false;
        }

        /// <summary>
        /// 处理技能进入=>需要初始化Init()传入绑定对象
        /// </summary>
        /// <param name="model"></param>
        public void Init(IAbilityEntity ability, IAbilityExecute execute)
        {
            Ability = ability;
            Execute = execute;
           
            this.IsInitialized = false;
            IsRuning = true;
#if UNITY_EDITOR
            this.Init(Owner.ViewGO);
#else
            this.Init(null);
#endif
            
        }

        public override Status Update()
        {
            if (IsRuning)
            {
                return base.Update();
            }
            return Status.Failure;
        }
    }
}

