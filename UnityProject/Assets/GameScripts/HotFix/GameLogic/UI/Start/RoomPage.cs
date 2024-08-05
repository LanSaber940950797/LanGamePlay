using TEngine;
using UnityEngine;

namespace GameLogic
{
    [Window(UILayer.UI, "RoomPage")]
    public partial class RoomPage : UIWindow
    {
        protected override void OnRefresh()
        {
            UpdateData();
        }

        protected override void RegisterEvent()
        {
            AddUIEvent(ILoginUI_Event.OnPlayerJonin, UpdateData);
            AddUIEvent<string>(ILoginUI_Event.OnTestTxt, OnTestTxt);
        }

        protected void UpdateData()
        {
            var root = GameHeler.Root();
            var room = root.GetComponent<Room>();
            m_textRoomId.text = $"房间：{room.Id}";
            //移除所有子节点
            for (int i = m_goPlayerList.transform.childCount - 1; i >= 0; i--)
            {
                GameObject.Destroy(m_goPlayerList.transform.GetChild(i).gameObject);
            }
            
            foreach (var player in room.PlayerNames)
            {
                var go = GameObject.Instantiate(m_textInput, m_goPlayerList.transform);
                go.text = $"玩家_{player.Value}";
            }
        }

        protected void OnClickTestBtn()
        {
            RoomHeler.Test(m_inputTest.text);
        }

        protected void OnTestTxt(string txt)
        {
            m_textInput.text = txt;
        }
    }
}