using ET;
using GameLogic.Battle;
using TEngine;

namespace GameLogic
{
    [MessageHandler(SceneType.Room)]
    public class C2Room_JoinRoomHandler :MessageHandler<Room, C2Room_JoinRoom, Room2C_JoinRoom>
    {
        protected override async ETTask Run(Room room, C2Room_JoinRoom request, Room2C_JoinRoom response)
        {
           
            room.Add(request.PlayerId, request.Name);
            foreach (var it in room.PlayerNames)
            {
                response.Players.Add((new RoomPlayerInfo()
                {
                    PlayerId = it.Key,
                    Name = it.Value
                }));
            }
            var world = room.LSWorld;
           
            response.TargetPlayerId = request.PlayerId;
            response.WolrdData = room.GetLSWorldData().ToArray();
            response.StartTime = room.StartTime;
            response.Frame = room.AuthorityFrame;
            //response.WolrdData = MongoHelper.Serialize(room.LSWorld);
            //GameEvent.Get<ILoginUI>().OnPlayerJonin();
            var actorComponent = world.GetComponent<ActorComponent>();
             ActorCreateInfo info = new ActorCreateInfo()
             {
                 ActorType = ActorType.Player,
                 SideType = SideType.SideA,
                 DescId = 1,
                 PlayerId = request.PlayerId,
                 Position = new TrueSync.TSVector(5, 0, 0),
                 Rotation = new TrueSync.TSQuaternion(0, 0, 0, 1),
             };
            
            actorComponent.CreateActor(info);
            await ETTask.CompletedTask;
        }
    }

}