namespace ET.Server;

[EntitySystemOf(typeof(RoomManager))]
[FriendOf(typeof(RoomManager))]
public static partial class RoomManagerSystem
{
    [EntitySystem]
    public static void Awake(this RoomManager self)
    {
       self.RoomMap.Clear();
    }

    public static async ETTask Add(this RoomManager self, long playerId, ActorId actorId)
    {
        if (self.RoomMap.TryGetValue(playerId, out var old))
        {
            await FiberManager.Instance.Remove(old.Fiber);
        }
        self.RoomMap.Add(playerId, actorId);
    }

    public static void Remove(this RoomManager self, long playerId)
    {
        self.RoomMap.Remove(playerId);
    }

    public static ActorId Get(this RoomManager self, long playerId)
    {
        return self.RoomMap[playerId];
    }

    public static long GetRoomId(this RoomManager self)
    {
        return ++self.MaxRoomId;
    }
   
}