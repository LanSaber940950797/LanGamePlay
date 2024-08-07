using System;
using ET;

namespace GameLogic.Battle
{
    public class BattleActionHelper
    {
        /// <summary>
        /// 创建行为通用接口，只有行为能力组件能调佣，其他组件不不要！！！！
        /// </summary>
        /// <param name="self"></param>
        /// <param name="action"></param>
        /// <param name="isSync"></param>
        /// <param name="isForce"></param>
        /// <typeparam name="TAbility"></typeparam>
        /// <typeparam name="TAction"></typeparam>
        /// <returns></returns>
        internal static bool TryMakeActionInner<TAbility, TAction>(TAbility self, out TAction action, bool isSync, bool isForce = false)
            where  TAbility : LSEntity, IActionAbility
            where TAction : LSEntity, IActionExecution,IAwake
        {
#if LAN_SYNC
            if (isSync 
                && !self.IsServer()
                && !isForce
                && typeof(TAction).IsAssignableFrom(typeof(IServerActionExecution))) 
            {
                //只能在服务器上创建
                Log.Error($"acotr {self.Owner.Id} 行为 {self.GetType().Name} 在客户端上只能运行快照");
                action = null;
                return false;
            }
#else
            isSync = false;
#endif
           
            if (self.Enable == false)
            {
                action = null;
            }
            else
            {
                action = self.Owner.AddChild<TAction>(true);
                action.ActionAbility = self;
                action.Creator = self.Owner;
                action.IsSync = isSync;
                action.IsSnapshot = false;
            }
            return self.Enable;
        }
        
        

       
    }
}