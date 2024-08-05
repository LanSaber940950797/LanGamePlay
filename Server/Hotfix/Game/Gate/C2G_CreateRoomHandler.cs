namespace ET.Server
{
	[MessageSessionHandler(SceneType.Gate)]
	public class C2G_CreateRoomHandler : MessageSessionHandler<C2G_CreateRoom, G2C_CreateRoom>
	{
		protected override async ETTask Run(Session session, C2G_CreateRoom request, G2C_CreateRoom response)
		{
			Player player = session.GetComponent<SessionPlayerComponent>().Player;

			StartSceneConfig startSceneConfig = StartSceneConfigCategory.Instance.Match;

			G2Match_CreateRoom g2MatchCreateRoom = G2Match_CreateRoom.Create();
			g2MatchCreateRoom.Id = player.Id;
			
			
			var match2GCreateRoom = await session.Root().GetComponent<MessageSender>().Call(startSceneConfig.ActorId, g2MatchCreateRoom) as Match2G_CreateRoom;
			if (match2GCreateRoom.Error != 0)
			{
				response.Error = match2GCreateRoom.Error;
				response.Message = match2GCreateRoom.Message;
				return;
			}
			
			response.RoomId = match2GCreateRoom.RoomId;
			player.AddComponent<PlayerRoomComponent>().RoomActorId = match2GCreateRoom.ActorId;
		}
	}
}