using ET;
using Kurisu.AkiBT;

namespace GameLogic.Battle
{
    [AkiInfo("Conditional:是否摇杆")]
    [AkiLabel("BattleAI:摇杆")]
    [AkiGroup("BattleAI")]
    public class IsJoystick : Conditional
    {
        protected Actor actor;

        protected override void OnAwake()
        {
            var tree = (BattleAITreeSO)Tree;
            actor = tree.Actor;
        }
        protected override bool IsUpdatable()
        {
            //摇杆是给本地玩家用的，所以直接获取输入组件
            var input = actor.LSWorld().Parent.GetComponent<InputControlComponent>();
            return input.IsJoystickTouch;
        }
    }
}