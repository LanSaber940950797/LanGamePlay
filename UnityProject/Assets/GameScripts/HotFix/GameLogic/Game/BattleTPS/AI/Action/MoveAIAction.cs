using Kurisu.AkiBT;
using TrueSync;
using UnityEngine;

namespace GameLogic.Battle
{
    public abstract class MoveAIAction : Action
    {
        protected Actor actor;
        public override void Awake()
        {
            var treeSO = (BattleAITreeSO)Tree;
            actor = treeSO.Actor;
        }

        protected override Status OnUpdate()
        {
            return Status.Success;
        }
        
        protected void MoveToTarget(TSVector targetPos, int speed = 1000)
        {
            var moveComponent = actor.GetComponent<MoveComponent>();
            moveComponent.MaxSpeed = moveComponent.Speed * speed / 1000;
            moveComponent.Move(targetPos);
        }
       
    }
}