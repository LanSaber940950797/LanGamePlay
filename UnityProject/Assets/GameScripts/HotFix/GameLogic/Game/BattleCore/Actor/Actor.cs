using ET;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using TrueSync;

namespace GameLogic.Battle
{
    [MemoryPackable]
    [ChildOf(typeof(ActorComponent))]
    public partial class Actor : LSEntity,IAwake<ActorCreateInfo>,ISerializeToEntity,IDeserialize
    {
       
        public ActorType ActorType;
       
        public SideType SideType;
       
        public int DescId;
      
        public long PlayerId;
    }
}