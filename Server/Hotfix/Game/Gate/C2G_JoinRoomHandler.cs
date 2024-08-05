namespace ET.Server
{
	[MessageSessionHandler(SceneType.Gate)]
	public class C2G_JoinRoomHandler : MessageSessionHandler<C2G_JoinRoom, G2C_JoinRoom>
	{
		protected override async ETTask Run(Session session, C2G_JoinRoom request, G2C_JoinRoom response)
		{
			Player player = session.GetComponent<SessionPlayerComponent>().Player;

			StartSceneConfig startSceneConfig = StartSceneConfigCategory.Instance.Match;

			G2Match_JoinRoom g2MatchCreateRoom = G2Match_JoinRoom.Create();
			g2MatchCreateRoom.Id = player.Id;
			
			
			var match2GJoinRoom = await session.Root().GetComponent<MessageSender>().Call(startSceneConfig.ActorId, g2MatchCreateRoom) as Match2G_JoinRoom;
			if (match2GJoinRoom.Error != 0)
			{
				response.Error = match2GJoinRoom.Error;
				response.Message = match2GJoinRoom.Message;
				return;
			}
			
			response.RoomId = match2GJoinRoom.RoomId;
			player.AddComponent<PlayerRoomComponent>().RoomActorId = match2GJoinRoom.ActorId;
		}
	}
}