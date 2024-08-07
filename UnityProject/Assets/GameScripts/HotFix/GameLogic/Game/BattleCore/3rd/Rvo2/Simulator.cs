using System.Collections.Generic;

namespace Rvo2
{
    public class Simulator
    {
        private int agentId = 0;
        private List<int> agentIdLst = new List<int>();
        private bool _pause = false;
        private Dictionary<int, Agent> aid2agent = new Dictionary<int, Agent>();

        public List<Obstacle> obstacles = new List<Obstacle>();
        public KdTree kdTree ;


        public Agent defaultAgent = null;
        public float time = 0.0f;
        
        public Simulator()
        {
            kdTree = new KdTree(this);
        }
        
        public bool IsPause()
        {
            return _pause;
        }

        public Agent GetAgentByIndex(int idx)
        {
            return aid2agent[agentIdLst[idx]];
        }

        public Agent GetAgentByAid(int aid)
        {
            return aid2agent[aid];
        }

        public float GetGlobalTime()
        {
            return time;
        }

        public int GetNumAgents()
        {
            return agentIdLst.Count;
        }

        public Agent GetAgentAidByIdx(int idx)
        {
            return aid2agent[agentIdLst[idx]];
        }

        public void SetAgentPrefVelocity(int aid, Vector2 velocity)
        {
            aid2agent[aid].prefVelocity.Copy(velocity);
        }

        public Vector2 GetAgentPosition(int aid)
        {
            if (aid2agent.ContainsKey(aid))
            {
                return aid2agent[aid].position;
            }
            return null;
        }

        public Vector2 GetAgentPrefVelocity(int aid)
        {
            return aid2agent[aid].prefVelocity;
        }

        public Vector2 GetAgentVelocity(int aid)
        {
            return aid2agent[aid].velocity;
        }

        public float GetAgentRadius(int aid)
        {
            return aid2agent[aid].Radius;
        }

        public List<Line> GetAgentOrcaLines(int aid)
        {
            return aid2agent[aid].orcaLines;
        }

        public int AddAgent(Vector2 position, float? radius = null, float? maxSpeed = null, Vector2? velocity = null, float? mass = null)
        {
            if (defaultAgent == null)
            {
                throw new System.Exception("no default agent");
            }

            Agent agent = new Agent();

            agent.position.Copy(position);
            agent.maxNeighbors_ = defaultAgent.maxNeighbors_;
            agent.maxSpeed_ = maxSpeed ?? defaultAgent.maxSpeed_;
            agent.neighborDist = defaultAgent.neighborDist;
            agent.Radius = radius ?? defaultAgent.Radius;
            agent.timeHorizon = defaultAgent.timeHorizon;
            agent.timeHorizonObst = defaultAgent.timeHorizonObst;
            agent.velocity.Copy(velocity ?? defaultAgent.velocity);

            agent.id = agentId++;

            if (mass.HasValue && mass.Value >= 0)
            {
                agent.mass = mass.Value;
            }
            aid2agent[agent.id] = agent;
            agentIdLst.Add(agent.id);

            return agent.id;
        }

        public void RemoveAgent(int aid)
        {
            if (aid2agent.ContainsKey(aid))
            {
                agentIdLst.Remove(aid);
                aid2agent.Remove(aid);
            }
        }

        public bool HasAgent(int aid)
        {
            return aid2agent.ContainsKey(aid);
        }

        public void SetAgentMass(int agentNo, float mass)
        {
            aid2agent[agentNo].mass = mass;
        }

        public float GetAgentMass(int agentNo)
        {
            return aid2agent[agentNo].mass;
        }

        public void SetAgentRadius(int agentNo, float radius)
        {
            aid2agent[agentNo].Radius = radius;
        }
        /**
    *
    * @param neighborDist 在寻找周围邻居的搜索距离，这个值设置过大，会让小球在很远距离时做出避障行为
    * @param maxNeighbors 寻找周围邻居的最大数目，这个值设置越大，最终计算的速度越精确，但会增大计算量
    * @param timeHorizon 代表计算动态的物体时的时间窗口
    * @param timeHorizonObst 代表计算静态的物体时的时间窗口，比如在RTS游戏中，小兵向城墙移动时，没必要做出避障，这个值需要 设置得很小
    * @param radius 代表计算ORCA时的小球的半径，这个值不一定与小球实际显示的半径一样，偏小有利于小球移动顺畅
    * @param maxSpeed 小球最大速度值
    * @param velocity 小球初始速度
    */
        public void SetAgentDefaults(float neighborDist, int maxNeighbors, float timeHorizon, float timeHorizonObst, float radius, float maxSpeed, Vector2 velocity)
        {
            if (defaultAgent == null)
            {
                defaultAgent = new Agent();
            }

            defaultAgent.maxNeighbors_ = maxNeighbors;
            defaultAgent.maxSpeed_ = maxSpeed;
            defaultAgent.neighborDist = neighborDist;
            defaultAgent.Radius = radius;
            defaultAgent.timeHorizon = timeHorizon;
            defaultAgent.timeHorizonObst = timeHorizonObst;
            defaultAgent.velocity.Copy(velocity);
        }

        public void Pause()
        {
            _pause = true;
        }

        public void Resume()
        {
            _pause = false;
        }

        public void Run(float dt)
        {
            if (_pause)
            {
                return;
            }

            kdTree.BuildAgentTree(GetNumAgents());
            int agentNum = agentIdLst.Count;
            for (int i = 0; i < agentNum; i++)
            {
                Agent agent = aid2agent[agentIdLst[i]];

                if (agent.isStopped)
                {
                    continue;
                }
                agent.ComputeNeighbors(this);
                agent.ComputeNewVelocity(dt);
            }
            for (int i = 0; i < agentNum; i++)
            {
                Agent agent = aid2agent[agentIdLst[i]];

                if (agent.isStopped)
                {
                    continue;
                }
                agent.Update(dt);
            }

            time += dt;
        }

        public int AddObstacle(List<Vector2> vertices)
        {
            if (vertices.Count < 2)
            {
                return -1;
            }

            int obstacleNo = obstacles.Count;

            for (int i = 0; i < vertices.Count; ++i)
            {
                Obstacle obstacle = new Obstacle();
                obstacle.Point = vertices[i];
                if (i != 0)
                {
                    obstacle.Previous = obstacles[obstacles.Count - 1];
                    obstacle.Previous.Next = obstacle;
                }
                if (i == vertices.Count - 1)
                {
                    obstacle.Next = obstacles[obstacleNo];
                    obstacle.Next.Previous = obstacle;
                }
                obstacle.Direction = RVOMath.Normalize(vertices[(i == vertices.Count - 1 ? 0 : i + 1)].Minus(vertices[i]));

                if (vertices.Count == 2)
                {
                    obstacle.Convex = true;
                }
                else
                {
                    obstacle.Convex = (RVOMath.LeftOf(vertices[(i == 0 ? vertices.Count - 1 : i - 1)], vertices[i], vertices[(i == vertices.Count - 1 ? 0 : i + 1)]) >= 0);
                }

                obstacle.Id = obstacles.Count;

                obstacles.Add(obstacle);
            }

            return obstacleNo;
        }

        public void ProcessObstacles()
        {
            kdTree.BuildObstacleTree();
        }

        public bool QueryVisibility(Vector2 point1, Vector2 point2, float radius)
        {
            return kdTree.QueryVisibility(point1, point2, radius);
        }

        public List<Obstacle> GetObstacles()
        {
            return obstacles;
        }

        public void Clear()
        {
            agentIdLst.Clear();
            agentId = 0;
            aid2agent.Clear();
            defaultAgent = null;
            kdTree = new KdTree(this);
            obstacles.Clear();
        }
    }
}