using System.IO;
using System.Linq;
using ET;

namespace GameLogic.Battle
{
    [EntitySystemOf(typeof(ActorComponent))]
    public  static partial class ActorComponentSystem
    {

        [EntitySystem]
        public static void Awake(this ActorComponent self)
        {
            //系统管理员创建，有些操作需要类似actor一样的操作行为
            self.CreateSystemActor();
        }

        [EntitySystem]
        public static void Destroy(this ActorComponent self)
        {
           
        }
        public static Actor GetPlayerActor(this ActorComponent self, long playerId)
        {
            if (self.PlayerActors.TryGetValue(playerId, out var actor))
            {
                return actor;
            }
            return null;
        }
        private static void CreateSystemActor(this ActorComponent self)
        {
            ActorCreateInfo info = new ActorCreateInfo()
            {
                ActorType = ActorType.System,
                DescId = 1,
                PlayerId = 0,
                SideType = SideType.Neutral
            };
            self.CreateActorInner(info);
        }
        
        // 这个接口是系统创建的actor，也就是普通actor，没有召唤者
        public static Actor CreateActor(this ActorComponent self, ActorCreateInfo info)
        {
            if (self.SystemActor == null)
            {
                Log.Error($"{self.Parent.Id} CreateSystemActor SystemActor is null");
            }
            return self.SystemActor.CreateActor(info);
        }

        public static Actor CreateActor(this Actor self, ActorCreateInfo info)
        {
            if (self.GetComponent<CreateActorAbility>().TryMakeAction(out var action))
            {
                action.CreateInfo = info;
                action.DoAction();
                return action.Creator;
            }
            return null;
        }
        public static void Load(this ActorComponent self, ActorComponent other)
        {
            var ids = other.Children.Keys.ToList();
            foreach (long id in ids)
            {
                Actor actor = other.GetChild<Actor>(id);
                self.LoadActor(actor);
            }
        }

        private static Actor CreateActorInner(this ActorComponent self, ActorCreateInfo info)
        {
            var actor = self.AddChild<Actor, ActorCreateInfo>(info);
            var transform = actor.Transform;
            transform.Position = info.Position;
            transform.Rotation = info.Rotation;
            self.OnActorCreate(actor, info);
            return actor;
        }

        private static void AddPlayerActor(this ActorComponent self, Actor actor)
        {
            self.PlayerActors.Add(actor.PlayerId, actor);
        }
        public static void CreateActor(this ActorComponent self, CreateActorAction action)
        {
            if (action.IsSnapshot)
            {
                self.LoadActor(action.Target);
            }
            else
            {
                action.Target = self.CreateActorInner(action.CreateInfo);
            }
        }
        private static void LoadActor(this ActorComponent self, Actor actor)
        {
            self.AddChild(actor);
            self.OnActorCreate(actor);

        }

        private static void OnActorCreate(this ActorComponent self, Actor actor, ActorCreateInfo info = null)
        {
            if (actor.ActorType == ActorType.Player)
            {
                self.AddPlayerActor(actor);
            }
            else if(actor.ActorType == ActorType.System)
            {
                self.SystemActor = actor;
            }
            if (actor.ActorType != ActorType.System)
            {
                if (info == null)
                {
                    info = new ActorCreateInfo()
                    {
                        ActorType = actor.ActorType,
                        DescId = actor.DescId,
                        PlayerId = actor.PlayerId,
                        Position = actor.Transform.Position,
                        Rotation = actor.Transform.Rotation,
                        SideType = actor.SideType
                    };
                }
                // 发送这是发送给前端展示的，所以接受的前端显示组件得先创建出来，但同步world的时候触发，world的组件还没创建出来
                //所以前端显示组件得先world创建，所以这里要发送给room
                GameHeler.Room().SendEvent(BattleEvent.ActorCreateView, actor, info);
            }
        }
        
        [EntitySystem]
        public static void Deserialize(this ActorComponent self)
        {
            foreach (Actor actor in self.Children.Values)
            {
                self.OnActorCreate(actor);
            }
        }
    }
}