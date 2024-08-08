using ET;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using TrueSync;
using Unity.Mathematics;
using UnityEngine;

namespace GameLogic.Battle
{
    [ComponentOf(typeof(Actor))]
    [MemoryPackable]
    public partial class TransformComponent : LSEntity, IAwake,ISerializeToEntity
    {
        //坐标
        public TSVector Position
        {
            get;
            set;
        }

        public TSVector forward;
        
        [MemoryPackIgnore]
        [BsonIgnore]
        public TSVector Forward //朝向
        {
            get
            { return this.Rotation * TSVector.forward; }
            set => this.Rotation = TSQuaternion.LookRotation(value, TSVector.up);
        }

        public TSQuaternion Rotation //角度
        {
            get;
            set;
        }

        public FP Height; //高度

        public TSVector CenterPosition
        {
            get
            {
                return this.Position + TSVector.up * this.Height;
            }
        }
        public ERoleShape Shape = ERoleShape.Rect;
        public FP Radius;
    }
}