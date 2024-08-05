using ET.Server;

namespace ET.Server;

[MessageHandler(SceneType.RoomRoot)]
[FriendOf(typeof(Room))]
public class A2RoomInner_MessageHandler : MessageHandler<Scene, A2RoomInner_Message>
{
    protected override async ETTask Run(Scene root, A2RoomInner_Message message)
    {
        var room = root.GetComponent<Room>();
           
        Log.Debug($"room {room.Id} receive message {message.OpCode}");
        MessageLocationSenderComponent messageLocationSenderComponent = root.GetComponent<MessageLocationSenderComponent>();
        if (room.MasterId != message.PlayerId) //客户端发送的消息是转发给房主服务器的
        {
            if (message.OpCode != GameOuter.C2Room_JoinRoom)
            {
                if (!room.PlayerIds.Contains(message.PlayerId))
                {
                    Log.Error($"room {room.Id} player {message.PlayerId} is not in room");
                    return;
                }
            }
            messageLocationSenderComponent.Get(LocationType.GateSession).Send(room.MasterId, message);
              
        }
        else //房主发送的消息是转发给目标玩家服务器的
        {
            if (message.OpCode == GameOuter.Room2C_JoinRoom)
            {
                var joinRoom = MessageSerializeHelper.Deserialize(OpcodeType.Instance.GetType(message.OpCode), message.MessageObj, 0, message.MessageObj.Length) as Room2C_JoinRoom;
                if (joinRoom.Error == 0 && !room.PlayerIds.Contains(message.TargetPlayerId))
                {
                    room.PlayerIds.Add(message.TargetPlayerId);
                }
            }
            messageLocationSenderComponent.Get(LocationType.GateSession).Send(message.TargetPlayerId, message);
        }
            
        await ETTask.CompletedTask;
    }
}