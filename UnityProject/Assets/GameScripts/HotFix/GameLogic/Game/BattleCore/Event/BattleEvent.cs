using TEngine;

namespace GameLogic.Battle
{
    public class BattleEvent
    {
        
        public static readonly int ActorStatusChange = RuntimeId.ToRuntimeId("BattleEvent.ActorStatusChange");
        public static readonly int ActorDead = RuntimeId.ToRuntimeId("BattleEvent.ActorDead");
        public static readonly int ActorAttrUpdate = RuntimeId.ToRuntimeId("BattleEvent.ActorAttrUpdate");
        public static readonly int ForceChangePosition = RuntimeId.ToRuntimeId("BattleEvent.ForceChangePosition");
        
        //这个事件是逻辑层创建
        public static readonly int ActorCreate = RuntimeId.ToRuntimeId("BattleEvent.ActorCreate");
        //这个事件通知渲染层，有些actor是数据加载进来的，不走逻辑层ActorCreate
        public static readonly int ActorCreateView = RuntimeId.ToRuntimeId("BattleEvent.ActorCreateView");
    }
}