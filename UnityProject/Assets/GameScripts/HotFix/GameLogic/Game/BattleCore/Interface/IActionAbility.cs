namespace GameLogic.Battle
{
    /// <summary>
    /// 战斗行动能力
    /// </summary>
    public interface IActionAbility
    {
        public Actor Owner { get; set; }
        public bool Enable { get; set; }
        
        //public bool TryMakeAction<T>(out T action) where T : IActionExecution;
    }
}