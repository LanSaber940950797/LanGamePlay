using UnityEngine;
using UnityEngine.UI;
using TEngine;

namespace GameLogic
{
	partial class RoomPage
	{
		#region 脚本工具生成的代码
		private Text m_textRoomId;
		private Text m_textPlayer;
		private GameObject m_goPlayerList;
		private Text m_textInput;
		private InputField m_inputTest;
		private Button m_btnTest;
		protected override void ScriptGenerator()
		{
			m_textRoomId = FindChildComponent<Text>("Panel/m_textRoomId");
			m_textPlayer = FindChildComponent<Text>("Panel/m_textPlayer");
			m_goPlayerList = FindChild("Panel/m_goPlayerList").gameObject;
			m_textInput = FindChildComponent<Text>("Panel/m_textInput");
			m_inputTest = FindChildComponent<InputField>("Panel/m_inputTest");
			m_btnTest = FindChildComponent<Button>("Panel/m_btnTest");
			m_btnTest.onClick.AddListener(OnClickTestBtn);
		}
		#endregion
	}
}