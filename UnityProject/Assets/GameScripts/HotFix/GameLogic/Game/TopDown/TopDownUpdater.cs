using ET;

namespace GameLogic
{
    [ComponentOf(typeof(Room))]
    public class TopDownUpdater : Entity,IUpdate, IAwake
    {
        
    }
    
    [EntitySystemOf(typeof(TopDownUpdater))]
    public static partial  class TopDownUpdaterSystem
    {
        [EntitySystem]
        public static void Update(this TopDownUpdater self)
        {
            long timeNow = TimeInfo.Instance.ServerFrameTime();
            Room room = self.GetParent<Room>();
            int frame = room.AuthorityFrame + 1;
            if (timeNow < room.FixedTimeCounter.FrameTime(frame))
            {
                return;
            }
            
            ++room.AuthorityFrame;
            room.TopDownUpdate();
        }
    }
}