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
            //self.Owner.AddEventListener<TSVector>(BattleEvent.ForceChangePosition, OnForceChangePosition, self);
        }

        [EntitySystem]
        public static void Destroy(this ViewComponent self)
        {
            self.Owner?.RemoveAllListenerByOwner(self);
        }
        
        [EntitySystem]
        public static void Update(this ViewComponent self)
        {

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
            self.Go = self.ModelGo;
            var room = self.Scene<Room>();
            room.GetComponent<BattleRootView>().AddGameObject(self, self.Go);
            self.Animator = self.ModelGo.GetComponent<Animator>();
            if(self.Animator == null)
            {
                self.Animator = self.ModelGo.GetComponentInChildren<Animator>();
            }
            //self.Controller = self.ModelGo.GetComponent<CharacterController>();
        }
        
        private static void OnForceChangePosition(this Entity entity, TSVector position)
        {
            var self = entity.As<ViewComponent>();
            if (self.Go != null)
            {
                self.Go.transform.position = position.ToVector();
            }
        }
        
    }
    
    

}