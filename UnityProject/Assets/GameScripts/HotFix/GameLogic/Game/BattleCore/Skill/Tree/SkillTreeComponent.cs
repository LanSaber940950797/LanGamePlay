using Cysharp.Threading.Tasks;
using ET;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using TEngine;

namespace GameLogic.Battle
{
    [MemoryPackable]
    public partial class SkillTreeComponent : LSEntity,IAwake<string>,ILSUpdate,IDestroy,ISerializeToEntity,IDeserialize
    {
        public string TreeName;
        
        [MemoryPackIgnore]
        [BsonIgnore]
        public SkillTreeSO Tree;


        public bool Enable { get; set; }
    }
    
    [EntitySystemOf(typeof(SkillTreeComponent))]
    [LSEntitySystemOf(typeof(SkillTreeComponent))]
    public static partial class SkillTreeComponentSystem
    {
        [EntitySystem]
        public static void Awake(this SkillTreeComponent self, string name)
        {
            self.TreeName = name;
            self.LoadSkillTree().NoContext();
        }
        
        public static async ETTask LoadSkillTree(this SkillTreeComponent self)
        {
            if (self.Tree == null)
            {
                self.Tree = await GameModule.Resource.LoadAssetAsync<SkillTreeSO>(self.TreeName);
                self.Tree.IsInitialized = false;
                self.Tree.IsRuning = false;
            }

#if ENABLE_VIEW && UNITY_EDITOR
            var component = self.ViewGO.AddComponent<SkillTreeViewComponent>();
            component.skillTreeSO = self.Tree;
#endif
        }
        
       
        [LSEntitySystem]
        public static void LSUpdate(this SkillTreeComponent self)
        {
            if (!self.Enable)
            {
                return;
            }
            
            if (self.Tree != null)
            {
                self.Tree.Update();
            }
        }
    
        [EntitySystem]
        public static void Destroy(this SkillTreeComponent self)
        {
            self.Tree = null;
        }

        public static bool IsRuning(this SkillTreeComponent self)
        {
            return self.Tree != null ? self.Tree.IsRuning : false;
        }

        public static void Init(this SkillTreeComponent self, IAbilityEntity ability, IAbilityExecute execute)
        {
            self.Tree.Init(ability, execute);
        }

        [EntitySystem]
        public static void Deserialize(this SkillTreeComponent self)
        {
            self.LoadSkillTree().NoContext();
        }
    }
}