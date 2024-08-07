using ET;
using TEngine;

namespace GameLogic
{
    [MessageHandler(SceneType.Room)]
    public class Room2C_TestChangeTxtNotifyHandler : MessageHandler<Room, Room2C_TestChangeTxtNotify>
    {
        protected override async ETTask Run(Room room, Room2C_TestChangeTxtNotify message)
        {
            if (room.PlayerNames.TryGetValue(message.Id, out var name))
            {
                GameEvent.Get<ILoginUI>().OnTestTxt($"{name}:{message.Txt}");
            }
        }
    }
}