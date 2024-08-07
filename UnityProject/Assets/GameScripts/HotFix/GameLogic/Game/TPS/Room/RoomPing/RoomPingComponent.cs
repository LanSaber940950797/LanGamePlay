using ET;

namespace GameLogic
{
    [ComponentOf(typeof(Room))]
    public class RoomPingComponent : Entity, IAwake, IDestroy
    {
        public long Ping { get; set; } //延迟值
    }
}