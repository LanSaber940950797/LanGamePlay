using ET;
using Rvo2;
using TrueSync;

namespace GameLogic.Battle
{
    [ComponentOf(typeof(Actor))]
    public class RvoSteerComponent : LSEntity, IAwake, ILSUpdate
    {
        public Actor Actor => GetParent<Actor>();
        public float Mass = 1;
        public int AgentId = -1;
        public Simulator Simulator;
        public Agent Agent => Simulator.GetAgentByAid(AgentId);
    }

    [EntitySystemOf(typeof(RvoSteerComponent))]
    public static partial class RvoSteerComponentSystem
    {
        [EntitySystem]
        public static void Awake(this RvoSteerComponent self)
        {
            self.SetSimulator();
        }
        
        private static void SetSimulator(this RvoSteerComponent self)
        {
            self.Simulator = self.LSWorld().GetComponent<RvoWorldComponent>().Simulator;

            var pos = self.Actor.GetComponent<TransformComponent>().Position;
         
            self.AgentId = self.Simulator.AddAgent(new Vector2(pos.x.AsFloat(), pos.y.AsFloat()));
            if (self.Actor.ActorType == ActorType.Player)
            {
                self.Agent.mass = 1000;
            }
            else
            {
                self.Agent.mass = 1;
                self.Agent.Radius = 0.5f;
            }
        }
        
        public static void Move(this RvoSteerComponent self, TSVector pos, FP maxSpeed)
        {
            var tmpPos = self.Actor.GetComponent<TransformComponent>().Position;
            var dir = pos - tmpPos;
            self.MoveByDir(dir, maxSpeed);
        }
        
        public static void MoveByDir(this RvoSteerComponent self, TSVector dir, FP maxSpeed)
        {
            var agent = self.Agent;
            var tmpPos = self.Actor.GetComponent<TransformComponent>().Position;
            if (dir.sqrMagnitude <= FP.Epsilon)
            {
                agent.prefVelocity.Set(0, 0);
            }
            else
            {
                if (dir.sqrMagnitude > 1f)
                {
                    dir = dir.normalized * maxSpeed;
                }
                
                agent.prefVelocity.Set(dir.x.AsFloat(), dir.y.AsFloat());
            }
        }

        [LSEntitySystem]
        public static void LSUpdate(this RvoSteerComponent self)
        {
            var agent = self.Agent;
            if (agent == null)
            {
                return;
            }

            if (agent.isStopped)
            {
                return;
            }
           
            if (self.Actor.ActorType != ActorType.Player)
            {
                var moveComponent = self.Actor.GetComponent<MoveComponent>();
                moveComponent.Velocity.Set(agent.velocity.x, agent.velocity.y, 0);
                var transform = self.Actor.GetComponent<TransformComponent>();
                transform.Position.Set(agent.position.x, agent.position.y, 0);
            }
        }
        
        public static void RefreshRVOAgentSpeed(this RvoSteerComponent self, FP maxSpeed)
        {
            self.Agent.maxSpeed_ = maxSpeed.AsFloat();
        }
        
        public static void ChangeAgentPosition(this RvoSteerComponent self, TSVector pos)
        {
            var agent = self.Agent;
            agent.position.x = pos.x.AsFloat();
            agent.position.y = pos.y.AsFloat();
        }
    }
}