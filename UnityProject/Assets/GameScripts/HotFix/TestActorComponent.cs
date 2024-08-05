using System.Collections.Generic;
using ET;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace GameLogic
{
    [MemoryPackable]
    public partial class TestActorComponent : Entity, IAwake,ISerializeToEntity
    {
        public int Value = 3;
    }
}