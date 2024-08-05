namespace ET.Server;

[EntitySystemOf(typeof(Room))]
[FriendOf(typeof(Room))]
public static partial class RoomSystem
{
    [EntitySystem]
    public static void Awake(this Room self, long playerId)
    {
        self.MasterId = playerId;
        self.PlayerIds.Clear();
        self.PlayerIds.Add(playerId);
    }
}