using System.Collections.Generic;
using ET;
using MemoryPack;


namespace GameLogic.Battle
{
    [ComponentOf(typeof(Actor))]
    [MemoryPackable]
    public partial class StatusComponent : LSEntity, IAwake,ISerializeToEntity
    {
       
        public StatusFlag Status = StatusFlag.None;
        public List<int> FlagBitArray = new List<int>();

        
    }
}