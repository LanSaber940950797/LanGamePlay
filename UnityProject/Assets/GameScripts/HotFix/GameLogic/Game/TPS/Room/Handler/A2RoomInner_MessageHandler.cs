using ET;

namespace GameLogic
{
    [MessageHandler(SceneType.Client)]
    public class A2RoomInner_MessageHandler : MessageHandler<Scene, A2RoomInner_Message>
    {
        protected override async ETTask Run(Scene root, A2RoomInner_Message message)
        {
            var room = root.GetComponent<Room>();
            var roomSender = room.GetComponent<RoomSender>();
            if (roomSender == null)
            {
                return;
            }
            
            await roomSender.HandleMessage(message);
           
        }
    }
}