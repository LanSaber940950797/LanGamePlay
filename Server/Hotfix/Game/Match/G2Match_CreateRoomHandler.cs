using System;


namespace ET.Server
{
	[MessageHandler(SceneType.Match)]
	public class G2Match_CreateRoomHandler : MessageHandler<Scene, G2Match_CreateRoom, Match2G_CreateRoom>
	{
		protected override async ETTask Run(Scene root, G2Match_CreateRoom request, Match2G_CreateRoom response)
		{
			Fiber fiber = root.Fiber();
			int fiberId = await FiberManager.Instance.Create(SchedulerType.ThreadPool, fiber.Zone, SceneType.RoomRoot, "RoomRoot");
			ActorId roomRootActorId = new(fiber.Process, fiberId);
			
			// 发送消息给房间纤程，初始化
			RoomManager2Room_Init roomManager2RoomInit = RoomManager2Room_Init.Create();
			var roomId = root.GetComponent<RoomManager>().GetRoomId();
			roomManager2RoomInit.PlayerId = request.Id;
			roomManager2RoomInit.RoomId = roomId;
			Room2RoomManager_Init room2RoomManagerInit = await root.GetComponent<MessageSender>().Call(roomRootActorId, roomManager2RoomInit) as Room2RoomManager_Init;
			response.RoomId = roomId;
			response.ActorId = roomRootActorId;
			await root.GetComponent<RoomManager>().Add(response.RoomId, roomRootActorId);
			await ETTask.CompletedTask;
		}
	}
}