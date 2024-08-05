using System;
using ET;

namespace GameLogic
{
    [EntitySystemOf(typeof(RoomPingComponent))]
    public static partial class RoomPingComponentSystem
    {
        [EntitySystem]
        private static void Awake(this RoomPingComponent self)
        {
            self.PingAsync().NoContext();
        }
        
        [EntitySystem]
        private static void Destroy(this RoomPingComponent self)
        {
            self.Ping = default;
        }
        
        private static async ETTask PingAsync(this RoomPingComponent self)
        {
            var room = self.GetParent<Room>();
            var roomSender = room.GetComponent<RoomSender>();
            long instanceId = self.InstanceId;
            Fiber fiber = self.Fiber();
            
            while (true)
            {
                try
                {
                    await fiber.Root.GetComponent<TimerComponent>().WaitAsync(2000);
                    if (self.InstanceId != instanceId)
                    {
                        return;
                    }
                    long time1 = TimeInfo.Instance.ClientNow();
                    // C2G_Ping不需要调用dispose，Call中会判断，如果用了对象池会自动回收
                    C2Room_Ping c2RoomPing = C2Room_Ping.Create(true);
                    // 这里response要用using才能回收到池，默认不回收
                    using Room2C_Ping response = await roomSender.CallRoomSvr(c2RoomPing) as Room2C_Ping;

                    if (self.InstanceId != instanceId)
                    {
                        return;
                    }

                    long time2 = TimeInfo.Instance.ClientNow();
                    self.Ping = time2 - time1;
                    
                    room.ServerMinusClientTime = response.Time + (time2 - time1) / 2 - time2;
                }
                catch (RpcException e)
                {
                    // session断开导致ping rpc报错，记录一下即可，不需要打成error
                    Log.Debug($"session disconnect, ping error: {self.Id} {e.Error}");
                    return;
                }
                catch (Exception e)
                {
                    Log.Debug($"ping error: \n{e}");
                }
            }
        }
    }
}