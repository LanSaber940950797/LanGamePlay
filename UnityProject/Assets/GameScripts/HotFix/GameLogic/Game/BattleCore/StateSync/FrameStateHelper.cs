using GameLogic.Battle;

namespace ET
{
    public static class FrameStateHelper
    {
        public static bool Check(OneFrameState x, OneFrameState y)
        {
            if (x.States.Count != y.States.Count)
            {
                return false;
            }
            
            for (int i = 0; i < x.States.Count; i++)
            {
                if (!Check(x.States[i], y.States[i]))
                {
                    return false;
                }
            }
            return  true;
        }

        private static bool Check(StateFrame x, StateFrame y)
        {
            if (x.Frame != y.Frame
                || x.Type != y.Type
                || x.ActorId != y.ActorId)
            {
                return false;
            }
            switch (x.Type)
            {
               case  FrameStateType.StateMove:
                   return Check(x.StateMove, y.StateMove);
                case FrameStateType.StateCastSkill:
                case FrameStateType.StateCreateActor:
                case FrameStateType.StateDamage:
                    return false;
            }
            return false;
        }

        private static bool Check(StateMove x, StateMove y)
        {
            if (!x.Position.Equals(y.Position))
            {
                Log.Error($"{x.Position} != {y.Position}");
                return false;
            }
            if (x.Velocity != y.Velocity)
            {
                Log.Error($"{x.Velocity} != {y.Velocity}");
                return false;
            }

            return true;
        }
    }
}