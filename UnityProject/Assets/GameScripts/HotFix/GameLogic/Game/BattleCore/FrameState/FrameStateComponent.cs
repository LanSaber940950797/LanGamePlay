using System.Collections.Generic;
using ET;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace GameLogic.Battle
{
    [ComponentOf(typeof(LSWorld))]
    [MemoryPackable]
    public partial class FrameStateComponent : LSEntity,IAwake,ISerializeToEntity,IDeserialize
    {
        [MemoryPackIgnore]
        [BsonIgnore]
        public List<StateFrame> InputFrames = new List<StateFrame>();
    }
}