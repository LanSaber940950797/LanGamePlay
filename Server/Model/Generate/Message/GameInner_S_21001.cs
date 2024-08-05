using MemoryPack;
using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 请求匹配
    /// </summary>
    [MemoryPackable]
    [Message(GameInner.G2Match_Match)]
    [ResponseType(nameof(Match2G_Match))]
    public partial class G2Match_Match : MessageObject, IRequest
    {
        public static G2Match_Match Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(G2Match_Match), isFromPool) as G2Match_Match;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public long Id { get; set; }

        [MemoryPackOrder(2)]
        public string RoomId { get; set; }

        [MemoryPackOrder(3)]
        public bool IsCreate { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Id = default;
            this.RoomId = default;
            this.IsCreate = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(GameInner.Match2G_Match)]
    public partial class Match2G_Match : MessageObject, IResponse
    {
        public static Match2G_Match Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Match2G_Match), isFromPool) as Match2G_Match;
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
    [Message(GameInner.Match2Map_GetRoom)]
    [ResponseType(nameof(Map2Match_GetRoom))]
    public partial class Match2Map_GetRoom : MessageObject, IRequest
    {
        public static Match2Map_GetRoom Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Match2Map_GetRoom), isFromPool) as Match2Map_GetRoom;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public List<long> PlayerIds { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.PlayerIds.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(GameInner.Map2Match_GetRoom)]
    public partial class Map2Match_GetRoom : MessageObject, IResponse
    {
        public static Map2Match_GetRoom Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Map2Match_GetRoom), isFromPool) as Map2Match_GetRoom;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        /// <summary>
        /// 房间的ActorId
        /// </summary>
        [MemoryPackOrder(3)]
        public ActorId ActorId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.ActorId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(GameInner.RoomManager2Room_Init)]
    [ResponseType(nameof(Room2RoomManager_Init))]
    public partial class RoomManager2Room_Init : MessageObject, IRequest
    {
        public static RoomManager2Room_Init Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(RoomManager2Room_Init), isFromPool) as RoomManager2Room_Init;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public long PlayerId { get; set; }

        [MemoryPackOrder(2)]
        public long RoomId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.PlayerId = default;
            this.RoomId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(GameInner.Room2RoomManager_Init)]
    public partial class Room2RoomManager_Init : MessageObject, IResponse
    {
        public static Room2RoomManager_Init Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Room2RoomManager_Init), isFromPool) as Room2RoomManager_Init;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public ActorId ActorId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.ActorId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    /// <summary>
    /// 请求创建房间
    /// </summary>
    [MemoryPackable]
    [Message(GameInner.G2Match_CreateRoom)]
    [ResponseType(nameof(Match2G_CreateRoom))]
    public partial class G2Match_CreateRoom : MessageObject, IRequest
    {
        public static G2Match_CreateRoom Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(G2Match_CreateRoom), isFromPool) as G2Match_CreateRoom;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public long Id { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Id = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(GameInner.Match2G_CreateRoom)]
    public partial class Match2G_CreateRoom : MessageObject, IResponse
    {
        public static Match2G_CreateRoom Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Match2G_CreateRoom), isFromPool) as Match2G_CreateRoom;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public ActorId ActorId { get; set; }

        [MemoryPackOrder(4)]
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
            this.ActorId = default;
            this.RoomId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    /// <summary>
    /// 请求加入房间
    /// </summary>
    [MemoryPackable]
    [Message(GameInner.G2Match_JoinRoom)]
    [ResponseType(nameof(Match2G_JoinRoom))]
    public partial class G2Match_JoinRoom : MessageObject, IRequest
    {
        public static G2Match_JoinRoom Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(G2Match_JoinRoom), isFromPool) as G2Match_JoinRoom;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public long Id { get; set; }

        [MemoryPackOrder(2)]
        public long RoomId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Id = default;
            this.RoomId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(GameInner.Match2G_JoinRoom)]
    public partial class Match2G_JoinRoom : MessageObject, IResponse
    {
        public static Match2G_JoinRoom Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Match2G_JoinRoom), isFromPool) as Match2G_JoinRoom;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public ActorId ActorId { get; set; }

        [MemoryPackOrder(4)]
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
            this.ActorId = default;
            this.RoomId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(GameInner.G2Match_GetRoomInfo)]
    [ResponseType(nameof(Match2G_GetRoomInfo))]
    public partial class G2Match_GetRoomInfo : MessageObject, IRequest
    {
        public static G2Match_GetRoomInfo Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(G2Match_GetRoomInfo), isFromPool) as G2Match_GetRoomInfo;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public long Id { get; set; }

        [MemoryPackOrder(2)]
        public long RoomId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Id = default;
            this.RoomId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(GameInner.Match2G_GetRoomInfo)]
    public partial class Match2G_GetRoomInfo : MessageObject, IResponse
    {
        public static Match2G_GetRoomInfo Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Match2G_GetRoomInfo), isFromPool) as Match2G_GetRoomInfo;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public long RoomId { get; set; }

        [MemoryPackOrder(4)]
        public ActorId ActorId { get; set; }

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
            this.ActorId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    public static class GameInner
    {
        public const ushort G2Match_Match = 21002;
        public const ushort Match2G_Match = 21003;
        public const ushort Match2Map_GetRoom = 21004;
        public const ushort Map2Match_GetRoom = 21005;
        public const ushort RoomManager2Room_Init = 21006;
        public const ushort Room2RoomManager_Init = 21007;
        public const ushort G2Match_CreateRoom = 21008;
        public const ushort Match2G_CreateRoom = 21009;
        public const ushort G2Match_JoinRoom = 21010;
        public const ushort Match2G_JoinRoom = 21011;
        public const ushort G2Match_GetRoomInfo = 21012;
        public const ushort Match2G_GetRoomInfo = 21013;
    }
}