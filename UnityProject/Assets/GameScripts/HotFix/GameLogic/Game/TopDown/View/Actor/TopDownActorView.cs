using ET;
using GameLogic.Battle;

namespace GameLogic
{
    [ComponentOf(typeof(ActorView))]
    public partial class TopDownActorView : LSEntity, IAwake
    {
        public Actor Actor => GetParent<ActorView>().Actor;
    }
    
    [EntitySystemOf(typeof(TopDownActorView))]
    public static partial class TopDownActorViewSystem
    {
        [EntitySystem]
        public static void Awake(this TopDownActorView self)
        {
           self.LodeMode().NoContext();
        }
        
        private static async ETTask LodeMode(this TopDownActorView self)
        {
            var viewComponent = self.GetComponent<ViewComponent>();
            await viewComponent.LoadMode("Warrior2d");
            var transformComponent = self.Actor.GetComponent<TransformComponent>();
            viewComponent.Go.transform.position = transformComponent.Position.ToVector();
            viewComponent.Go.transform.rotation = transformComponent.Rotation.ToQuaternion();
        }
    }
}