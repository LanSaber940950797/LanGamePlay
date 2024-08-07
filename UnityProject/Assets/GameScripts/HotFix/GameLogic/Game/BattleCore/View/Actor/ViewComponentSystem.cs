 using ET;
using TEngine;
using TrueSync;
using UnityEngine;
using Log = ET.Log;

namespace GameLogic.Battle
{
    [EntitySystemOf(typeof(ViewComponent))]
    public static partial class ViewComponentSystem
    {
        [EntitySystem]
        public  static void Awake(this ViewComponent self, LSEntity owner, string modePath)
        {
            self.owner = owner;
            self.ModePath = modePath;
            var room = self.Scene<Room>();
            if (!string.IsNullOrEmpty(self.ModePath))
            {
                self.LoadMode(self.ModePath).NoContext();
            }
        }

        [EntitySystem]
        public static void Destroy(this ViewComponent self)
        {
            self.Owner?.RemoveAllListenerByOwner(self);
        }
        

        
        public static async ETTask LoadMode(this ViewComponent self, string path)
        {
            if (self.ModePath == null)
            {
                self.ModePath = path;
            }
            if (self.ModelGo != null)
            {
                GameObject.Destroy(self.ModelGo);
            }
            self.ModelGo = await GameModule.Resource.LoadGameObjectAsync(path);
            var room = self.Scene<Room>();
            if (BattleConstValue.WorldType == BattleWorldType.TwoDimensional)
            { //2d世界下，模型动作会改变角色y坐标，所以需要创建一个gameobject承载
                self.Go = room.GetComponent<BattleRootView>().CreateGameObject(self);
                self.ModelGo.transform.SetParent(self.Go.transform);
            }
            else //3d世界直接使用模型gameobject
            {
                self.Go = self.ModelGo;
                room.GetComponent<BattleRootView>().AddGameObject(self, self.Go);
            }
        }
        
      
        
    }
    
    

}