using ET;
using Kurisu.AkiBT;

namespace GameLogic.Battle
{
    [AkiInfo("Action:朝方向移动")]
    [AkiLabel("BattleAI:朝方向移动")]
    [AkiGroup("BattleAI")]
    public class MoveDirAction : MoveAIAction
    {
        protected override Status OnUpdate()
        {
            var inputControlCompoent = actor.LSWorld().Parent.GetComponent<InputControlComponent>();
            var moveComponent = actor.GetComponent<MoveComponent>();
            moveComponent.MaxSpeed = moveComponent.Speed;
            actor.GetComponent<MoveComponent>().MoveDir(inputControlCompoent.MoveDir.ToTSVector());
            return Status.Success;
        }
    }
}