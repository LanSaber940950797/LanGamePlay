using System;
using System.Collections.Generic;
using ET;
using GameLogic.Battle;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using TEngine;
using TrueSync;
using UnityEngine.UI;

namespace GameLogic.Battle
{
    [FriendOf(typeof (NumericComponent))]
   
    [EntitySystemOf(typeof( NumericComponent))]
    public  static partial class NumericComponentSystem
    {
        [EntitySystem]
        public static void Awake(this NumericComponent self)
        {
            self.Event = MemoryPool.Acquire<ActorEventDispatcher>();
        }

        [EntitySystem]
        public static void Destroy(this NumericComponent self)
        {
            MemoryPool.Release(self.Event);
            self.Event = null;
        }
        //浮点数精度
        public const int Precision = 1000;
        public static float GetAsFloat(this NumericComponent self, int numericType)
        {
            return (float)self.GetByKey(numericType) / Precision;
        }
        
        public static FP GetAsFP(this NumericComponent self, int numericType)
        {
            return self.GetByKey(numericType) / Precision;
        }

        public static int GetAsInt(this NumericComponent self, int numericType)
        {
            return (int)self.GetByKey(numericType);
        }

        public static long GetAsLong(this NumericComponent self, int numericType)
        {
            return self.GetByKey(numericType);
        }

        public static void Set(this NumericComponent self, int nt, float value)
        {
            self[nt] = (long)(value * Precision);
        }
        public static void Set(this NumericComponent self, int nt, FP value)
        {
            self[nt] = (long)(value * Precision);
        }

        public static void Set(this NumericComponent self, int nt, int value)
        {
            self[nt] = value;
        }

        public static void Set(this NumericComponent self, int nt, long value)
        {
            self[nt] = value;
        }

        public static void SetNoEvent(this NumericComponent self, int numericType, long value)
        {
            self.Insert(numericType, value, false);
        }

        public static void Insert(this NumericComponent self, int numericType, long value, bool isPublicEvent = true)
        {
            long oldValue = self.GetByKey(numericType);
            if (oldValue == value)
            {
                return;
            }

            self.NumericDic[numericType] = value;

            if (numericType >= NumericType.Max)
            {
                self.Update(numericType, isPublicEvent);
                return;
            }

            if (isPublicEvent)
            {
                self.Event.SendEvent(numericType, numericType, oldValue, value);
               
            }
        }

        public static long GetByKey(this NumericComponent self, int key)
        {
            long value = 0;
            self.NumericDic.TryGetValue(key, out value);
            return value;
        }

        public static void Update(this NumericComponent self, int numericType, bool isPublicEvent)
        {
            int final = (int)numericType / 10;
            int bas = final * 10 + 1;
            int add = final * 10 + 2;
            int pct = final * 10 + 3;
            int finalAdd = final * 10 + 4;
            int finalPct = final * 10 + 5;

            // 一个数值可能会多种情况影响，比如速度,加个buff可能增加速度绝对值100，也有些buff增加10%速度，所以一个值可以由5个值进行控制其最终结果
            // final = (((base + add) * (100 + pct) / 100) + finalAdd) * (100 + finalPct) / 100;
            long result = (long)(((self.GetByKey(bas) + self.GetByKey(add)) * (100 + self.GetAsFloat(pct)) / 100f + self.GetByKey(finalAdd)) *
                (100 + self.GetAsFloat(finalPct)) / 100f);
            self.Insert(final, result, isPublicEvent);
        }
        
        #region 订阅事件
        public static void OnAttrUpdate(this Actor self, AttributeType type, Action<Entity, int, long, long> callback, Entity owner)
        {
            self.GetComponent<NumericComponent>().OnAttrUpdate(type, callback, owner);
        }

        public static void OffAttrUpdate(this Actor self, AttributeType type, Action<Entity, int, long, long> callback, Entity owner)
        {
            self.GetComponent<NumericComponent>()?.OffAttrUpdate(type, callback, owner);
        }

        public static void OffAttrUpdateAll(this Actor self, Entity owner)
        {
            self.GetComponent<NumericComponent>()?.OffAttrUpdateAll(owner);
        }
        public static void OnAttrUpdate(this NumericComponent self, AttributeType type, Action<Entity, int, long, long> callback, Entity owner)
        {
            self.Event.AddEventListener((int)type, callback, owner);
        }

        private static void OffAttrUpdate(this NumericComponent self, AttributeType type, Action<Entity, int, long, long> callback, Entity owner)
        {
            self.Event.RemoveEventListener((int)type, callback, owner);
        }
        
        private static void OffAttrUpdateAll(this NumericComponent self, Entity owner)
        {
            self.Event.RemoveAllListenerByOwner(owner);
        }
        


        #endregion

        [EntitySystem]
        public static void Deserialize(this NumericComponent self)
        {
            self.Event = MemoryPool.Acquire<ActorEventDispatcher>();
        }
    }
    
    public struct NumbericChange
    {
        public CanvasScaler.Unit Unit;
        public int NumericType;
        public long Old;
        public long New;
    }

    [ComponentOf(typeof (Actor))]
    [MemoryPackable]
    public partial class NumericComponent: LSEntity, IAwake, ISerializeToEntity,IDeserialize,IDestroy
    {
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, long> NumericDic = new Dictionary<int, long>();
        [MemoryPackIgnore]
        [BsonIgnore]
        public ActorEventDispatcher Event;
        public long this[int numericType]
        {
            get
            {
                return this.GetByKey(numericType);
            }
            set
            {
                this.Insert(numericType, value);
            }
        }
    }
}