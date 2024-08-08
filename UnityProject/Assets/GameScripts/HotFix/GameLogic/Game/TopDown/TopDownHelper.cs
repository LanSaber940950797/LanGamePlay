using Cysharp.Threading.Tasks;
using ET;
using GameLogic.Battle;
using TEngine;
using TrueSync;

namespace GameLogic
{
    public static class TopDownHelper
    {
        public static async UniTask StartGame()
        {
            await GameModule.Scene.LoadScene("TopDownBattle");
            var root = GameHeler.Root();
            root.RemoveComponent<Room>();
            Room room = root.AddComponent<Room, bool>(false);
            room.TopDownInit(TimeInfo.Instance.ClientNow());
            room.AddComponent<TopDownUpdater>();
            
            var actorComponent = room.LSWorld.GetComponent<ActorComponent>();
            actorComponent.CreateActor(new ActorCreateInfo()
            {
                ActorType = ActorType.Player,
                SideType = SideType.SideA,
                DescId = 1,
                Position = new(5, 0, 0),
                Rotation = TSQuaternion.identity,
            });
            actorComponent.CreateActor((new ActorCreateInfo()
            {
                ActorType = ActorType.Monster,
                SideType = SideType.SideB,
                DescId = 1,
                Position = new(0, 0, 0),
                Rotation = TSQuaternion.identity,
            }));
        }
    }
}