using System;
using System.Collections.Generic;
using ET;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace GameLogic.Battle
{
    public class AttributeUpdateEvent
    {
        public AttributeType attributeType;
        public long newValue;
        public long oldValue;
    }
    /// <summary>
    /// 属性组件
    /// </summary>
    [ComponentOf(typeof(Actor))]
    [MemoryPackable]
    public partial class AttributeComponent : LSEntity, IAwake,IDestroy,ISerializeToEntity,IDeserialize
    {
        //public AttributeManager AttributeManager { get; set; }
        
        public Dictionary<int, long> Attributes = new Dictionary<int, long>();
        public bool IsNotify = true;
        
        [MemoryPackIgnore]
        [BsonIgnore]
        public ActorEventDispatcher Event;
        
    }
}