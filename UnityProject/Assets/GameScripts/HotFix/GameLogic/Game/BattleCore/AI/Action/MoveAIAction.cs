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
        

    }
}