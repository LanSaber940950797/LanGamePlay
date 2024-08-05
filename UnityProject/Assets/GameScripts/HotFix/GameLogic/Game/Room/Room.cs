using System.Collections.Generic;
using ET;

namespace GameLogic
{
    
    
    
    [ComponentOf(typeof(Scene))]
    public class Room : Entity, IScene, IAwake, IUpdate
    {
        public Fiber Fiber { get; set; }
        public SceneType SceneType { get; set; }
        
        public string Name { get; set; }
        
        public long RoomId { get; set; }
        public long MasterId { get; set; }
        public long MyId { get; set; }
        public static bool IsMaster { get; set; }
        
        public List<long> PlayerIds = new List<long>();
        
        public Dictionary<long, string> PlayerNames = new Dictionary<long, string>();

        public long ServerMinusClientTime;
        // 帧缓存
        //public FrameBuffer FrameBuffer { get; set; }
        public long StartTime { get; set; }
        // 计算fixedTime，fixedTime在客户端是动态调整的，会做时间膨胀缩放
        public FixedTimeCounter FixedTimeCounter { get; set; }
        // 预测帧
        public int PredictionFrame { get; set; } = -1;

        // 权威帧
        public int AuthorityFrame { get; set; } = -1;
        
        // 状态帧缓存
        public StateFrameBuffer StateFrameBuffer { get; set; } 
        
        private EntityRef<LSWorld> lsWorld;

        // LSWorld做成child，可以有多个lsWorld，比如守望先锋有两个
        public LSWorld LSWorld
        {
            get
            {
                return this.lsWorld;
            }
            set
            {
                this.AddChild(value);
                this.lsWorld = value;
            }
        }
        
        public int SpeedMultiply { get; set; }
    }
}