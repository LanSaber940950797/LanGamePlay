using System;
using System.Collections.Generic;


namespace Rvo2
{
    public class Agent
    {
        public List<KeyValuePair<double, Agent>> agentNeighbors = new List<KeyValuePair<double, Agent>>();
        public List<KeyValuePair<double, Obstacle>> obstaclNeighbors = new List<KeyValuePair<double, Obstacle>>();
        public List<Line> orcaLines = new List<Line>();
        public Vector2 position = new Vector2(0, 0);
        public Vector2 prefVelocity = new Vector2(0, 0);
        public Vector2 velocity = new Vector2(0, 0);
        public int id { get; set; } = 0;
        public int maxNeighbors_ { get; set; } = 0;
        public float maxSpeed_ { get; set; } = 0.0f;
        private float _neighborDist = 0.0f;

        public float neighborDist
        {
            get { return _neighborDist; }
            set { _neighborDist = value; }
        }

        public float Radius { get; set; } = 0.0f;
        public float timeHorizon { get; set; } = 0.0f;
        public float timeHorizonObst { get; set; } = 0.0f;
        private Vector2 _newVelocity = new Vector2(0, 0);
        public float mass { get; set; } = 1;
        public bool isStopped { get; set; } = false;

        public void ComputeNeighbors(Simulator sim)
        {
            obstaclNeighbors.Clear();
            double rangeSq = Math.Pow((timeHorizonObst * maxSpeed_ + Radius), 2);
            sim.kdTree.ComputeObstacleNeighbors(this, rangeSq);

            agentNeighbors.Clear();

            if (maxNeighbors_ > 0)
            {
                rangeSq = Math.Pow(neighborDist, 2);
                rangeSq = sim.kdTree.ComputeAgentNeighbors(this, rangeSq);
            }
        }

        public void ComputeNewVelocity(double dt)
        {
            this.orcaLines.Clear();
            List<Line> orcaLines = this.orcaLines;

            var invTimeHorizonObst = 1.0f / timeHorizonObst;

            for (int i = 0; i < obstaclNeighbors.Count; ++i)
            {
                Obstacle obstacle1 = obstaclNeighbors[i].value;
                Obstacle obstacle2 = obstacle1.Next;

                Vector2 relativePosition1 = obstacle1.Point.Minus(position);
                Vector2 relativePosition2 = obstacle2.Point.Minus(position);

                bool alreadyCovered = false;

                for (int j = 0; j < orcaLines.Count; ++j)
                {
                    if (RVOMath.Det(relativePosition1.Scale((float)invTimeHorizonObst).Minus(orcaLines[j].point),
                            orcaLines[j].direction) - invTimeHorizonObst * Radius >= -RVOMath.RVO_EPSILON
                        && RVOMath.Det(relativePosition2.Scale((float)invTimeHorizonObst).Minus(orcaLines[j].point),
                            orcaLines[j].direction) - invTimeHorizonObst * Radius >= -RVOMath.RVO_EPSILON)
                    {
                        alreadyCovered = true;
                        break;
                    }
                }

                if (alreadyCovered)
                {
                    continue;
                }

                var distSq1 = RVOMath.AbsSq(relativePosition1);
                var distSq2 = RVOMath.AbsSq(relativePosition2);

                double radiusSq = RVOMath.Sqr(Radius);

                Vector2 obstacleVector = obstacle2.Point.Minus(obstacle1.Point);
                double s = relativePosition1.Scale(-1).Multiply(obstacleVector) / RVOMath.AbsSq(obstacleVector);
                double distSqLine = RVOMath.AbsSq(relativePosition1.Scale(-1).Minus(obstacleVector.Scale((float)s)));

                Line line = new Line();
                if (s < 0 && distSq1 <= radiusSq)
                {
                    if (obstacle1.Convex)
                    {
                        line.point = new Vector2(0, 0);
                        line.direction = RVOMath.Normalize(new Vector2(-relativePosition1.y, relativePosition1.x));
                        orcaLines.Add(line);
                    }

                    continue;
                }
                else if (s > 1 && distSq2 <= radiusSq)
                {
                    if (obstacle2.Convex && RVOMath.Det(relativePosition2, obstacle2.Direction) >= 0)
                    {
                        line.point = new Vector2(0, 0);
                        line.direction = RVOMath.Normalize(new Vector2(-relativePosition2.y, relativePosition2.x));
                        orcaLines.Add(line);
                    }

                    continue;
                }
                else if (s >= 0 && s <= 1 && distSqLine <= radiusSq)
                {
                    line.point = new Vector2(0, 0);
                    line.direction = obstacle1.Direction.Scale(-1);
                    orcaLines.Add(line);
                    continue;
                }

                Vector2 leftLegDirection, rightLegDirection;

                if (s < 0 && distSqLine <= radiusSq)
                {
                    if (!obstacle1.Convex)
                    {
                        continue;
                    }

                    obstacle2 = obstacle1;

                    var leg1 = (float)Math.Sqrt(distSq1 - radiusSq);
                    leftLegDirection = (new Vector2(relativePosition1.x * leg1 - relativePosition1.y * Radius,
                        relativePosition1.x * Radius + relativePosition1.y * leg1)).Scale(1 / distSq1);
                    rightLegDirection = (new Vector2(relativePosition1.x * leg1 + relativePosition1.y * Radius,
                        -relativePosition1.x * Radius + relativePosition1.y * leg1)).Scale(1 / distSq1);
                }
                else if (s > 1 && distSqLine <= radiusSq)
                {
                    if (!obstacle2.Convex)
                    {
                        continue;
                    }

                    obstacle1 = obstacle2;

                    var leg2 = (float)Math.Sqrt(distSq2 - radiusSq);
                    leftLegDirection = (new Vector2(relativePosition2.x * leg2 - relativePosition2.y * Radius,
                        relativePosition2.x * Radius + relativePosition2.y * leg2)).Scale(1 / distSq2);
                    rightLegDirection = (new Vector2(relativePosition2.x * leg2 + relativePosition2.y * Radius,
                        -relativePosition2.x * Radius + relativePosition2.y * leg2)).Scale(1 / distSq2);
                }
                else
                {
                    if (obstacle1.Convex)
                    {
                        var leg1 = (float)Math.Sqrt(distSq1 - radiusSq);
                        leftLegDirection = (new Vector2(relativePosition1.x * leg1 - relativePosition1.y * Radius,
                            relativePosition1.x * Radius + relativePosition1.y * leg1)).Scale(1 / distSq1);
                    }
                    else
                    {
                        leftLegDirection = obstacle1.Direction.Scale(-1);
                    }

                    if (obstacle2.Convex)
                    {
                        var leg2 = (float)Math.Sqrt(distSq2 - radiusSq);
                        rightLegDirection = (new Vector2(relativePosition2.x * leg2 + relativePosition2.y * Radius,
                            -relativePosition2.x * Radius + relativePosition2.y * leg2)).Scale(1 / distSq2);
                    }
                    else
                    {
                        rightLegDirection = obstacle1.Direction;
                    }
                }

                var leftNeighbor = obstacle1.Previous;

                bool isLeftLegForeign = false;
                bool isRightLegForeign = false;

                if (obstacle1.Convex && RVOMath.Det(leftLegDirection, leftNeighbor.Direction.Scale(-1)) >= 0.0)
                {
                    leftLegDirection = leftNeighbor.Direction.Scale(-1);
                    isLeftLegForeign = true;
                }

                if (obstacle2.Convex && RVOMath.Det(rightLegDirection, obstacle2.Direction) <= 0.0)
                {
                    rightLegDirection = obstacle2.Direction;
                    isRightLegForeign = true;
                }

                Vector2 leftCutoff = obstacle1.Point.Minus(position).Scale(invTimeHorizonObst);
                Vector2 rightCutoff = obstacle2.Point.Minus(position).Scale(invTimeHorizonObst);
                Vector2 cutoffVec = rightCutoff.Minus(leftCutoff);

                Vector2 w = velocity.Minus(leftCutoff).Minus(cutoffVec.Scale(0.5f));
                var wLengthSq = RVOMath.AbsSq(w);

                var dotProduct1 = w.Multiply(relativePosition1);

                if (dotProduct1 < 0.0 && RVOMath.Sqr(dotProduct1) > radiusSq * wLengthSq)
                {
                    Vector2 unitW = RVOMath.Normalize(w);
                    line.direction = new Vector2(unitW.y, -unitW.x);
                    line.point = leftCutoff.Plus(unitW.Scale(Radius * invTimeHorizonObst));
                    orcaLines.Add(line);
                    continue;
                }
                else if (dotProduct1 >= 0.0 && RVOMath.Det(w, cutoffVec) <= 0.0)
                {
                    line.direction = obstacle1.Direction.Scale(-1);
                    line.point = leftCutoff.Plus(line.direction.Scale(Radius * invTimeHorizonObst));
                    orcaLines.Add(line);
                    continue;
                }

                var dotProduct2 = w.Multiply(relativePosition2);

                if (dotProduct2 < 0.0 && RVOMath.Sqr(dotProduct2) > radiusSq * wLengthSq)
                {
                    Vector2 unitW = RVOMath.Normalize(w);
                    line.direction = new Vector2(unitW.y, -unitW.x);
                    line.point = rightCutoff.Plus(unitW.Scale(Radius * invTimeHorizonObst));
                    orcaLines.Add(line);
                    continue;
                }
                else if (dotProduct2 >= 0.0 && RVOMath.Det(w, cutoffVec) >= 0.0)
                {
                    line.direction = obstacle1.Direction;
                    line.point = rightCutoff.Plus(line.direction.Scale(Radius * invTimeHorizonObst));
                    orcaLines.Add(line);
                    continue;
                }

                double distSqCutoff =
                    RVOMath.AbsSq(w.Minus(cutoffVec.Scale(RVOMath.Det(cutoffVec, w) / RVOMath.AbsSq(cutoffVec))));
                double distSqLeft =
                    RVOMath.AbsSq(w.Minus(
                        leftLegDirection.Scale(RVOMath.Det(leftLegDirection, w) / RVOMath.AbsSq(leftLegDirection))));
                double distSqRight =
                    RVOMath.AbsSq(w.Minus(rightLegDirection.Scale(RVOMath.Det(rightLegDirection, w) /
                                                                  RVOMath.AbsSq(rightLegDirection))));

                if (distSqCutoff <= distSqLeft && distSqCutoff <= distSqRight)
                {
                    line.direction = obstacle1.Direction.Scale(-1);
                    Vector2 aux = new Vector2(-line.direction.y, line.direction.x);
                    line.point = aux.Scale(Radius * invTimeHorizonObst).Plus(leftCutoff);
                    orcaLines.Add(line);
                    continue;
                }
                else if (distSqLeft <= distSqRight)
                {
                    if (isLeftLegForeign)
                    {
                        continue;
                    }

                    line.direction = leftLegDirection;
                    Vector2 aux = new Vector2(-line.direction.y, line.direction.x);
                    line.point = aux.Scale(Radius * invTimeHorizonObst).Plus(leftCutoff);
                    orcaLines.Add(line);
                    continue;
                }
                else
                {
                    if (isRightLegForeign)
                    {
                        continue;
                    }

                    line.direction = rightLegDirection.Scale(-1);
                    Vector2 aux = new Vector2(-line.direction.y, line.direction.x);
                    line.point = aux.Scale(Radius * invTimeHorizonObst).Plus(rightCutoff);
                    orcaLines.Add(line);
                    continue;
                }
            }

            int numObstLines = orcaLines.Count;

            var invTimeHorizon = (float)1.0 / timeHorizon;

            for (int i = 0; i < agentNeighbors.Count; ++i)
            {
                Agent other = agentNeighbors[i].value;

                Vector2 relativePosition = other.position.Minus(position);

                var massRatio = (other.mass / (mass + other.mass));
                var neighborMassRatio = (mass / (mass + other.mass));

                Vector2 velocityOpt = (massRatio >= 0.5
                    ? (velocity.Minus(velocity.Scale(massRatio)).Scale(2))
                    : prefVelocity.Plus(velocity.Minus(prefVelocity).Scale(massRatio * 2)));
                Vector2 neighborVelocityOpt = (neighborMassRatio >= 0.5
                    ? other.velocity.Scale(2).Scale(1 - neighborMassRatio)
                    : (other.prefVelocity.Plus(other.velocity.Minus(other.prefVelocity).Scale(2 * neighborMassRatio))));

                Vector2 relativeVelocity = velocityOpt.Minus(neighborVelocityOpt);
                var distSq = RVOMath.AbsSq(relativePosition);
                var combinedRadius = Radius + other.Radius;
                var combinedRadiusSq = RVOMath.Sqr(combinedRadius);

                Line line = new Line();
                Vector2 u;

                if (distSq > combinedRadiusSq)
                {
                    Vector2 w = relativeVelocity.Minus(relativePosition.Scale(invTimeHorizon));
                    var wLengthSq = RVOMath.AbsSq(w);

                    var dotProduct1 = w.Multiply(relativePosition);

                    if (dotProduct1 < 0.0 && RVOMath.Sqr(dotProduct1) > combinedRadiusSq * wLengthSq)
                    {
                        Vector2 unitW = RVOMath.Normalize(w);
                        line.direction = new Vector2(unitW.y, -unitW.x);
                        u = unitW.Scale(combinedRadius * invTimeHorizon - (float)Math.Sqrt(distSq - combinedRadiusSq));
                    }
                    else
                    {
                        var leg = (float)Math.Sqrt(distSq - combinedRadiusSq);

                        if (RVOMath.Det(relativePosition, w) > 0.0)
                        {
                            Vector2 aux = new Vector2(relativePosition.x * leg - relativePosition.y * combinedRadius,
                                relativePosition.x * combinedRadius + relativePosition.y * leg);
                            line.direction = aux.Scale(1 / distSq);
                        }
                        else
                        {
                            var aux = new Vector2(relativePosition.x * leg + relativePosition.y * combinedRadius,
                                -relativePosition.x * combinedRadius + relativePosition.y * leg);
                            line.direction = aux.Scale(1 / distSq);
                        }

                        var dotProduct2 = relativeVelocity.Multiply(line.direction);
                        u = line.direction.Scale(dotProduct2).Minus(relativeVelocity);
                    }
                }
                else
                {
                    var invTimeStep = (float)1.0 / dt;

                    Vector2 w = relativeVelocity.Minus(relativePosition.Scale((float)invTimeStep));

                    var wLength = RVOMath.Abs(w);
                    Vector2 unitW = w.Scale(1 / wLength);

                    line.direction = new Vector2(unitW.y, -unitW.x);
                    u = unitW.Scale((float)(combinedRadius * invTimeStep - wLength));
                }

                line.point = velocityOpt.Plus(u.Scale(massRatio));
                orcaLines.Add(line);
            }

            int lineFail = LinearProgram2(orcaLines, maxSpeed_, prefVelocity, false, _newVelocity);

            if (lineFail < orcaLines.Count)
            {
                LinearProgram3(orcaLines, numObstLines, lineFail, maxSpeed_, _newVelocity);
            }
        }

        public double InsertAgentNeighbor(Agent agent, double rangeSq)
        {
            if (this != agent)
            {
                double distSq = RVOMath.AbsSq(position.Minus(agent.position));

                if (distSq < rangeSq)
                {
                    if (agentNeighbors.Count < maxNeighbors_)
                    {
                        agentNeighbors.Add(new KeyValuePair<double, Agent>(distSq, agent));
                    }

                    int i = agentNeighbors.Count - 1;
                    while (i != 0 && distSq < agentNeighbors[i - 1].key)
                    {
                        agentNeighbors[i] = agentNeighbors[i - 1];
                        --i;
                    }

                    agentNeighbors[i] = new KeyValuePair<double, Agent>(distSq, agent);

                    if (agentNeighbors.Count == maxNeighbors_)
                    {
                        rangeSq = agentNeighbors[agentNeighbors.Count - 1].key;
                    }
                }
            }

            return rangeSq;
        }


        public void InsertObstacleNeighbor(Obstacle obstacle, double rangeSq)
        {
            Obstacle nextObstacle = obstacle.Next;

            double distSq = RVOMath.DistSqPointLineSegment(obstacle.Point, nextObstacle.Point, position);

            if (distSq < rangeSq)
            {
                obstaclNeighbors.Add(new KeyValuePair<double, Obstacle>(distSq, obstacle));

                int i = obstaclNeighbors.Count - 1;
                while (i != 0 && distSq < obstaclNeighbors[i - 1].key)
                {
                    obstaclNeighbors[i] = obstaclNeighbors[i - 1];
                    --i;
                }

                obstaclNeighbors[i] = new KeyValuePair<double, Obstacle>(distSq, obstacle);
            }
        }

        public void Update(float dt)
        {
            velocity.Copy(_newVelocity);
            position.Copy(position.Plus(velocity.Scale(dt)));
        }

        public bool LinearProgram1(List<Line> lines, int lineNo, float radius, Vector2 optVelocity, bool directionOpt, Vector2 result)
        {
            var dotProduct = lines[lineNo].point.Multiply(lines[lineNo].direction);
            var discriminant = RVOMath.Sqr(dotProduct) + RVOMath.Sqr(radius) - RVOMath.AbsSq(lines[lineNo].point);

            if (discriminant < 0.0)
            {
                return false;
            }

            var sqrtDiscriminant = Math.Sqrt(discriminant);
            var tLeft = -dotProduct - sqrtDiscriminant;
            var tRight = -dotProduct + sqrtDiscriminant;

            for (int i = 0; i < lineNo; ++i)
            {
                double denominator = RVOMath.Det(lines[lineNo].direction, lines[i].direction);
                double numerator = RVOMath.Det(lines[i].direction, lines[lineNo].point.Minus(lines[i].point));

                if (Math.Abs(denominator) <= RVOMath.RVO_EPSILON)
                {
                    if (numerator < 0.0)
                    {
                        return false;
                    }

                    continue;
                }

                double t = numerator / denominator;

                if (denominator >= 0.0)
                {
                    tRight = Math.Min(tRight, t);
                }
                else
                {
                    tLeft = Math.Max(tLeft, t);
                }

                if (tLeft > tRight)
                {
                    return false;
                }
            }

            if (directionOpt)
            {
                if (optVelocity.Multiply(lines[lineNo].direction) > 0.0)
                {
                    result.Copy(lines[lineNo].point.Minus(optVelocity.Scale((float)tRight)));
                }
                else
                {
                    result.Copy(lines[lineNo].point.Minus(optVelocity.Scale((float)tLeft)));
                }
            }
            else
            {
                double t = lines[lineNo].direction.Multiply(optVelocity.Minus(lines[lineNo].point));

                if (t < tLeft)
                {
                    result.Copy(lines[lineNo].point.Minus(optVelocity.Scale((float)tLeft)));
                }
                else if (t > tRight)
                {
                    result.Copy(lines[lineNo].point.Minus(optVelocity.Scale((float)tRight)));
                }
                else
                {
                    result.Copy(lines[lineNo].point.Plus(lines[lineNo].direction.Scale((float)-t)));
                }
            }

            return true;
        }
        
        public int LinearProgram2(List<Line> lines, float radius, Vector2 optVelocity, bool directionOpt, Vector2 result)
        {
            if (directionOpt)
            {
                result.Copy(optVelocity.Scale(radius));
            }
            else if (RVOMath.AbsSq(optVelocity) > RVOMath.Sqr(radius))
            {
                result.Copy(RVOMath.Normalize(optVelocity).Scale(radius));
            }
            else
            {
                result.Copy(optVelocity);
            }

            for (int i = 0; i < lines.Count; ++i)
            {
                if (RVOMath.Det(lines[i].direction, lines[i].point.Minus(result)) > 0.0)
                {
                    var tmpResult = result.Clone();

                    double time = 0.0;
                    if (!LinearProgram1(lines, i, radius, optVelocity, directionOpt, result))
                    {
                        result.Copy(tmpResult);
                        return i;
                    }
                }
            }

            return lines.Count;
        }

        public void LinearProgram3(List<Line> lines, int numObstLines, int beginLine, float radius, Vector2 result)
        {
            double distance = 0.0f;
            // 遍历所有剩余ORCA线
            for (int i = beginLine; i < lines.Count; ++i)
            {// 每一条 ORCA 线都需要精确的做出处理，distance 为 最大违规的速度
                if (RVOMath.Det(lines[i].direction, lines[i].point.Minus(result)) > distance)
                {
                    List<Line> projLines = new List<Line>();
                    for (int j = 0; j < numObstLines; ++j)
                    {
                        projLines.Add(new Line());
                    }

                    for (int j = 0; j < numObstLines; ++j)
                    {
                        Line line = new Line();
                        var determinant = RVOMath.Det(lines[i].direction, lines[j].direction);
                        if(Math.Abs(determinant) <= RVOMath.RVO_EPSILON)
                        {
                            if (lines[i].direction.Multiply(lines[j].direction) > 0.0)
                            {
                                continue;
                            }
                            else
                            {
                                line.point = lines[i].point.Plus(lines[j].point).Scale(0.5f);
                            }
                        }
                        else
                        {
                            line.point = lines[i].point.Plus(lines[i].direction.Scale(RVOMath.Det(lines[j].direction, lines[i].point.Minus(lines[j].point)) / determinant));
                        }
                        line.direction = RVOMath.Normalize(lines[j].direction.Minus(lines[i].direction));
                        projLines.Add(line);
                    }
                    var tempResult = result.Clone();
                    if (LinearProgram2(projLines, radius, new Vector2(-lines[i].direction.y, lines[i].direction.x), true, result) < projLines.Count)
                    {
                        result.Copy(tempResult);
                    }

                    distance = RVOMath.Det(lines[i].direction, lines[i].point.Minus(result));
                }
            }
        }
    }
}