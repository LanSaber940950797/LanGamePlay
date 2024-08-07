using System.Collections.Generic;
using ET;

namespace GameLogic
{
    [ComponentOf(typeof(Room))]
    public class RoomSender : Entity, IAwake
    {
        public int RpcId;
        public readonly Dictionary<int, MessageSenderStruct> requestCallback = new();
        public Room Room => GetParent<Room>();

    }
}