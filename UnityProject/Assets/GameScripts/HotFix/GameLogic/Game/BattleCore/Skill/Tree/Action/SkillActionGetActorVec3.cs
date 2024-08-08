using Kurisu.AkiBT;
using UnityEngine;

namespace GameLogic.Battle
{
    public enum GetVec3Type
    {
        Pos,
        Forward,
        Center,
    }
    
    [AkiInfo("Action:获取角色位置")]
    [AkiLabel("Skill:GetActorPos")]
    [AkiGroup("Skill")]
    public class SkillActionGetActorVec3 : SkillAction
    {
        [SerializeField, Tooltip("选择角色")]
        public SharedSTObject<Actor> target;
        [SerializeField, Tooltip("获取类型")]
        public GetVec3Type getVec3Type;
        
        public SharedVector3 getVec3;
        
        public override void Awake()
        {
            base.Awake();
            InitVariable(target);
            InitVariable(getVec3);
        }
        protected override Status OnUpdate()
        {
            var actor = target.Value;
            if (actor == null)
            {
                return Status.Failure;
            }

            var transform = actor.GetComponent<TransformComponent>();
            if (getVec3Type == GetVec3Type.Pos)
            {
                getVec3.Value = transform.Position.ToVector();
            }
            else if (getVec3Type == GetVec3Type.Forward)
            {
                getVec3.Value = transform.Forward.ToVector();
            }
            else if (getVec3Type == GetVec3Type.Center)
            {
                getVec3.Value = transform.CenterPosition.ToVector();
            }
            else
            {
                return Status.Failure;
            }
            
            return Status.Success;
        }
    }
}