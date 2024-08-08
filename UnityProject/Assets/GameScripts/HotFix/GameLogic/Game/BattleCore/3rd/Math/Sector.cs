using UnityEngine;

namespace GameLogic.Battle
{
    public class Sector
    {
        private Vector3 _center = Vector3.zero;
        private Vector3 _direction = Vector3.zero;
        private float _radius = 0;
        private float _angle = 0;
        private float? _angleSinValue = null;
        private float? _angleCosValue = null;

        public  Sector(Vector3 center, Vector3 direction, float angle, float radius)
        {
            _center = center;
            _direction = direction;
            _radius = radius;
            if (_angle != angle)
            {
                _angleSinValue = null;
                _angleCosValue = null;
            }

            _angle = angle;
        }

        public bool OverlapsWithCircle(Vector3 center, float radius)
        {
            Vector3 tempCenterVector = center - _center;

            float radiusSum = _radius + radius;

            if (tempCenterVector.sqrMagnitude > radiusSum * radiusSum)
            {
                return false;
            }

            float relativeCircleCenterX = Vector3.Dot(tempCenterVector, _direction);
            float relativeCircleCenterY = Mathf.Abs(tempCenterVector.x * (-_direction.y) + tempCenterVector.y * _direction.x);

            if (_angleCosValue == null)
            {
                _angleCosValue = Mathf.Cos(Mathf.Deg2Rad * (_angle * 0.5f));
            }

            if (relativeCircleCenterX > tempCenterVector.magnitude * _angleCosValue)
            {
                return true;
            }

            if (_angleSinValue == null)
            {
                _angleSinValue = Mathf.Sin(Mathf.Deg2Rad * (_angle * 0.5f));
            }

            Vector3 tempCenterVector2 = new Vector3(_angleCosValue.Value, _angleSinValue.Value, 0);
            Vector3 tempPosition = new Vector3(relativeCircleCenterX, relativeCircleCenterY, 0);

            return MathUtils.PointToLineSegmentDistanceSqr(Vector3.zero, tempCenterVector2, tempPosition) <= radius * radius;
        }
    }
}