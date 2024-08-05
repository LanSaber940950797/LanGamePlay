using System.Collections.Generic;

namespace ET
{
    public class LogMsg: Singleton<LogMsg>, ISingletonAwake
    {
        private readonly HashSet<ushort> ignore = new()
        {
            10017, 
            10018, 
            // OuterMessage.C2G_Benchmark, 
            // OuterMessage.G2C_Benchmark,
        };

        public void Awake()
        {
        }

        public void Debug(Fiber fiber, object msg)
        {
            ushort opcode = OpcodeType.Instance.GetOpcode(msg.GetType());
            if (this.ignore.Contains(opcode))
            {
                return;
            }
            fiber.Log.Debug(msg.ToString());
        }
        
        public void Info(Fiber fiber, object msg)
        {
            ushort opcode = OpcodeType.Instance.GetOpcode(msg.GetType());
            if (this.ignore.Contains(opcode))
            {
                return;
            }
            fiber.Log.Info(msg.ToString());
        }
    }
}