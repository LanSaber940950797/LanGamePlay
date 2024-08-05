using System;
using ET;
using TEngine;
using TrueSync;

namespace GameLogic.Battle
{
    [EntitySystemOf(typeof(AttributeComponent))]
    public static partial class AttributeComponentSystem
    {
        [EntitySystem]
        public static void Awake(this AttributeComponent self)
        {
           self.Event = MemoryPool.Acquire<ActorEventDispatcher>();
        }
        [EntitySystem]
        public static void Destroy(this AttributeComponent self)
        {
            if (self.Event != null)
            {
                MemoryPool.Release(self.Event);
                self.Event = null;
            }
            self.Attributes.Clear();
        }

        #region 获取接口
        
        public static long GetHp(this AttributeComponent self)
        {
            return self.GetAttribute(AttributeType.Hp);
        }
        
        public static long GetHpMax(this AttributeComponent self)
        {
            return self.GetAttribute(AttributeType.HpMax);
        }
        
        public static long GetHpPercent(this AttributeComponent self)
        {
            return self.GetAttribute(AttributeType.Hp) * 1000 / self.GetAttribute(AttributeType.HpMax);
        }
        
        
        
        public static long GetAttribute(this AttributeComponent self, int attrId)
        {
            if (self.Attributes.TryGetValue(attrId, out var value))
            {
                return value;
            }
            return 0;
        }

        public static long GetAttribute(this AttributeComponent self, AttributeType type)
        {
            return self.GetAttribute((int)type);
        }

        public static long GetAttribute(this AttributeComponent self, AttributeType type, AttributeSubType subType)
        {
            return self.GetAttribute((int)type * 10 + (int)subType);
        }

        #endregion

        #region 设置接口

        public static AttributeType GetAttributeType(this AttributeComponent self, int attrId)
        {
            if (attrId > (int)AttributeType.OverMax)
            {
                attrId /= 10;
            }

            return (AttributeType)attrId;
        }
        
        public static AttributeSubType GetAttributeSubType(this AttributeComponent self, int attrId)
        {
            if (attrId > (int)AttributeType.OverMax)
            {
                return (AttributeSubType)(attrId % 10);
            }

            return AttributeSubType.None;
        }
        
        public static void SetAttribute(this AttributeComponent self, int attrId, long value)
        {
            var attrValue = self.GetAttribute(attrId);
            if (attrValue == value)
            {
                return;
            }

            value = self.CorrectValue(attrId, value);
            if (attrValue == value)
            {
                return;
            }
            
            var attrType = self.GetAttributeType(attrId);
            var oldValue = self.GetAttribute(attrType);
            
            if (self.Attributes.ContainsKey(attrId))
            {
                self.Attributes[attrId] = value;
            }
            else
            {
                self.Attributes.Add(attrId, value);
            }

            if (self.IsNeedUpDate(attrType))
            {
                self.UpdateAttr(attrType);
            }

            var newValue = self.GetAttribute(attrType);
            if (oldValue != newValue)
            {
                if (self.IsNotify)
                {
                    self.Event.SendEvent((int)attrType, attrType, oldValue, newValue);
                }
                self.OnCorrelation(attrType, oldValue, newValue);
            }
            
            
        }
        
        public static void SetAttribute(this AttributeComponent self, AttributeType type, AttributeSubType subType, long value)
        {
            self.SetAttribute((int)type * 10 + (int)subType, value);
        }

        public static void ModifyAttribute(this AttributeComponent self, int attrId, long value)
        {
            var curValue = self.GetAttribute(attrId);
            curValue += value;
            self.SetAttribute(attrId, curValue);
        }
        
        public static void ModifyAttribute(this AttributeComponent self, AttributeType type, AttributeSubType subType, long value)
        {
            self.ModifyAttribute((int)type * 10 + (int)subType, value);
        }
        
        public static void ModifyAttribute(this AttributeComponent self, AttributeType type, long value)
        {
            self.ModifyAttribute((int)type, value);
        }

        public static void SetAttribute(this AttributeComponent self, AttributeType type, long value)
        {
            self.SetAttribute((int)type, value);
        }
        
        private static bool IsNeedUpDate(this AttributeComponent self, AttributeType type)
        {
            return type > AttributeType.BattleAttrMin && type < AttributeType.BattleAttrMax;
        }
        
        private  static void UpdateAttr(this AttributeComponent self, AttributeType type)
        {
            int final = (int)type;
            int bas = final * 10 + 1; 
            int add = final * 10 + 2;
            int pct = final * 10 + 3;
            int finalAdd = final * 10 + 4;
            int finalPct = final * 10 + 5;
            // 一个数值可能会多种情况影响，比如速度,加个buff可能增加速度绝对值100，也有些buff增加10%速度，所以一个值可以由5个值进行控制其最终结果
            // final = (((base + add) * (100 + pct) / 100) + finalAdd) * (100 + finalPct) / 100;

            var basAttr = self.GetAttribute(bas);
            var addAttr = self.GetAttribute(add);
            var pctAttr = self.GetAttribute(pct);
            var finalAddAttr = self.GetAttribute(finalAdd);
            var finalPctAttr = self.GetAttribute(finalPct);
            
            
            long newVar =  ((basAttr + addAttr) * (1000 + pctAttr) / 1000 + finalAddAttr) * (1000 + finalPctAttr) / 1000;
            newVar = self.CorrectValue(final, newVar);
            self.Attributes[final] = newVar;
        }

        #endregion

        #region 其他

        private  static void OnCorrelation(this AttributeComponent self, AttributeType type, long oldValue, long newValue)
        {
            
            var changeValue = newValue - oldValue;
            if (type == AttributeType.HpMax)
            {
                if (changeValue < 0)
                {
                    var hp = self.GetAttribute(AttributeType.Hp);
                    if (hp > newValue)
                    {
                        self.SetAttribute(AttributeType.Hp, newValue);
                    }
                }
                else
                {
                    self.ModifyAttribute(AttributeType.Hp, changeValue);
                }
            }
            else if (type == AttributeType.MpMax)
            {
                if (changeValue < 0)
                {
                    var mp = self.GetAttribute(AttributeType.Mp);
                    if (mp > newValue)
                    {
                        self.SetAttribute(AttributeType.Mp, newValue);
                    }
                }
                else
                {
                    self.ModifyAttribute(AttributeType.Mp, changeValue);
                }
            }
        }
        private  static long CorrectValue(this AttributeComponent self, int attributeId, long value)
        {
            if (attributeId > (int)AttributeType.OverMax)
            {
                //中间值不管
                return value;
            }
            
            
            AttributeType type = self.GetAttributeType(attributeId);
            AttributeSubType subType = self.GetAttributeSubType(attributeId);
            if (type == AttributeType.Hp)
            {
                if (value < 0)
                {
                    value = 0;
                }

                long maxHp = self.GetAttribute(AttributeType.HpMax);
                if (value > maxHp)
                {
                    value = maxHp;
                }
            }
            else if(type == AttributeType.Mp)
            {
                if (value < 0)
                {
                    value = 0;
                }

                long maxMp = self.GetAttribute(AttributeType.MpMax);
                if (value > maxMp)
                {
                    value = maxMp;
                }
            }

            return value;
        }


        
        
        #endregion

        #region 订阅事件
        public static void OnAttrUpdate(this Actor self, AttributeType type, Action<Entity, AttributeType, long, long> callback, Entity owner)
        {
            self.GetComponent<AttributeComponent>().OnAttrUpdate(type, callback, owner);
        }

        public static void OffAttrUpdate(this Actor self, AttributeType type, Action<Entity, AttributeType, long, long> callback, Entity owner)
        {
            self.GetComponent<AttributeComponent>()?.OffAttrUpdate(type, callback, owner);
        }

        public static void OffAttrUpdateAll(this Actor self, Entity owner)
        {
            self.GetComponent<AttributeComponent>()?.OffAttrUpdateAll(owner);
        }
        public static void OnAttrUpdate(this AttributeComponent self, AttributeType type, Action<Entity, AttributeType, long, long> callback, Entity owner)
        {
            self.Event.AddEventListener((int)type, callback, owner);
        }

        private static void OffAttrUpdate(this AttributeComponent self, AttributeType type, Action<Entity, AttributeType, long, long> callback, Entity owner)
        {
            self.Event.RemoveEventListener((int)type, callback, owner);
        }
        
        private static void OffAttrUpdateAll(this AttributeComponent self, Entity owner)
        {
            self.Event.RemoveAllListenerByOwner(owner);
        }
        


        #endregion

        [EntitySystem]
        public static void Deserialize(this AttributeComponent self)
        {
            self.Event = MemoryPool.Acquire<ActorEventDispatcher>();
        }
    }
}