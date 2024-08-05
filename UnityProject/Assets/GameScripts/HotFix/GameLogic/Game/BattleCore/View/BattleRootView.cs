using ET;
using UnityEngine;

namespace GameLogic.Battle
{
    [ComponentOf(typeof(Room))]
    public class BattleRootView : Entity, IAwake, IDestroy
    {
        public GameObject BattleRoot;
    }
}