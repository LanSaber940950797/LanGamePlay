using TEngine;

namespace GameLogic
{
    [Window(UILayer.UI, "GameStartPage")]
    public partial class GameStartPage : UIWindow
    {
        private void OnClickCreateBtn()
        {
            var name = m_inputName.text;
            RoomHeler.CreateRoom(name).NoContext();
        }

        private void OnClickJoinBtn()
        {
            var roomId = this.m_inputRoomId.text;
            var name = m_inputName.text;
            RoomHeler.JoinRoom(long.Parse(roomId), name).NoContext();
        }
    }
}