using ET;
using Rvo2;

namespace GameLogic.Battle
{
    [ComponentOf(typeof(LSWorld))]
    public class RvoWorldComponent: LSEntity, IAwake,ILSLateUpdate
    {
        //rov 用的是浮点数，如果需要服务器验证，则修改成定点数
        public Simulator Simulator;
    }
    
    [EntitySystemOf(typeof(RvoWorldComponent))]
    public static partial class RvoWorldComponentSystem
    {
        [EntitySystem]
        public static void Awake(this RvoWorldComponent self)
        {
            self.Simulator = new Simulator();
            self.Simulator.SetAgentDefaults(2, 10, 5, 0.05f, 0.3f, 
                0, new Vector2(0,0));
        }

        [LSEntitySystem]
        public static void LSLateUpdate(this RvoWorldComponent self)
        {
            self.Simulator.Run((float)LSConstValue.UpdateInterval / 1000);
        }
        
    }
}