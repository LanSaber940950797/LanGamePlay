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
            Room room = self.GetParent<Room>();
            room.TopDownUpdate();
        }
    }
}