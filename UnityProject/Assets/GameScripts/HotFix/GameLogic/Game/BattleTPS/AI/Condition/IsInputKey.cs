using ET;
using Kurisu.AkiBT;
using UnityEngine;

namespace GameLogic.Battle
{
    [AkiInfo("Conditional:是否按下按键")]
    [AkiLabel("BattleAI:按下按键")]
    [AkiGroup("BattleAI")]
    public class IsInputKey : Conditional
    {
        protected Actor actor;
        
        [AkiLabel("按键类型")]
        [SerializeField]
        public InputType inputType;

        protected override void OnAwake()
        {
            var tree = (BattleAITreeSO)Tree;
            actor = tree.Actor;
        }
        protected override bool IsUpdatable()
        {
            //摇杆是给本地玩家用的，所以直接获取输入组件
            var input = GameHeler.Room().GetComponent<InputControlComponent>();
            return input.LastInputType == inputType;
        }
    }
}