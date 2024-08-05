using System.Collections.Generic;
using ET;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace GameLogic.Battle
{
    [MemoryPackable]
    [ComponentOf(typeof(LSWorld))]
    public partial class ActorComponent : LSEntity, IAwake, IDestroy, ISerializeToEntity,IDeserialize
    {
        [MemoryPackIgnore]
        [BsonIgnore]
        public Dictionary<long, EntityRef<Actor>> PlayerActors = new Dictionary<long, EntityRef<Actor>>();
        private EntityRef<Actor> systemActor;
        
        [MemoryPackIgnore]
        [BsonIgnore]
        public Actor SystemActor
        {
            get
            {
                return systemActor;
            }
            set
            {
                systemActor = value;
            }
        }
    }
}