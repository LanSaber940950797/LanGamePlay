using ET;

namespace GameLogic.Battle
{
    /// <summary>
    /// 能力执行体，能力执行体是实际创建、执行能力表现，触发能力效果应用的地方
    /// 这里可以存一些表现执行相关的临时的状态数据
    /// </summary>
    public interface IAbilityExecute
    {
        /// <summary>
        /// 所属的能力实体
        /// </summary>
        public LSEntity AbilityEntity { get; set; }
        /// <summary>
        /// 执行体拥有者
        /// </summary>
        public Actor Owner { get; set; }

        public IAbilityEntity Ability { get;set; }
        /// 开始执行
        // public void BeginExecute();
        //
        // /// 结束执行
        // public void EndExecute();
        public void EndExecute();
        
    }

   
}