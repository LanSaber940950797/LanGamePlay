using ET;
using MemoryPack;

namespace GameLogic.Battle
{
    
    /// <summary>
    /// tps acotr 组件，tps actor 相关的加载创建在这里实现
    /// </summary>
    [ComponentOf(typeof(LSWorld))]
    [MemoryPackable]
    public partial class TpsActorComponent : LSEntity, IAwake,ISerializeToEntity
    {
        
    }
}