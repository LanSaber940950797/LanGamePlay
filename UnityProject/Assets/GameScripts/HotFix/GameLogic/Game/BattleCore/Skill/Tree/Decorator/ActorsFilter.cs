using System.Collections.Generic;
using ET;
using Kurisu.AkiBT;
using UnityEngine;

namespace GameLogic.Battle
{

    
    [AkiInfo("Composite:目标筛选")]
    [AkiLabel("Skill:目标筛选")]
    [AkiGroup("Skill")]
    public class ActorsFilter : Decorator
    {
        [SerializeField, Tooltip("遍历目标数组")]
        public SharedSTObject<SharedActors> targets;
        
        [SerializeField, Tooltip("临时存储筛选的目标")]
        public SharedSTObject<Actor> tmpFilterActor;
        
      
        [SerializeField, Tooltip("筛选目标数量, 0不限制")]
        public int tatgetCount;
        protected override void OnAwake() {
            base.OnAwake();
        }
        
        protected override Status OnUpdate()
        {
            FilterActor();
            return Status.Success;
        }

        protected void FilterActor()
        {
            var actors = targets.Value.Value;
            List<EntityRef<Actor>> filterActors = new List<EntityRef<Actor>>();
            foreach (var actor in actors)
            {
                tmpFilterActor.Value = actor;
                bool result = false;
                
                var status = Child.Update();
                if (status == Status.Success)
                {
                    filterActors.Add(actor);
                    if(tatgetCount > 0 && filterActors.Count >= tatgetCount)
                    {
                        break;
                    }
                }
            }
            if(targets.Value == null)
                targets.Value = new SharedActors();
            targets.Value.Value = filterActors;
        }

        public override bool CanUpdate()
        {
            return true;
        }
    }
}