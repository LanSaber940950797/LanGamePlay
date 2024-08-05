using NativeCollection;

namespace ET.Server;

[ComponentOf(typeof(Scene))]
public class Room : Entity, IAwake<long>
{
    public long RoomId { get; set; }
    
    public long MasterId;
    
    public List<long> PlayerIds = new();
}