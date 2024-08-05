using ET;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameLogic.Battle
{
    [EntitySystemOf(typeof(BattleRootView))]
    public static partial class BattleRootViewSystem
    {
        [EntitySystem]
        public static void Awake(this BattleRootView self)
        {
            self.BattleRoot = new GameObject("BattleRoot");
        }

        [EntitySystem]
        public static void Destroy(this BattleRootView self)
        {
            Object.Destroy(self.BattleRoot);
            self.BattleRoot = null;
        }
        
        public static GameObject CreateGameObject(this BattleRootView self, Entity entity)
        {
            var obj = new GameObject(entity.ViewName);
            obj.transform.SetParent(self.BattleRoot.transform);
            return obj;
        }

        public static void AddGameObject(this BattleRootView self, Entity entity, GameObject go)
        {
            go.transform.SetParent(self.BattleRoot.transform);
        }
    }
}