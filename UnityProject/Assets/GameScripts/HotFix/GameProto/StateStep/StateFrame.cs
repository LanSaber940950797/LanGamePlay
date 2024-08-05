using System;
using System.Runtime.InteropServices;
using ET;
using MemoryPack;

namespace GameLogic.Battle
{
    [Serializable]
    public enum FrameStateType
    {
        StateMove = 1,
        StateCastSkill = 2,
        StateCreateActor = 3,
        StateDamage = 4,
        StateCure = 5,
        StateAddBuff = 6,
    }
    
    public interface ISnapshot
    {
        FrameStateType GetStateType();
        void WriteTo(StateFrame stateFrame);
    }
    //[MemoryPackable]
    //[StructLayout(LayoutKind.Explicit)]
    // public partial class StateFrameData
    // {
    //   
    //     public StateMove StateMove;
    //     public StateCastSkill StateCastSkill;
    //     public StateCreateActor StateCreateActor;
    //     public  StateDamage StateDamage;
    //     public StateCure StateCure;
    //     
    // }
    /// <summary>
    /// 状态帧
    /// </summary>
    [MemoryPackable]
    public partial class StateFrame
    {
        [MemoryPackOrder(0)]
        public int Frame;
        [MemoryPackOrder(1)]
        public FrameStateType Type;
        [MemoryPackOrder(2)]
        public long ActorId;
    
        [MemoryPackOrder(3)]
        public StateMove StateMove;
        [MemoryPackOrder(4)]
        public StateCastSkill StateCastSkill;
        [MemoryPackOrder(5)]
        public StateCreateActor StateCreateActor;
        [MemoryPackOrder(6)]
        public  StateDamage StateDamage;
        [MemoryPackOrder(7)]
        public StateCure StateCure;
        [MemoryPackOrder(8)]
        public  StateAddBuff StateAddBuff;
    }
    
    [MemoryPackable]
    public partial class StateMove : ISnapshot
    {
        public TrueSync.TSVector Velocity;
        public TrueSync.TSVector Position;
        public int MoveType;
        public FrameStateType GetStateType()
        {
            return FrameStateType.StateMove;
        }

        public void WriteTo(StateFrame stateFrame)
        {
            stateFrame.Type = GetStateType();
            stateFrame.StateMove = this;
        }
    }
    [MemoryPackable]
    public partial class StateCastSkill : ISnapshot
    {
        public int SkillId;
        public FrameStateType GetStateType()
        {
            return  FrameStateType.StateCastSkill;
        }

        public void WriteTo(StateFrame stateFrame)
        {
            stateFrame.Type = GetStateType();
            stateFrame.StateCastSkill = this;
        }
    }
    [MemoryPackable]
    public partial class StateCreateActor : ISnapshot
    {
        public long ActorId;
        public byte[] Data;
        public FrameStateType GetStateType()
        {
            return  FrameStateType.StateCreateActor;
        }

        public void WriteTo(StateFrame stateFrame)
        {
            stateFrame.Type = GetStateType();
            stateFrame.StateCreateActor = this;
        }
    }
    
    [MemoryPackable]
    public partial class StateAddBuff : ISnapshot
    {
        
        public long TargetId;
        public int BuffId;
        public int Layer;
        public FrameStateType GetStateType()
        {
           return  FrameStateType.StateAddBuff;
        }

        public void WriteTo(StateFrame stateFrame)
        {
            stateFrame.Type = GetStateType();
            stateFrame.StateAddBuff = this;
        }
    }
    
    [MemoryPackable]
    public partial class StateDamage : ISnapshot
    {
        public long CreatorId;
        public long TargetId;
        public long Damage;
        public FrameStateType GetStateType()
        {
            return FrameStateType.StateDamage;
        }

        public void WriteTo(StateFrame stateFrame)
        {
            stateFrame.Type = GetStateType();
            stateFrame.StateDamage = this;
        }
    }
    
    [MemoryPackable]
    public partial class StateCure : ISnapshot
    {
        public long CreatorId;
        public long TargetId;
        public long Cure;
        public FrameStateType GetStateType()
        {
            return FrameStateType.StateCure;
        }

        public void WriteTo(StateFrame stateFrame)
        {
            stateFrame.Type = GetStateType();
            stateFrame.StateCure = this;
        }
    }
}