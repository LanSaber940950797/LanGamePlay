using ET;

namespace GameLogic
{
    public static class GameHeler
    {
        public static Scene Root()
        {
            return GameEntrySystem.Instance.Root;
        }

        public static Room Room()
        {
            return Root().GetComponent<Room>();
        }
    }
}