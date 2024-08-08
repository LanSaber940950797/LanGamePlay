using ET;

namespace GameLogic.Battle
{
   
    
    public enum BattleWorldType
    {
        TwoDimensional,
        ThreeDimensional
    }

    public static class BattleConstValue
    {
        public static BattleWorldType WorldType = BattleWorldType.ThreeDimensional;
        public static bool IsStateSync = true;
    }
    
    public static class BattleHelper
    {
        public static int FixFrameTime(this LSEntity self)
        {
            LSWorld world = self.LSWorld();
            return  world.FixFrameTime();
        }

        public static int FixFrameTime(this LSWorld self)
        {
            var room = self.GetParent<Room>();
            return self.Frame * LSConstValue.UpdateInterval;
        }

        public static int Frame(this LSEntity self)
        {
            return self.LSWorld().Frame;
        }

        public static bool IsServer(this Entity self)
        {
            return Room.IsMaster;
        }
    }
}