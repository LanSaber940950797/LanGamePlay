// using System.Collections.Generic;
// using ET;
// using MemoryPack;
// using MongoDB.Bson.Serialization.Attributes;
//
// namespace GameLogic.Battle
// {
//     [MemoryPackable]
//     [ComponentOf(typeof(LSWorld))]
//     public partial class TestActorComponent : Entity, IAwake,ISerializeToEntity
//     {
//         [MemoryPackConstructor]
//         public TestActorComponent()
//         {
//         }
//         [MemoryPackInclude]
//         public int test;
//     }
// }