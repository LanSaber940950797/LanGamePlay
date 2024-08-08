using Cysharp.Threading.Tasks;
using ET;
using TEngine;
using UnityEngine;

namespace GameLogic.Battle
{
    [ComponentOf(typeof(Actor))]
    public class ActorAIComponent : LSEntity, IAwake,ILSUpdate, IAwake<string>
    {
        public BattleAITreeSO AITreeSo;
        public string AITreeName;
        public Actor Actor => GetParent<Actor>();
        public int LastUpdateTime;
        //public readonly float DefaultTargetTime = 1f;
        
        
        // public EntityRef<Actor> Target;
        // public float TargetSelctTime;
        // public Vector3 MoveDir;
        // public Vector3 MovePos;
        // public Vector3 AttackDir;
        // public Vector3 AttackPos;
        
        
    }
    
    [EntitySystemOf(typeof(ActorAIComponent))]
    [LSEntitySystemOf(typeof(ActorAIComponent))]
    public static  partial class ActorAIComponentSystem
    {
        [EntitySystem]
        public static void Awake(this ActorAIComponent self, string treeName)
        {
            if (string.IsNullOrEmpty(treeName))
            {
               return; 
            }
            
            self.AITreeName = treeName;
            self.LoadAITree().Forget();
        }
        
        public static async UniTask LoadAITree(this ActorAIComponent self, string treeName = null)
        {
            if (treeName != null)
            {
                self.AITreeName = treeName;
            }
            if (self.AITreeSo == null)
            {
                self.AITreeSo = await GameModule.Resource.LoadAssetAsync<BattleAITreeSO>(self.AITreeName);
                self.AITreeSo.Actor = self.Actor;
                self.AITreeSo.IsInitialized = false;
                self.AITreeSo.Init(null);
            }

#if ENABLE_VIEW && UNITY_EDITOR
            var component = self.ViewGO.AddComponent<BattleAITreeViewComponent>();
            component.treeSo = self.AITreeSo;
#endif
        }

        [LSEntitySystem]
        public static void LSUpdate(this ActorAIComponent self)
        {
            var nowTime = self.FixFrameTime();
            if (self.LastUpdateTime < nowTime) //AI逻辑会产生新的行为，不能重复进入
            {
                self.AITreeSo?.Update();
            }
           
        }
        
    }
}