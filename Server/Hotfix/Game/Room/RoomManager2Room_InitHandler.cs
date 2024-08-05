using System.Collections.Generic;

namespace ET.Server
{
    [MessageHandler(SceneType.RoomRoot)]
    [FriendOf(typeof(Room))]
    public class RoomManager2Room_InitHandler: MessageHandler<Scene, RoomManager2Room_Init, Room2RoomManager_Init>
    {
        protected override async ETTask Run(Scene root, RoomManager2Room_Init request, Room2RoomManager_Init response)
        {
            Room room = root.AddComponent<Room, long>(request.PlayerId);
            room.RoomId = request.RoomId;
            //room.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.RoomMessage);
            response.ActorId = root.GetActorId();
            Log.Error($"RoomManager2Room_InitHandler: {request.PlayerId}, room master: {room.MasterId}");
            await ETTask.CompletedTask;
        }
    }
}