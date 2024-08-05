using ET;

namespace GameLogic
{    
    [MessageHandler(SceneType.Room)]
    public class C2Room_PingHandler : MessageHandler<Room, C2Room_Ping, Room2C_Ping>
    {
        protected override async ETTask Run(Room unit, C2Room_Ping request, Room2C_Ping response)
        {
            using C2Room_Ping _ = request; // 这里用完调用Dispose可以回收到池，不调用的话GC会回收
			
            response.Time = TimeInfo.Instance.ClientNow();
            await ETTask.CompletedTask;
        }
    }
}