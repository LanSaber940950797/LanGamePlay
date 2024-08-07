using System;
using System.Collections.Generic;

namespace Rvo2
{
    public class FloatPair
    {
        public float a;
        public float b;

        public FloatPair(float a, float b)
        {
            this.a = a;
            this.b = b;
        }

        public bool LessThan(FloatPair rhs)
        {
            return a < rhs.a || !(rhs.a < a) && b < rhs.b;
        }

        public bool LessEqualThan(FloatPair rhs)
        {
            return (a == rhs.a && b == rhs.b) || LessThan(rhs);
        }

        public bool BigThan(FloatPair rhs)
        {
            return !LessEqualThan(rhs);
        }

        public bool BigEqualThan(FloatPair rhs)
        {
            return !LessThan(rhs);
        }
    }

    public class AgentTreeNode
    {
        public int Begin;
        public int End;
        public int Left;
        public int Right;
        public float MaxX;
        public float MaxY;
        public float MinX;
        public float MinY;
    }

    public class ObstacleTreeNode
    {
        public Obstacle Obstacle;
        public ObstacleTreeNode Left;
        public ObstacleTreeNode Right;
    }

    
    public class KdTree
    {
        private const int MAX_LEAF_SIZE = 10;
        private List<Agent> agents = new List<Agent>();
        private List<AgentTreeNode> agentTree = new List<AgentTreeNode>();
        private ObstacleTreeNode obstacleTree;
        private Simulator _simulator;

        public KdTree(Simulator simulator)
        {
            _simulator = simulator;
        }

        public void BuildAgentTree(int agentNum)
        {
            
            if (agents.Count != agentNum)
            {
                agents.Clear();
                for (int i = 0; i < agentNum; i++)
                {
                    agents.Add(_simulator.GetAgentByIndex(i));
                }
                agentTree.Clear();
                int agentTreeSize = 2 * agentNum;
                for (int i = 0; i < agentTreeSize; i++)
                {
                    agentTree.Add(new AgentTreeNode());
                }
            }

            if (agents.Count != 0)
            {
                BuildAgentTreeRecursive(0, agents.Count, 0);
            }
        }

        public void BuildObstacleTree()
        {
            obstacleTree = new ObstacleTreeNode();
            List<Obstacle> obstacles = new List<Obstacle>(_simulator.obstacles.Count);
            for (int i = 0; i < obstacles.Count; i++)
            {
                obstacles[i] = _simulator.obstacles[i];
            }
            obstacleTree = BuildObstacleTreeRecursive(obstacles);
        }

        public double ComputeAgentNeighbors(Agent agent, double rangeSq)
        {
            return QueryAgentTreeRecursive(agent, rangeSq, 0);
        }

        public void ComputeObstacleNeighbors(Agent agent, double rangeSq)
        {
            QueryObstacleTreeRecursive(agent, rangeSq, obstacleTree);
        }

        public bool QueryVisibility(Vector2 q1, Vector2 q2, float radius)
        {
            return QueryVisibilityRecursive(q1, q2, radius, obstacleTree);
        }

        private void BuildAgentTreeRecursive(int begin, int end, int node)
        {
            agentTree[node].Begin = begin;
            agentTree[node].End = end;
            agentTree[node].MinX = agentTree[node].MaxX = agents[begin].position.x;
            agentTree[node].MinY = agentTree[node].MaxY = agents[begin].position.y;

            for (int i = begin + 1; i < end; ++i)
            {
                agentTree[node].MaxX = Math.Max(agentTree[node].MaxX, agents[i].position.x);
                agentTree[node].MinX = Math.Min(agentTree[node].MinX, agents[i].position.x);
                agentTree[node].MaxY = Math.Max(agentTree[node].MaxY, agents[i].position.y);
                agentTree[node].MinY = Math.Min(agentTree[node].MinY, agents[i].position.y);
            }

            if (end - begin > MAX_LEAF_SIZE)
            {
                bool isVertical = (agentTree[node].MaxX - agentTree[node].MinX) > (agentTree[node].MaxY - agentTree[node].MinY);
                double splitValue = 0.5 * (isVertical ? agentTree[node].MaxX + agentTree[node].MinX : agentTree[node].MaxY + agentTree[node].MinY);

                int left = begin;
                int right = end;

                while (left < right)
                {
                    while (left < right && (isVertical ? agents[left].position.x : agents[left].position.y) < splitValue)
                    {
                        ++left;
                    }

                    while (right > left && (isVertical ? agents[right - 1].position.x : agents[right - 1].position.y) >= splitValue)
                    {
                        --right;
                    }

                    if (left < right)
                    {
                        Agent tmp = agents[left];
                        agents[left] = agents[right - 1];
                        agents[right - 1] = tmp;
                        ++left;
                        --right;
                    }
                }

                int leftSize = left - begin;
                if (leftSize == 0)
                {
                    ++leftSize;
                    ++left;
                    ++right;
                }

                agentTree[node].Left = node + 1;
                agentTree[node].Right = node + 2 * leftSize;

                BuildAgentTreeRecursive(begin, left, agentTree[node].Left);
                BuildAgentTreeRecursive(left, end, agentTree[node].Right);
            }
        }

        private ObstacleTreeNode BuildObstacleTreeRecursive(List<Obstacle> obstacles)
        {
            if (obstacles.Count == 0)
            {
                return null;
            }
            else
            {
                ObstacleTreeNode node = new ObstacleTreeNode();
                int optimalSplit = 0;
                int minLeft = obstacles.Count;
                int minRight = minLeft;

                for (int i = 0; i < obstacles.Count; ++i)
                {
                    int leftSize = 0;
                    int rightSize = 0;

                    Obstacle obstacleI1 = obstacles[i];
                    Obstacle obstacleI2 = obstacleI1.Next;
                    FloatPair fp1;
                    FloatPair fp2;
                    for (int j = 0; j < obstacles.Count; j++)
                    {
                        if (i == j)
                        {
                            continue;
                        }

                        Obstacle obstacleJ1 = obstacles[j];
                        Obstacle obstacleJ2 = obstacleJ1.Next;

                        double j1LeftOfI = RVOMath.LeftOf(obstacleI1.Point, obstacleI2.Point, obstacleJ1.Point);
                        double j2LeftOfI = RVOMath.LeftOf(obstacleI1.Point, obstacleI2.Point, obstacleJ2.Point);

                        if (j1LeftOfI >= -RVOMath.RVO_EPSILON && j2LeftOfI >= -RVOMath.RVO_EPSILON)
                        {
                            ++leftSize;
                        }
                        else if (j1LeftOfI <= RVOMath.RVO_EPSILON && j2LeftOfI <= RVOMath.RVO_EPSILON)
                        {
                            ++rightSize;
                        }
                        else
                        {
                            ++leftSize;
                            ++rightSize;
                        }

                        fp1 = new FloatPair(Math.Max(leftSize, rightSize), Math.Min(leftSize, rightSize));
                        fp2 = new FloatPair(Math.Max(minLeft, minRight), Math.Min(minLeft, minRight));

                        if (fp1.BigEqualThan(fp2))
                        {
                            break;
                        }
                    }

                    fp1 = new FloatPair(Math.Max(leftSize, rightSize), Math.Min(leftSize, rightSize));
                    fp2 = new FloatPair(Math.Max(minLeft, minRight), Math.Min(minLeft, minRight));

                    if (fp1.LessThan(fp2))
                    {
                        minLeft = leftSize;
                        minRight = rightSize;
                        optimalSplit = i;
                    }
                }

                {
                    /* Build split node. */
                    List<Obstacle> leftObstacles = new List<Obstacle>(minLeft);
                    for (int n = 0; n < minLeft; ++n) leftObstacles.Add(null);

                    List<Obstacle> rightObstacles = new List<Obstacle>(minRight);
                    for (int n = 0; n < minRight; ++n) rightObstacles.Add(null);

                    int leftCounter = 0;
                    int rightCounter = 0;
                    int i = optimalSplit;

                    Obstacle obstacleI1 = obstacles[i];
                    Obstacle obstacleI2 = obstacleI1.Next;

                    for (int j = 0; j < obstacles.Count; ++j)
                    {
                        if (i == j)
                        {
                            continue;
                        }

                        Obstacle obstacleJ1 = obstacles[j];
                        Obstacle obstacleJ2 = obstacleJ1.Next;

                        double j1LeftOfI = RVOMath.LeftOf(obstacleI1.Point, obstacleI2.Point, obstacleJ1.Point);
                        double j2LeftOfI = RVOMath.LeftOf(obstacleI1.Point, obstacleI2.Point, obstacleJ2.Point);

                        if (j1LeftOfI >= -RVOMath.RVO_EPSILON && j2LeftOfI >= -RVOMath.RVO_EPSILON)
                        {
                            leftObstacles[leftCounter++] = obstacles[j];
                        }
                        else if (j1LeftOfI <= RVOMath.RVO_EPSILON && j2LeftOfI <= RVOMath.RVO_EPSILON)
                        {
                            rightObstacles[rightCounter++] = obstacles[j];
                        }
                        else
                        {
                            /* Split obstacle j. */
                            var t = RVOMath.Det(obstacleI2.Point.Minus(obstacleI1.Point), obstacleJ1.Point.Minus(obstacleI1.Point)) /
                                RVOMath.Det(obstacleI2.Point.Minus(obstacleI1.Point), obstacleJ1.Point.Minus(obstacleJ2.Point));

                            Vector2 splitpoint = obstacleJ1.Point.Plus((obstacleJ2.Point.Minus(obstacleJ1.Point)).Scale(t));

                            Obstacle newObstacle = new Obstacle();
                            newObstacle.Point = splitpoint;
                            newObstacle.Previous = obstacleJ1;
                            newObstacle.Next = obstacleJ2;
                            newObstacle.Convex = true;
                            newObstacle.Direction = obstacleJ1.Direction;

                            newObstacle.Id = _simulator.obstacles.Count;

                            _simulator.obstacles.Add(newObstacle);

                            obstacleJ1.Next = newObstacle;
                            obstacleJ2.Previous = newObstacle;

                            if (j1LeftOfI > 0.0)
                            {
                                leftObstacles[leftCounter++] = obstacleJ1;
                                rightObstacles[rightCounter++] = newObstacle;
                            }
                            else
                            {
                                rightObstacles[rightCounter++] = obstacleJ1;
                                leftObstacles[leftCounter++] = newObstacle;
                            }
                        }
                    }

                    node.Obstacle = obstacleI1;
                    node.Left = BuildObstacleTreeRecursive(leftObstacles);
                    node.Right = BuildObstacleTreeRecursive(rightObstacles);
                    return node;
                }
            }
        }

        private double QueryAgentTreeRecursive(Agent agent, double rangeSq, int node)
        {
            if (agentTree[node].End - agentTree[node].Begin <= MAX_LEAF_SIZE)
            {
                for (int i = agentTree[node].Begin; i < agentTree[node].End; ++i)
                {
                    rangeSq = agent.InsertAgentNeighbor(agents[i], rangeSq);
                    
                }
            }
            else
            {
                double distSqLeft = RVOMath.Sqr(Math.Max(0, agentTree[agentTree[node].Left].MinX - agent.position.x)) +
                    RVOMath.Sqr(Math.Max(0, agent.position.x - agentTree[agentTree[node].Left].MaxX)) +
                    RVOMath.Sqr(Math.Max(0, agentTree[agentTree[node].Left].MinY - agent.position.y)) +
                    RVOMath.Sqr(Math.Max(0, agent.position.y - agentTree[agentTree[node].Left].MaxY));

                double distSqRight = RVOMath.Sqr(Math.Max(0, agentTree[agentTree[node].Right].MinX - agent.position.x)) +
                    RVOMath.Sqr(Math.Max(0, agent.position.x - agentTree[agentTree[node].Right].MaxX)) +
                    RVOMath.Sqr(Math.Max(0, agentTree[agentTree[node].Right].MinY - agent.position.y)) +
                    RVOMath.Sqr(Math.Max(0, agent.position.y - agentTree[agentTree[node].Right].MaxY));

                if (distSqLeft < distSqRight)
                {
                    if (distSqLeft < rangeSq)
                    {
                        rangeSq = QueryAgentTreeRecursive(agent, rangeSq, agentTree[node].Left);

                        if (distSqRight < rangeSq)
                        {
                            rangeSq = QueryAgentTreeRecursive(agent, rangeSq, agentTree[node].Right);
                        }
                    }
                }
                else
                {
                    if (distSqRight < rangeSq)
                    {
                        rangeSq = QueryAgentTreeRecursive(agent, rangeSq, agentTree[node].Right);

                        if (distSqLeft < rangeSq)
                        {
                            rangeSq = QueryAgentTreeRecursive(agent, rangeSq, agentTree[node].Left);
                        }
                    }
                }
            }
            return rangeSq;
        }

        private double QueryObstacleTreeRecursive(Agent agent, double rangeSq, ObstacleTreeNode node)
        {
            if (node == null)
            {
                return rangeSq;
            }
            else
            {
                Obstacle obstacle1 = node.Obstacle;
                Obstacle obstacle2 = obstacle1.Next;

                var agentLeftOfLine = RVOMath.LeftOf(obstacle1.Point, obstacle2.Point, agent.position);

                rangeSq = QueryObstacleTreeRecursive(agent, rangeSq, (agentLeftOfLine >= 0 ? node.Left : node.Right));

                double distSqLine = RVOMath.Sqr(agentLeftOfLine) / RVOMath.AbsSq(obstacle2.Point.Minus(obstacle1.Point));

                if (distSqLine < rangeSq)
                {
                    if (agentLeftOfLine < 0)
                    {
                        /*
                         * Try obstacle at this node only if is on right SideType of
                         * obstacle (and can see obstacle).
                         */
                        agent.InsertObstacleNeighbor(node.Obstacle, rangeSq);
                    }

                    /* Try other SideType of line. */
                    QueryObstacleTreeRecursive(agent, rangeSq, (agentLeftOfLine >= 0 ? node.Right : node.Left));
                }

                return rangeSq;
            }
        }

        private bool QueryVisibilityRecursive(Vector2 q1, Vector2 q2, float radius, ObstacleTreeNode node)
        {
            if (node == null)
            {
                return true;
            }
            else
            {
                Obstacle obstacle1 = node.Obstacle;
                Obstacle obstacle2 = obstacle1.Next;

                var q1LeftOfI = RVOMath.LeftOf(obstacle1.Point, obstacle2.Point, q1);
                var q2LeftOfI = RVOMath.LeftOf(obstacle1.Point, obstacle2.Point, q2);
                var invLengthI = 1.0 / RVOMath.AbsSq(obstacle2.Point.Minus(obstacle1.Point));

                if (q1LeftOfI >= 0 && q2LeftOfI >= 0)
                {
                    return QueryVisibilityRecursive(q1, q2, radius, node.Left) && ((RVOMath.Sqr(q1LeftOfI) * invLengthI >= RVOMath.Sqr(radius) && RVOMath.Sqr(q2LeftOfI) * invLengthI >= RVOMath.Sqr(radius)) || QueryVisibilityRecursive(q1, q2, radius, node.Right));
                }
                else if (q1LeftOfI <= 0 && q2LeftOfI <= 0)
                {
                    return QueryVisibilityRecursive(q1, q2, radius, node.Right) && ((RVOMath.Sqr(q1LeftOfI) * invLengthI >= RVOMath.Sqr(radius) && RVOMath.Sqr(q2LeftOfI) * invLengthI >= RVOMath.Sqr(radius)) || QueryVisibilityRecursive(q1, q2, radius, node.Left));
                }
                else if (q1LeftOfI >= 0 && q2LeftOfI <= 0)
                {
                    /* One can see through obstacle from left to right. */
                    return QueryVisibilityRecursive(q1, q2, radius, node.Left) && QueryVisibilityRecursive(q1, q2, radius, node.Right);
                }
                else
                {
                    var point1LeftOfQ = RVOMath.LeftOf(q1, q2, obstacle1.Point);
                    var point2LeftOfQ = RVOMath.LeftOf(q1, q2, obstacle2.Point);
                    var invLengthQ = 1.0 / RVOMath.AbsSq(q2.Minus(q1));

                    return (point1LeftOfQ * point2LeftOfQ >= 0 && RVOMath.Sqr(point1LeftOfQ) * invLengthQ > RVOMath.Sqr(radius) && RVOMath.Sqr(point2LeftOfQ) * invLengthQ > RVOMath.Sqr(radius) && QueryVisibilityRecursive(q1, q2, radius, node.Left) && QueryVisibilityRecursive(q1, q2, radius, node.Right));
                }
            }
        }
    }
}