using System.IO;
using ET;
using GameLogic.Battle;

namespace GameLogic
{
    [EntitySystemOf(typeof(Room))]
    public static partial class RoomSystem
    {
        public static Room Room(this Entity entity)
        {
            return entity.IScene as Room;
        }
        
        [EntitySystem]
        public static void Awake(this Room self, bool isNet)
        {
            if (isNet)
            {
                self.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.OrderedMessage);
                self.AddComponent<RoomSender>();
            }
           
            self.SceneType = SceneType.Room;
        }
        public static void Add(this Room self, long id, string name)
        {
            self.PlayerIds.Add(id);
            self.PlayerNames.Add(id, name);
        }

        public static long ServerNow(this Room self)
        {
            return TimeInfo.Instance.ClientNow() + self.ServerMinusClientTime;
        }
        
        public static MemoryBuffer GetLSWorldData(this Room self)
        { 
            MemoryBuffer memoryBuffer = new(10240);
            memoryBuffer.Seek(0, SeekOrigin.Begin);
            memoryBuffer.SetLength(0);
            
            MemoryPackHelper.Serialize(self.LSWorld, memoryBuffer);
            memoryBuffer.Seek(0, SeekOrigin.Begin);
            return memoryBuffer;
        }
        
        public static void Init(this Room self, long startTime, int frame = -1, LSWorld serverWorld = null)
        {
            self.StartTime = startTime;
            self.AuthorityFrame = frame;
            self.PredictionFrame = frame;
            self.StateFrameBuffer = new StateFrameBuffer(frame);
            self.FixedTimeCounter = new FixedTimeCounter(self.StartTime, 0, LSConstValue.UpdateInterval);
            if (serverWorld == null)
            {
                self.LSWorld = new LSWorld(SceneType.Battle);
                LSWorld lsWorld = self.LSWorld;
                lsWorld.Frame = frame + 1;
                lsWorld.AddComponent<ActorComponent>();
            }
            else
            {
                self.LSWorld = serverWorld;
            }
            
            //创建前端管理组件
            self.AddComponent<ActorViewComponent>();
            self.AddComponent<BattleRootView>();
        }

        
    }

}