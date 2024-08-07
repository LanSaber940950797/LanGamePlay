using ET;

namespace GameLogic
{
    [ComponentOf(typeof(Scene))]
    public class PlayerComponent : Entity, IAwake
    {
        public long PlayerId { get; set; }
    }
}