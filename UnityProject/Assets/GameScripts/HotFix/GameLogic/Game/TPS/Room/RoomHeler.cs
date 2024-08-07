using System;
using Cysharp.Threading.Tasks;
using ET;
using GameLogic.Battle;
using MemoryPack;
using TEngine;
using UnityEngine;
using UnityEngine.SceneManagement;
using Log = ET.Log;

namespace GameLogic
{
    public static class RoomHeler
    {
        
        public static A2RoomInner_Message SerializeMessage(IRoomMessage message)
        {
            var roomMessage = A2RoomInner_Message.Create();
            roomMessage.OpCode = OpcodeType.Instance.GetOpcode(message.GetType());
            roomMessage.Name = message.GetType().Name;
            roomMessage.MessageObj = MessageSerializeHelper.Serialize(message as MessageObject);
            //message.PlayerId = GameHeler.Root().GetComponent<Player>().PlayerId;
            return roomMessage; 
        }

        public static MessageObject DeserializeMessage(A2RoomInner_Message message) 
        {
            return MessageSerializeHelper.Deserialize(OpcodeType.Instance.GetType(message.OpCode), message.MessageObj, 0, message.MessageObj.Length);
        }
        
        public static async ETTask CreateRoom(string name)
        {
            var root = GameHeler.Root();
            await LoginHelper.Login(root, name, "123456");

            C2G_CreateRoom c2GCreateRoom = C2G_CreateRoom.Create();
           
            G2C_CreateRoom g2CCreateRoom = await root.GetComponent<ClientSenderComponent>().Call(c2GCreateRoom) as G2C_CreateRoom;
            if (g2CCreateRoom.Error != 0)
            {
                Log.Error($"创建房间失败:{g2CCreateRoom.Message}");
                return;
            }
            
            //todo 执行进入场景流程
            root.RemoveComponent<Room>();
            var room = root.AddComponentWithId<Room, bool>(g2CCreateRoom.RoomId, true);
            room.RoomId = g2CCreateRoom.RoomId;
            Room.IsMaster = true;
            room.MyId = root.GetComponent<PlayerComponent>().PlayerId;
            room.MasterId = root.GetComponent<PlayerComponent>().PlayerId;
            room.Add(room.MasterId, name);
            
            GameModule.UI.CloseUI<GameStartPage>();
            //GameModule.UI.ShowUI<RoomPage>();
            await GameModule.Scene.LoadScene("BattleTest");
      
            room.TpsInit(TimeInfo.Instance.ClientNow());
            var world = room.LSWorld;
            var actorComponent = world.GetComponent<ActorComponent>();
            //actorComponent.CreateSystemActor();
            ActorCreateInfo info = new ActorCreateInfo()
            {
                ActorType = ActorType.Player,
                SideType = SideType.SideA,
                DescId = 1,
                PlayerId = root.GetComponent<PlayerComponent>().PlayerId,
                Position = new TrueSync.TSVector(3, 0, -5),
                Rotation = new TrueSync.TSQuaternion(0, 0, 0, 1),
            };
            actorComponent.CreateActor(info);
            for (int i = 0; i < 5; i++)
            {
                ActorCreateInfo test = new ActorCreateInfo()
                {
                    ActorType = ActorType.Monster,
                    SideType = SideType.SideA,
                    DescId = 1,
                    //PlayerId = root.GetComponent<PlayerComponent>().PlayerId,
                    Position = new TrueSync.TSVector(i * 3 + 4, 0, i),
                    Rotation = new TrueSync.TSQuaternion(0, 0, 0, 1),
                };
                actorComponent.CreateActor(test);
            }
           
           
            //GameModule.UI.ShowUI<RoomPage>();
            GameModule.UI.ShowUI<BattlePage>();

        }
        
        public static async ETTask JoinRoom(long roomId, string name)
        {
            var root = GameHeler.Root();
            if (root.GetComponent<PlayerComponent>() == null)
            {
                await LoginHelper.Login(root, name, "123456");
            }
            
            root.RemoveComponent<Room>();
            var room = root.AddComponentWithId<Room, bool>(roomId, true);
            GameModule.UI.CloseUI<GameStartPage>();
            await GameModule.Scene.LoadScene("BattleTest");
            C2Room_JoinRoom c2RoomJoinRoom = C2Room_JoinRoom.Create();
            c2RoomJoinRoom.RoomId = roomId;
            c2RoomJoinRoom.Name = name;
            Room2C_JoinRoom room2CJoinRoom = await room.GetComponent<RoomSender>().Call(0, c2RoomJoinRoom) as Room2C_JoinRoom;
            
            if (room2CJoinRoom.Error != 0)
            {
                Log.Error($"创建房间失败:{room2CJoinRoom.Message}");
                return;
            }
            foreach (var into in room2CJoinRoom.Players)
            {
                room.Add(into.PlayerId, into.Name);
            }
            room.MasterId = room2CJoinRoom.PlayerId;
            room.MyId = root.GetComponent<PlayerComponent>().PlayerId;
            Room.IsMaster = false;
            LSWorld serverWorld = MemoryPackHelper.Deserialize(typeof(LSWorld), room2CJoinRoom.WolrdData, 0, room2CJoinRoom.WolrdData.Length) as LSWorld;
            serverWorld.SceneType = SceneType.Battle;
            serverWorld.SetIdGenerator(room2CJoinRoom.IdGenerator);
            room.TpsInit(room2CJoinRoom.StartTime, room2CJoinRoom.Frame, serverWorld);
            room.AddComponent<RoomPingComponent>();
            GameModule.UI.ShowUI<BattlePage>();
        }
        
        public static async  ETTask Test(string txt)
        {
            var root = GameHeler.Root();
            var room = root.GetComponent<Room>();
            if (Room.IsMaster)
            {
                SendTxt(room.MasterId, txt);
                return;
            }
            
            C2Room_TestChangeTxt c2RoomTestChangeTxt = C2Room_TestChangeTxt.Create();
            c2RoomTestChangeTxt.Txt = txt;
            room.GetComponent<RoomSender>().SendRoomSvr(c2RoomTestChangeTxt);
        }

        public static void SendTxt(long id, string txt)
        {
            var root = GameHeler.Root();
            var room = root.GetComponent<Room>();
           
            Room2C_TestChangeTxtNotify notify = Room2C_TestChangeTxtNotify.Create();
            notify.Id = id;
            notify.Txt = txt;
           
            room.GetComponent<RoomSender>().Broadcast(notify);
        }
    }
}