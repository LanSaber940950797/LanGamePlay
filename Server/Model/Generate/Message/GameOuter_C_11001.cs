using MemoryPack;
using System.Collections.Generic;

namespace ET
{
    [MemoryPackable]
    [Message(GameOuter.C2G_Match)]
    [ResponseType(nameof(G2C_Match))]
    public partial class C2G_Match : MessageObject, ISessionRequest
    {
        public static C2G_Match Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2G_Match), isFromPool) as C2G_Match;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public string RoomId { get; set; }

        [MemoryPackOrder(2)]
        public bool IsCreate { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.RoomId = default;
            this.IsCreate = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(GameOuter.G2C_Match)]
    public partial class G2C_Match : MessageObject, ISessionResponse
    {
        public static G2C_Match Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(G2C_Match), isFromPool) as G2C_Match;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(GameOuter.A2RoomInner_Message)]
    public partial class A2RoomInner_Message : MessageObject, IRoomMessage
    {
        public static A2RoomInner_Message Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(A2RoomInner_Message), isFromPool) as A2RoomInner_Message;
        }

        [MemoryPackOrder(0)]
        public long PlayerId { get; set; }

        [MemoryPackOrder(1)]
        public long TargetPlayerId { get; set; }

        [MemoryPackOrder(2)]
        public ushort OpCode { get; set; }

        [MemoryPackOrder(3)]
        public long RoomId { get; set; }

        [MemoryPackOrder(4)]
        public string Name { get; set; }

        [MemoryPackOrder(5)]
        public byte[] MessageObj { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.PlayerId = default;
            this.TargetPlayerId = default;
            this.OpCode = default;
            this.RoomId = default;
            this.Name = default;
            this.MessageObj = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    /// <summary>
    /// 创建房间
    /// </summary>
    [MemoryPackable]
    [Message(GameOuter.C2G_CreateRoom)]
    [ResponseType(nameof(G2C_CreateRoom))]
    public partial class C2G_CreateRoom : MessageObject, ISessionRequest
    {
        public static C2G_CreateRoom Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2G_CreateRoom), isFromPool) as C2G_CreateRoom;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(GameOuter.G2C_CreateRoom)]
    public partial class G2C_CreateRoom : MessageObject, ISessionResponse
    {
        public static G2C_CreateRoom Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(G2C_CreateRoom), isFromPool) as G2C_CreateRoom;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public long RoomId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.RoomId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    /// <summary>
    /// 创建房间
    /// </summary>
    [MemoryPackable]
    [Message(GameOuter.C2G_JoinRoom)]
    [ResponseType(nameof(G2C_JoinRoom))]
    public partial class C2G_JoinRoom : MessageObject, ISessionRequest
    {
        public static C2G_JoinRoom Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2G_JoinRoom), isFromPool) as C2G_JoinRoom;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public long RoomId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.RoomId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(GameOuter.G2C_JoinRoom)]
    public partial class G2C_JoinRoom : MessageObject, ISessionResponse
    {
        public static G2C_JoinRoom Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(G2C_JoinRoom), isFromPool) as G2C_JoinRoom;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public long RoomId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.RoomId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(GameOuter.C2Room_JoinRoom)]
    [ResponseType(nameof(Room2C_JoinRoom))]
    public partial class C2Room_JoinRoom : MessageObject, IRoomRequest
    {
        public static C2Room_JoinRoom Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2Room_JoinRoom), isFromPool) as C2Room_JoinRoom;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public long PlayerId { get; set; }

        [MemoryPackOrder(2)]
        public long TargetPlayerId { get; set; }

        [MemoryPackOrder(3)]
        public long RoomId { get; set; }

        [MemoryPackOrder(4)]
        public string Name { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.PlayerId = default;
            this.TargetPlayerId = default;
            this.RoomId = default;
            this.Name = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(GameOuter.RoomPlayerInfo)]
    public partial class RoomPlayerInfo : MessageObject
    {
        public static RoomPlayerInfo Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(RoomPlayerInfo), isFromPool) as RoomPlayerInfo;
        }

        [MemoryPackOrder(0)]
        public long PlayerId { get; set; }

        [MemoryPackOrder(1)]
        public string Name { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.PlayerId = default;
            this.Name = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(GameOuter.Room2C_JoinRoom)]
    public partial class Room2C_JoinRoom : MessageObject, IRoomResponse
    {
        public static Room2C_JoinRoom Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Room2C_JoinRoom), isFromPool) as Room2C_JoinRoom;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public long PlayerId { get; set; }

        [MemoryPackOrder(4)]
        public long TargetPlayerId { get; set; }

        [MemoryPackOrder(5)]
        public List<RoomPlayerInfo> Players { get; set; } = new();

        [MemoryPackOrder(6)]
        public byte[] WolrdData { get; set; }

        [MemoryPackOrder(7)]
        public long StartTime { get; set; }

        [MemoryPackOrder(8)]
        public int Frame { get; set; }

        /// <summary>
        /// string WoldData = 10; //先用字符串替换，MemoryPack有问题
        /// </summary>
        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.PlayerId = default;
            this.TargetPlayerId = default;
            this.Players.Clear();
            this.WolrdData = default;
            this.StartTime = default;
            this.Frame = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(GameOuter.C2Room_TestChangeTxt)]
    public partial class C2Room_TestChangeTxt : MessageObject, IRoomMessage
    {
        public static C2Room_TestChangeTxt Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2Room_TestChangeTxt), isFromPool) as C2Room_TestChangeTxt;
        }

        [MemoryPackOrder(0)]
        public long PlayerId { get; set; }

        [MemoryPackOrder(1)]
        public long TargetPlayerId { get; set; }

        [MemoryPackOrder(2)]
        public string Txt { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.PlayerId = default;
            this.TargetPlayerId = default;
            this.Txt = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(GameOuter.Room2C_TestChangeTxtNotify)]
    public partial class Room2C_TestChangeTxtNotify : MessageObject, IRoomMessage
    {
        public static Room2C_TestChangeTxtNotify Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Room2C_TestChangeTxtNotify), isFromPool) as Room2C_TestChangeTxtNotify;
        }

        [MemoryPackOrder(0)]
        public long PlayerId { get; set; }

        [MemoryPackOrder(1)]
        public long TargetPlayerId { get; set; }

        [MemoryPackOrder(2)]
        public long Id { get; set; }

        [MemoryPackOrder(3)]
        public string Txt { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.PlayerId = default;
            this.TargetPlayerId = default;
            this.Id = default;
            this.Txt = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(GameOuter.C2Room_Ping)]
    [ResponseType(nameof(Room2C_Ping))]
    public partial class C2Room_Ping : MessageObject, IRoomRequest
    {
        public static C2Room_Ping Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2Room_Ping), isFromPool) as C2Room_Ping;
        }

        [MemoryPackOrder(0)]
        public long PlayerId { get; set; }

        [MemoryPackOrder(1)]
        public long TargetPlayerId { get; set; }

        [MemoryPackOrder(2)]
        public int RpcId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.PlayerId = default;
            this.TargetPlayerId = default;
            this.RpcId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(GameOuter.Room2C_Ping)]
    public partial class Room2C_Ping : MessageObject, IRoomResponse
    {
        public static Room2C_Ping Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Room2C_Ping), isFromPool) as Room2C_Ping;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public long Time { get; set; }

        [MemoryPackOrder(4)]
        public long PlayerId { get; set; }

        [MemoryPackOrder(5)]
        public long TargetPlayerId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.Time = default;
            this.PlayerId = default;
            this.TargetPlayerId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    public static class GameOuter
    {
        public const ushort C2G_Match = 11002;
        public const ushort G2C_Match = 11003;
        public const ushort A2RoomInner_Message = 11004;
        public const ushort C2G_CreateRoom = 11005;
        public const ushort G2C_CreateRoom = 11006;
        public const ushort C2G_JoinRoom = 11007;
        public const ushort G2C_JoinRoom = 11008;
        public const ushort C2Room_JoinRoom = 11009;
        public const ushort RoomPlayerInfo = 11010;
        public const ushort Room2C_JoinRoom = 11011;
        public const ushort C2Room_TestChangeTxt = 11012;
        public const ushort Room2C_TestChangeTxtNotify = 11013;
        public const ushort C2Room_Ping = 11014;
        public const ushort Room2C_Ping = 11015;
    }
}