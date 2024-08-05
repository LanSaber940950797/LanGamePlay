// using System;
// using System.Collections.Generic;
// using ET;
// using MemoryPack;
// using TEngine;
//
//
// namespace GameLogic.Battle
// {
//    
//     public class AttributeManager
//     {
//         
//         private Dictionary<int, long> _attributes = new Dictionary<int, long>();
//         public bool IsNotify = true;
//         private ActorEventDispatcher _event = MemoryPool.Acquire<ActorEventDispatcher>();
//         #region 获取接口
//
//         public long GetAttribute(int attrId)
//         {
//             if (_attributes.TryGetValue(attrId, out var value))
//             {
//                 return value;
//             }
//             return 0;
//         }
//
//         public long GetAttribute(AttributeType type)
//         {
//             return GetAttribute((int)type);
//         }
//
//         public long GetAttribute(AttributeType type, AttributeSubType subType)
//         {
//             return GetAttribute((int)type * 10 + (int)subType);
//         }
//
//         #endregion
//
//         #region 设置接口
//
//         public AttributeType GetAttributeType(int attrId)
//         {
//             if (attrId > (int)AttributeType.OverMax)
//             {
//                 attrId /= 10;
//             }
//
//             return (AttributeType)attrId;
//         }
//         
//         public AttributeSubType GetAttributeSubType(int attrId)
//         {
//             if (attrId > (int)AttributeType.OverMax)
//             {
//                 return (AttributeSubType)(attrId % 10);
//             }
//
//             return AttributeSubType.None;
//         }
//         
//         public void SetAttribute(int attrId, long value)
//         {
//             var attrValue = GetAttribute(attrId);
//             if (attrValue == value)
//             {
//                 return;
//             }
//
//             value = CorrectValue(attrId, value);
//             if (attrValue == value)
//             {
//                 return;
//             }
//             
//             var attrType = GetAttributeType(attrId);
//             var oldValue = GetAttribute(attrType);
//             
//             if (_attributes.ContainsKey(attrId))
//             {
//                 _attributes[attrId] = value;
//             }
//             else
//             {
//                 _attributes.Add(attrId, value);
//             }
//
//             if (IsNeedUpDate(attrType))
//             {
//                 UpdateAttr(attrType);
//             }
//
//             var newValue = GetAttribute(attrType);
//             if (oldValue != newValue)
//             {
//                 if (IsNotify)
//                 {
//                     _event.SendEvent((int)attrType, attrType, oldValue, newValue);
//                 }
//                 OnCorrelation(attrType, oldValue, newValue);
//             }
//             
//             
//         }
//         
//         public void SetAttribute(AttributeType type, AttributeSubType subType, long value)
//         {
//             SetAttribute((int)type * 10 + (int)subType, value);
//         }
//
//         public void ModifyAttribute(int attrId, long value)
//         {
//             var curValue = GetAttribute(attrId);
//             curValue += value;
//             SetAttribute(attrId, curValue);
//         }
//         
//         public void ModifyAttribute(AttributeType type, AttributeSubType subType, long value)
//         {
//             ModifyAttribute((int)type * 10 + (int)subType, value);
//         }
//         
//         public void ModifyAttribute(AttributeType type, long value)
//         {
//             ModifyAttribute((int)type, value);
//         }
//
//         public void SetAttribute(AttributeType type, long value)
//         {
//             SetAttribute((int)type, value);
//         }
//         
//         private bool IsNeedUpDate(AttributeType type)
//         {
//             return type > AttributeType.BattleAttrMin && type < AttributeType.BattleAttrMax;
//         }
//         
//         private void UpdateAttr(AttributeType type)
//         {
//             int final = (int)type;
//             int bas = final * 10 + 1; 
//             int add = final * 10 + 2;
//             int pct = final * 10 + 3;
//             int finalAdd = final * 10 + 4;
//             int finalPct = final * 10 + 5;
//             // 一个数值可能会多种情况影响，比如速度,加个buff可能增加速度绝对值100，也有些buff增加10%速度，所以一个值可以由5个值进行控制其最终结果
//             // final = (((base + add) * (100 + pct) / 100) + finalAdd) * (100 + finalPct) / 100;
//             long newVar =  ((GetAttribute(bas) + GetAttribute(add)) * (1000 + GetAttribute(pct)) / 1000 + GetAttribute(finalAdd)) * (1000 + GetAttribute(finalPct)) / 1000;
//             newVar = CorrectValue(final, newVar);
//             _attributes[final] = newVar;
//         }
//
//         #endregion
//
//         #region 其他
//
//         private void OnCorrelation(AttributeType type, long oldValue, long newValue)
//         {
//             
//             var changeValue = newValue - oldValue;
//             if (type == AttributeType.HpMax)
//             {
//                 if (changeValue < 0)
//                 {
//                     var hp = GetAttribute(AttributeType.Hp);
//                     if (hp > newValue)
//                     {
//                         SetAttribute(AttributeType.Hp, newValue);
//                     }
//                 }
//                 else
//                 {
//                     ModifyAttribute(AttributeType.Hp, changeValue);
//                 }
//             }
//             else if (type == AttributeType.MpMax)
//             {
//                 if (changeValue < 0)
//                 {
//                     var mp = GetAttribute(AttributeType.Mp);
//                     if (mp > newValue)
//                     {
//                         SetAttribute(AttributeType.Mp, newValue);
//                     }
//                 }
//                 else
//                 {
//                     ModifyAttribute(AttributeType.Mp, changeValue);
//                 }
//             }
//         }
//         private long CorrectValue(int attributeId, long value)
//         {
//             if (attributeId > (int)AttributeType.OverMax)
//             {
//                 //中间值不管
//                 return value;
//             }
//             
//             
//             AttributeType type = GetAttributeType(attributeId);
//             AttributeSubType subType = GetAttributeSubType(attributeId);
//             if (type == AttributeType.Hp)
//             {
//                 if (value < 0)
//                 {
//                     value = 0;
//                 }
//
//                 long maxHp = GetAttribute(AttributeType.HpMax);
//                 if (value > maxHp)
//                 {
//                     value = maxHp;
//                 }
//             }
//             else if(type == AttributeType.Mp)
//             {
//                 if (value < 0)
//                 {
//                     value = 0;
//                 }
//
//                 long maxMp = GetAttribute(AttributeType.MpMax);
//                 if (value > maxMp)
//                 {
//                     value = maxMp;
//                 }
//             }
//
//             return value;
//         }
//
//         public void ClearAttrs()
//         {
//             _attributes.Clear();
//         }
//         
//         public void Clear()
//         {
//             if (_event != null)
//             {
//                 MemoryPool.Release(_event);
//                 _event = null;
//             }
//
//             ClearAttrs();
//         }
//         #endregion
//
//         public void Subscribe(AttributeType type, Action<Entity, AttributeType, long, long> callback, Entity owner)
//         {
//             _event.AddEventListener((int)type, callback, owner);
//         }
//
//         public void UnSubscribe(AttributeType type, Action<Entity, AttributeType, long, long> callback, Entity owner)
//         {
//             _event.RemoveEventListener((int)type, callback, owner);
//         }
//         
//         public void UnSubscribeAll(Entity owner)
//         {
//             _event.RemoveAllListenerByOwner(owner);
//         }
//         
//         public void CopyFrom(AttributeManager other)
//         {
//             _attributes.Clear();
//             foreach (var kv in other._attributes)
//             {
//                 _attributes.Add(kv.Key, kv.Value);
//             }
//         }
//     }
// }