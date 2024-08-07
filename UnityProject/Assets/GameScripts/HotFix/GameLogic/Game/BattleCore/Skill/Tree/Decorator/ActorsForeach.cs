using Kurisu.AkiBT;
using UnityEngine;

namespace GameLogic.Battle
{
    [AkiInfo("Composite:遍历角色")]
    [AkiLabel("Skill:遍历角色")]
    [AkiGroup("Skill")]
    public class ActorsForeach : Decorator
    {
        [SerializeField, Tooltip("遍历目标数组")]
        public SharedSTObject<SharedActors> targets;
        
        [SerializeField, Tooltip("临时存储筛选的目标")]
        public SharedSTObject<Actor> tmpFilterActor;
        
        private int lastInde = 0;
        protected override void OnAwake() {
            base.OnAwake();
            lastInde = 0;
        }
        
        protected override Status OnUpdate()
        {
            var actors = targets.Value.Value;
            for (int i = lastInde; i < actors.Count; i++)
            {
                tmpFilterActor.Value = actors[i];
                var status = Child.Update();
                if (status == Status.Success) continue;
                if (status == Status.Running)
                {
                    lastInde = i;
                }
                return status;
            }
            return Status.Success;
        }
    }
}