using TEngine;

namespace GameLogic
{
    public class PlayerEventDefine
    {
        public static readonly int Move = RuntimeId.ToRuntimeId("PlayerEventDefine.Move");
        public static readonly int Look = RuntimeId.ToRuntimeId("PlayerEventDefine.Look");
        public static readonly int Jump = RuntimeId.ToRuntimeId("PlayerEventDefine.Jump");
        public static readonly int Sprint = RuntimeId.ToRuntimeId("PlayerEventDefine.Sprint");
        
    }
}