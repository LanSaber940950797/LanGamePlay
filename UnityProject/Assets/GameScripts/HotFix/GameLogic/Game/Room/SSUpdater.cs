using ET;

namespace GameLogic
{
    //状态同步客户端更新器
    [ComponentOf(typeof(Room))]
    public class SSUpdater : Entity,IAwake,IUpdate
    {
        public long MyId { get; set; }

        public OneFrameState InputState { get; set; } = new();
    }
}