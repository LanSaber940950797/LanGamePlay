using System.Collections.Generic;
using ET;

namespace GameLogic.Battle
{
    [EntitySystemOf(typeof(StatusComponent))]
    public static partial class StatusComponentSystem
    {
        public static readonly List<int> EnumBitArray = new List<int>();
        public static void Awake(this StatusComponent self)
        {
            self.Status = StatusFlag.None;
            InitEnumBitArray();
            self.FlagBitArray.Clear();
            foreach (var _ in EnumBitArray)
            {
                self.FlagBitArray.Add(0);
            }
        }

        private static void  InitEnumBitArray()
        {
            if(EnumBitArray.Count == 0)
            {
                for (int i = 0; i < 32; i++)
                {
                    EnumBitArray.Add(1 << i);
                }
            }
        }

        public static void AddStatus(this StatusComponent self, StatusFlag _status)
        {
            for (int i = 0; i < EnumBitArray.Count; i++)
            {
                var bit = EnumBitArray[i];
                if ((bit & (int)_status) > 0)
                {
                    self.FlagBitArray[i]++;
                }
            }
            
            self.Status |= _status;
            self.Parent.SendEvent(BattleEvent.ActorStatusChange, self);
        }
        
        public static void RemoveStatus(this StatusComponent self, StatusFlag _status)
        {
            bool change = false;
            for (int i = 0; i < EnumBitArray.Count; i++)
            {
                var bit = EnumBitArray[i];
                if ((bit & (int)_status) > 0)
                {
                    self.FlagBitArray[i]--;
                    if(self.FlagBitArray[i] <= 0)
                    {
                        self.Status &= ~(StatusFlag)bit;
                        self.FlagBitArray[i] = 0;
                        change = true;
                    }
                }
            }
            
            if(change)
            {
                self.Parent.SendEvent(BattleEvent.ActorStatusChange, self);
            }
        }
        
        public static bool HasStatus(this StatusComponent self, StatusFlag _status)
        {
            return (self.Status & _status) > 0;
        }


        #region  常用接口

        public static bool IsCanMove(this StatusComponent self)
        {
            return (self.Status & StatusFlag.Rooted) == 0;
        }

        public static bool IsCanSpell(this StatusComponent self)
        {
            return !self.HasStatus(StatusFlag.Slienced);
        }
        #endregion

    }
}