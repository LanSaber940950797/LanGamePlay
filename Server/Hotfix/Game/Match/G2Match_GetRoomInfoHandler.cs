using System;


namespace ET.Server
{
	[MessageHandler(SceneType.Match)]
	public class G2Match_GetRoomInfoHandler : MessageHandler<Scene, G2Match_GetRoomInfo, Match2G_GetRoomInfo>
	{
		protected override async ETTask Run(Scene root, G2Match_GetRoomInfo request, Match2G_GetRoomInfo response)
		{
			RoomManager roomManager = root.GetComponent<RoomManager>();
			var roomRootActorId = roomManager.Get(request.RoomId);
			response.RoomId = request.Id;
			response.ActorId = roomRootActorId;
			await ETTask.CompletedTask;
		}
	}
}