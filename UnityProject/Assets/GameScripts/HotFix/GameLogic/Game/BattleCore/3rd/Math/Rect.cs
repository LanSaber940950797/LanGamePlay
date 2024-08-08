
using UnityEngine;
using Rvo2;
using Vector2 = Rvo2.Vector2;
namespace GameLogic.Battle
{
    public class Rect
    {
        private Vector2 _center;
        private Vector2 _direction;
        private float _width;
        private float _height;
        private Vector2 _corn1;
        private Vector2 _corn2;
        private Vector2 _corn3;
        private Vector2 _corn4;
        private Vector2 _axis1;
        private Vector2 _axis2;

        public Rect(Vector3 center, float width, float height, Vector3 direction)
        {
            Init(center, width, height, direction);
        }

        public void Init(Vector3 center, float width, float height, Vector3 direction)
        {
            if (BattleDefine.WorldType == EBattleWorldType.TwoDimensional)
            {
                _center = new Vector2(center.x, center.y);
                _direction = new Vector2(direction.x, direction.y);
            }
            else
            {
                _center = new Vector2(center.x, center.z);
                _direction = new Vector2(direction.x, direction.z);
            }

            _width = width;
            _height = height;
            Vector2 xAxis = new Vector2(_direction.x, _direction.y);
            Vector2 yAxis = new Vector2(-_direction.y, _direction.x);

            xAxis *= width * 0.5f;
            yAxis *= height * 0.5f;

            _corn1 = new Vector2(_center.x, _center.y);
            _corn2 = new Vector2(_center.x, _center.y);
            _corn3 = new Vector2(_center.x, _center.y);
            _corn4 = new Vector2(_center.x, _center.y);

            _corn1.Substract(_corn1, xAxis);
            _corn1.Substract(_corn1, yAxis);
            _corn2 = _corn2.Plus(xAxis);
            _corn2.Substract(_corn2, yAxis);
            _corn3 = _corn3.Plus(xAxis);
            _corn3 = _corn3.Plus(yAxis);
            _corn4.Substract(_corn4,xAxis);
            _corn4 = _corn4.Plus(yAxis);
            ComputeAxis();
        }

        public bool OverlapsWithCircle(Vector3 center, float radius)
        {
            Vector2 tempVec;
            if (BattleDefine.WorldType == EBattleWorldType.TwoDimensional)
            {
                tempVec = new Vector2(center.x, center.y);
            }
            else
            {
                tempVec = new Vector2(center.x, center.z);
            }

            tempVec.Substract(tempVec, _corn1);
           
            float proj1 = Vector2.Dot(tempVec, _axis1);
            float proj2 = Vector2.Dot(tempVec, _axis2);
            float axis1Min = -radius;
            float axis1Max = _width + radius;
            float axis2Min = -radius;
            float axis2Max = _height + radius;

            if (proj1 > axis1Min && proj1 < axis1Max && proj2 > axis2Min && proj2 < axis2Max)
            {
                return true;
            }

            return false;
        }

        public bool OverlapsWithRect(Rect other)
        {
            return InternalOverlapsWithRect(other) && other.InternalOverlapsWithRect(this);
        }

        public bool ContainsPoint(Vector3 point)
        {
            Vector2 tempVec;
            if (BattleDefine.WorldType == EBattleWorldType.TwoDimensional)
            {
                tempVec = new Vector2(point.x, point.y);
            }
            else
            {
                tempVec = new Vector2(point.x, point.z);
            }
            
            tempVec.Substract(tempVec, _corn1);
            float proj1 = Vector2.Dot(tempVec, _axis1);
            float proj2 = Vector2.Dot(tempVec, _axis2);
            float axis1Min = 0;
            float axis1Max = _width;
            float axis2Min = 0;
            float axis2Max = _height;

            if (proj1 > axis1Min && proj1 < axis1Max && proj2 > axis2Min && proj2 < axis2Max)
            {
                return true;
            }

            return false;
        }

        private void ComputeAxis()
        {
            _axis1 = _corn2.Substract(_corn2, _corn1);
            _axis1.Normalize();
            _axis2 = _corn4.Substract(_corn4, _corn3);
            _axis2.Normalize();
        }

        private bool InternalOverlapsWithRect(Rect other)
        {
            float axis1Min = Vector2.Dot(_corn1, _axis1);
            float axis1Max = Vector2.Dot(_corn3, _axis1);

            ComputeProjection(other, _axis1, out Vector2 tempProjection);
            if (tempProjection.x > axis1Max || tempProjection.y < axis1Min)
            {
                return false;
            }

            float axis2Min = Vector2.Dot(_corn1, _axis2);
            float axis2Max = Vector2.Dot(_corn3, _axis2);

            ComputeProjection(other, _axis2, out tempProjection);
            if (tempProjection.x > axis2Max || tempProjection.y < axis2Min)
            {
                return false;
            }

            return true;
        }

        private void ComputeProjection(Rect other, Vector2 axis, out Vector2 outProjection)
        {
            float value = Vector2.Dot(other._corn1, axis);

            outProjection = new Vector2(value, value);
            value = Vector2.Dot(other._corn2, axis);
            if (value < outProjection.x)
            {
                outProjection.x = value;
            }
            else if (value > outProjection.y)
            {
                outProjection.y = value;
            }

            value = Vector2.Dot(other._corn3, axis);
            if (value < outProjection.x)
            {
                outProjection.x = value;
            }
            else if (value > outProjection.y)
            {
                outProjection.y = value;
            }

            value = Vector2.Dot(other._corn4, axis);
            if (value < outProjection.x)
            {
                outProjection.x = value;
            }
            else if (value > outProjection.y)
            {
                outProjection.y = value;
            }
        }
    }
}