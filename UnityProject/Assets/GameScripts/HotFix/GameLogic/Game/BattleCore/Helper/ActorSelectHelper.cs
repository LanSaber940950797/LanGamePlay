using System.Collections.Generic;
using ET;

//using DG.DemiEditor;

namespace GameLogic.Battle
{
    /// <summary>
    /// 战斗单位选择器
    /// </summary>
    public static class ActorSelectHelper
    {
        public static TargetSideType GetFriend(this Actor self, Actor target)
        {
            if (self.SideType == target.SideType)
            {
                return TargetSideType.Friend;
            }

            return TargetSideType.Enemy;
        }



        public static List<EntityRef<Actor>> GetActors(Actor self, TargetSideType targetSide, bool isNoSelf)
        {
            var actorComponent = self.GetParent<ActorComponent>();
            List<EntityRef<Actor>> list = new List<EntityRef<Actor>>();
            var allActor = actorComponent.GetAllActors();
            foreach (var unit in allActor)
            {
                if (targetSide == TargetSideType.None || self.GetFriend(unit) == targetSide)
                {
                    list.Add(unit);
                }
            }
            return list;
        }
    }
}