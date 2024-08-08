
using Kurisu.AkiBT;
using UnityEngine;

namespace GameLogic.Battle
{
    [AkiInfo("Action:获取目标")]
    [AkiLabel("Skill:获取目标")]
    [AkiGroup("Skill")]
    public class SkillActionGetTargets : Action
    {
        // [SerializeField, Tooltip("是否存活")]
        // public bool isAlive;
        [SerializeField, Tooltip("目标阵营")]
        public TargetSideType targetSide;
        
        [SerializeField, Tooltip("是否允许自己")]
        public bool isSelf;
        
        [SerializeField, Tooltip("选择角色")]
        public SharedSTObject<Actor> self;
        
        [SerializeField, Tooltip("作用目标")]
        public SharedSTObject<SharedActors> targets;

        public override void Awake()
        {
            base.Awake();
            InitVariable(self);
            InitVariable(targets);
        }

        protected override Status OnUpdate()
        {
            GetTargets();
            return Status.Success;
        }
        
        protected void GetTargets()
        {
            var actor = self.Value;
            if(actor == null)
                return;
            var list = ActorSelectHelper.GetActors(actor, targetSide, isSelf);
            if(targets.Value == null)
                targets.Value = new SharedActors();
            targets.Value.Value = list;
        }
    }

}