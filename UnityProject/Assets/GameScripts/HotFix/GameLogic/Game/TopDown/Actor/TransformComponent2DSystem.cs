using GameLogic.Battle;
using TrueSync;
using UnityEngine;

namespace GameLogic.Battle
{
    public static class TransformComponent2DSystem
    {
         public static bool OverlapsWithCircle(this TransformComponent self,TSVector center, FP radius)
         {
             var selfCenter = self.CenterPosition.ToVector();

            switch (self.Shape)
            {
                case ERoleShape.Circle:
                    return MathUtils.IsCircleOverlaps(center.ToVector(), radius.AsFloat(), selfCenter, self.Radius.AsFloat());

                case ERoleShape.Rect:
                    var tempRect = new GameLogic.Battle.Rect(selfCenter, self.Radius.AsFloat() * 2, self.Height.AsFloat(), self.Forward.ToVector());

                    return tempRect.OverlapsWithCircle(center.ToVector(), radius.AsFloat());

                default:
                    return false;
            }
        }

        public static bool OverlapsWithRect(this TransformComponent self, Rect rect)
        {
            var selfCenter = self.CenterPosition.ToVector();

            switch (self.Shape)
            {
                case ERoleShape.Circle:
                    return rect.OverlapsWithCircle(selfCenter, self.Radius.AsFloat());

                case ERoleShape.Rect:
                    var tempRect = new Rect(selfCenter, self.Radius.AsFloat() * 2, self.Height.AsFloat(), self.Forward.ToVector());
                    return tempRect.OverlapsWithRect(rect);

                default:
                    return false;
            }
        }

        public static bool OverlapsWithSector(this TransformComponent self, Sector sector)
        {
            var selfCenter = self.CenterPosition.ToVector();
            return sector.OverlapsWithCircle(selfCenter, self.Radius.AsFloat());
        }

        public static bool ContainsPoint(this TransformComponent self, Vector3 point)
        {
            var selfCenter = self.CenterPosition.ToVector();
            switch (self.Shape)
            {
                case ERoleShape.Circle:
                    return MathUtils.IsPointInCircle(selfCenter, self.Radius.AsFloat(), point);

                case ERoleShape.Rect:
                    var tempRect = new Rect(selfCenter, self.Radius.AsFloat() * 2, self.Height.AsFloat(), self.Forward.ToVector());
                    return tempRect.ContainsPoint(point);

                default:
                    return false;
            }
        }

        public static bool OverlapsWithTransform(this TransformComponent self, TransformComponent target)
        {
            if (target.Shape == ERoleShape.Circle)
            {
                return self.OverlapsWithCircle(target.CenterPosition, target.Radius);
            }
            else if (target.Shape == ERoleShape.Rect)
            {
                Rect rect = new Rect(target.CenterPosition.ToVector()
                    ,self.Radius.AsFloat() * self.Radius.AsFloat(), target.Height.AsFloat(), target.Forward.ToVector());
                return self.OverlapsWithRect(rect);
            }

            return false;
        }
    }
}