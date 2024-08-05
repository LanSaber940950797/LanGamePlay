using UnityEngine;
using UnityEngine.UI;
using TEngine;

namespace GameLogic
{
	partial class GameStartPage
	{
		#region 脚本工具生成的代码
		private Button m_btnCreate;
		private Button m_btnJoin;
		private InputField m_inputRoomId;
		private InputField m_inputName;
		protected override void ScriptGenerator()
		{
			m_btnCreate = FindChildComponent<Button>("Panel/m_btnCreate");
			m_btnJoin = FindChildComponent<Button>("Panel/m_btnJoin");
			m_inputRoomId = FindChildComponent<InputField>("m_inputRoomId");
			m_inputName = FindChildComponent<InputField>("m_inputName");
			m_btnCreate.onClick.AddListener(OnClickCreateBtn);
			m_btnJoin.onClick.AddListener(OnClickJoinBtn);
		}
		#endregion
	}
}