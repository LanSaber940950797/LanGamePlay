using System;
using System.Collections.Generic;
using ET;

namespace GameLogic.Battle
{
    /// <summary>
    /// 行动点，一次战斗行动<see cref="IActionExecution"/>会触发战斗实体一系列的行动点<see cref="ActionPoint"/>
    /// </summary>
   
    [ComponentOf(typeof(Actor))]
    public class ActionPointComponent : LSEntity, IAwake, IDestroy
    {
        public ActorEventDispatcher EventDispatcher { get; set; }
    }
}