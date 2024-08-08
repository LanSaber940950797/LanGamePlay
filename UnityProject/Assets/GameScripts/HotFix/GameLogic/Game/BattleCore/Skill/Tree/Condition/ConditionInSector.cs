using Kurisu.AkiBT;
using UnityEngine;

namespace GameLogic.Battle
{
    [AkiInfo("Conditional:在扇形范围内")]
    [AkiLabel("Skill:扇形范围内")]
    [AkiGroup("Skill")]
    public class ConditionInSector : SkillConditional
    {
        [AkiLabel("扇形角度")]
        [SerializeField]
        public SharedFloat sectorAngle;
        [AkiLabel("扇形半径")]
        [SerializeField]
        public SharedFloat sectorRadius;
        [AkiLabel("扇形中心点")]
        [SerializeField]
        public SharedVector3 sectorCenter;
        [AkiLabel("扇形方向")]
        [SerializeField]
        public SharedVector3 sectorDirection;
        
        [AkiLabel("目标")]
        [SerializeField]
        public SharedSTObject<Actor> target;

        protected override void OnStart()
        {
            base.OnStart();
            InitVariable(sectorAngle);
            InitVariable(sectorRadius);
            InitVariable(sectorCenter);
            InitVariable(sectorDirection);
            InitVariable(target);
            
        }

        protected override bool IsUpdatable()
        {
            Sector tmpSector = new Sector(sectorCenter.Value, sectorDirection.Value, sectorAngle.Value, sectorRadius.Value);
            var actor = target.Value;
            if (actor == null)
            {
                return false;
            }

            return actor.GetComponent<TransformComponent>().OverlapsWithSector(tmpSector);
        }
    }
}