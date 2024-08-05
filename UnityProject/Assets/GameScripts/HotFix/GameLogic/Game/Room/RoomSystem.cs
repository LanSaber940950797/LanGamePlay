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
        public static void Awake(this Room self)
        {
            self.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.OrderedMessage);
            self.AddComponent<RoomSender>();
            self.SceneType = SceneType.Room;
        }
        public static void Add(this Room self, long id, string name)
        {
            self.PlayerIds.Add(id);
            self.PlayerNames.Add(id, name);
        }

        public static void Init(this Room self, long startTime, int frame = -1, LSWorld serverWorld = null)
        {
            self.StartTime = startTime;
            self.AuthorityFrame = frame;
            self.PredictionFrame = frame;
            self.StateFrameBuffer = new StateFrameBuffer(frame);
            self.FixedTimeCounter = new FixedTimeCounter(self.StartTime, 0, LSConstValue.UpdateInterval);
            self.AddComponent<ActorViewComponent>();
            self.AddComponent<BattleRootView>();
            if (serverWorld == null)
            {
                self.LSWorld = new LSWorld(SceneType.LockStepClient);
                LSWorld lsWorld = self.LSWorld;
                lsWorld.Frame = frame + 1;
                lsWorld.AddComponent<FrameStateComponent>();
                lsWorld.AddComponent<ActorComponent>();
            }
            else
            {
                //self.AddChild(serverWorld);
                self.LSWorld = serverWorld;
            }
            self.AddComponent<InputControlComponent>();
            self.AddComponent<SSUpdater>();
        }

        public static void Load(this Room self, LSWorld world)
        {
            self.LSWorld.GetComponent<ActorComponent>().Load(world.GetComponent<ActorComponent>());
        }

        public static void Update(this Room self, OneFrameStates oneFrameStates, int frame, long noUpdateId = -1)
        {
            LSWorld lsWorld = self.LSWorld;
            FrameStateComponent frameStateComponent = lsWorld.GetComponent<FrameStateComponent>();
            frameStateComponent.Update(oneFrameStates, frame, noUpdateId);
            lsWorld.Frame = frame;
            var inputControlComponent = self.GetComponent<InputControlComponent>();
            lsWorld.Update();
            inputControlComponent.LSLateUpdate();
            frameStateComponent.LateUpdate(oneFrameStates, self.MyId);
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
        

        
    }

}