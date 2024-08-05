using System.Collections.Generic;

namespace ET.Server;

[ComponentOf(typeof(Scene))]
public class RoomManager : Entity,IAwake
{
    public Dictionary<long, ActorId> RoomMap = new Dictionary<long, ActorId>();
    public long MaxRoomId { get; set; }
}