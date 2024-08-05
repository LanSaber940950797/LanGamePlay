using ET.Server;

namespace ET
{
    [Invoke((long)MailBoxType.RoomMessage)]
    [FriendOf(typeof(Room))]
    public class MailBoxType_RoomMessageHandler: AInvokeHandler<MailBoxInvoker>
    {
        public override void Handle(MailBoxInvoker args)
        {
            HandleAsync(args).NoContext();
        }
        
        private static async ETTask HandleAsync(MailBoxInvoker args)
        {
            MailBoxComponent mailBoxComponent = args.MailBoxComponent;
            
            MessageObject messageObject = args.MessageObject;
            
            if(messageObject is not IRoomMessage roomMsg)
            {
                Log.Error($"mailbox type is {mailBoxComponent.MailBoxType}, but message is {messageObject.GetType()}");
                return;
            }
            var room = mailBoxComponent.GetParent<Room>();
           
            var root = mailBoxComponent.Root();
            MessageLocationSenderComponent messageLocationSenderComponent = root.GetComponent<MessageLocationSenderComponent>();
            Log.Debug($"MailBoxType_RoomMessageHandler room id {room.MasterId} {messageObject}");
            if (room.MasterId != roomMsg.PlayerId) //客户端发送的消息是转发给房主服务器的
            {
                if (messageObject is not C2Room_JoinRoom)
                {
                    if (!room.PlayerIds.Contains(roomMsg.PlayerId))
                    {
                        Log.Error($"room {room.Id} player {roomMsg.PlayerId} is not in room");
                        return;
                    }
                }
                messageLocationSenderComponent.Get(LocationType.GateSession).Send(room.MasterId, messageObject);
              
            }
            else //房主发送的消息是转发给目标玩家服务器的
            {
                if (messageObject is Room2C_JoinRoom joinRoom && joinRoom.Error == 0)
                {
                    if (!room.PlayerIds.Contains(roomMsg.TargetPlayerId))
                    {
                        room.PlayerIds.Add(roomMsg.TargetPlayerId);
                    }
                }
                messageLocationSenderComponent.Get(LocationType.GateSession).Send(roomMsg.TargetPlayerId, messageObject);
            }
            
            await ETTask.CompletedTask;
        }
    }
}