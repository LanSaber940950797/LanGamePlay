using UnityEngine;

namespace GameLogic.Battle
{
    public static class MathUtils
    {
        public static readonly float Epsilon = 0.0001f;

        public static float Sqr(float x)
        {
            return x * x;
        }

        public static float MaxSqr(float x, float y)
        {
            return Mathf.Max(x, y) * Mathf.Max(x, y);
        }

        public static void RandomPosition(Vector3 outVec, float distance)
        {
            outVec.Set(distance * 0.7071f, 0, 0);

            if (BattleDefine.WorldType == EBattleWorldType.TwoDimensional)
            {
                outVec.y = outVec.x * (RandomUtils.Result(50) ? -1 : 1);
            }
            else
            {
                outVec.z = outVec.x * (RandomUtils.Result(50) ? -1 : 1);
            }

            outVec.x = outVec.x * (RandomUtils.Result(50) ? -1 : 1);
        }

        public static void RotateVector(ref Vector3 outVec, Vector3 v, float a)
        {
            if (BattleDefine.WorldType == EBattleWorldType.TwoDimensional)
            {
                outVec = Quaternion.Euler(0, 0, a) * v;
            }
            else
            {
                outVec = Quaternion.Euler(0, a, 0) * v;
            }
        }

        public static bool IsPointInCircle(Vector3 center, float radius, Vector3 point)
        {
            float diff_x = center.x - point.x;
            float diff_y = 0;

            if (BattleDefine.WorldType == EBattleWorldType.TwoDimensional)
            {
                diff_y = center.y - point.y;
            }
            else
            {
                diff_y = center.z - point.z;
            }

            if (Mathf.Abs(diff_x) >= radius || Mathf.Abs(diff_y) >= radius)
            {
                return false;
            }

            return diff_x * diff_x + diff_y * diff_y <= radius * radius;
        }

        public static bool IsCircleOverlaps(Vector3 center1, float circle1_radius, Vector3 center2, float circle2_radius)
        {
            float diff_x = center1.x - center2.x;
            float diff_y = 0;

            if (BattleDefine.WorldType == EBattleWorldType.TwoDimensional)
            {
                diff_y = center1.y - center2.y;
            }
            else
            {
                diff_y = center1.z - center2.z;
            }

            float radius = circle1_radius + circle2_radius;

            if (Mathf.Abs(diff_x) >= radius || Mathf.Abs(diff_y) >= radius)
            {
                return false;
            }

            return diff_x * diff_x + diff_y * diff_y <= radius * radius;
        }

        public static bool IsLineCircleIntersect(Vector3 point1, Vector3 point2, Vector3 center, float circle_radius)
        {
            float distance1 = Vector3.SqrMagnitude(point1 - center);
            float distance2 = Vector3.SqrMagnitude(point2 - center);
            float radiusPow = circle_radius * circle_radius;

            if (distance1 <= radiusPow || distance2 <= radiusPow)
            {
                return true;
            }

            float a = 0;
            float b = point1.x - point2.x;
            float c = 0;
            float distPow = 0;

            if (BattleDefine.WorldType == EBattleWorldType.TwoDimensional)
            {
                a = point2.y - point1.y;
                c = point2.x * point1.y - point1.x * point2.y;
                distPow = Mathf.Pow((a * center.x + b * center.y + c), 2);
            }
            else
            {
                a = point2.z - point1.z;
                c = point2.x * point1.z - point1.x * point2.z;
                distPow = Mathf.Pow((a * center.x + b * center.z + c), 2);
            }

            if (distPow <= radiusPow)
            {
                return true;
            }

            return false;
        }

        public static float PointToLineSegmentDistanceSqr(Vector3 startPoint, Vector3 endPoint, Vector3 otherPoint)
        {
            Vector3 tempVector1 = otherPoint - startPoint;
            Vector3 tempVector2 = endPoint - startPoint;
            float r = Vector3.Dot(tempVector1, tempVector2) / tempVector2.sqrMagnitude;

            if (r < 0)
            {
                return tempVector1.sqrMagnitude;
            }
            else if (r > 1)
            {
                tempVector1 = otherPoint - endPoint;

                return tempVector1.sqrMagnitude;
            }
            else
            {
                tempVector1 = startPoint + tempVector2 * r;
                tempVector2 = otherPoint - tempVector1;

                return tempVector2.sqrMagnitude;
            }
        }

        public static void ShuffleArray<T>(T[] array)
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = Mathf.FloorToInt(Random.value * (i + 1));
                T temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }
    }
    }