using ET;

namespace GameLogic.Battle
{
    /// <summary>
    /// 能力实体，存储着某个英雄某个能力的数据和状态
    /// </summary>
    public interface IAbilityEntity
    {
        /// <summary>
        /// 能力拥有者
        /// </summary>
        public Actor Owner { get; set; }
        /// <summary>
        /// 能力挂载的父级单位
        /// </summary>
        public Actor ParentActor { get; }
        public bool Enable { get; set; }


        /// 尝试激活能力
        // public void TryActivateAbility();
        //
        // /// 激活能力
        // public void ActivateAbility();
        //
        // /// 禁用能力
        // public void DeactivateAbility();
        //
        // /// 结束能力
        // public void EndAbility();
        //
        // /// 创建能力执行体
        // public LSEntity CreateExecution();
    }
}