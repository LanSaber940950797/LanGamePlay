using ET;

namespace GameLogic
{
    [MessageHandler(SceneType.Room)]
    public class C2Room_TestChangeTxtHandle : MessageHandler<Room, C2Room_TestChangeTxt>
    {
        protected override async ETTask Run(Room room, C2Room_TestChangeTxt message)
        {
           
            RoomHeler.SendTxt(message.PlayerId, message.Txt);
            await ETTask.CompletedTask;
        }
    }
}