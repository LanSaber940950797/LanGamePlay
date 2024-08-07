using ET;
using GameConfig;

namespace GameLogic.Battle
{
    [EntitySystemOf(typeof(Actor))]
    [LSEntitySystemOf(typeof(Actor))]
    public static partial class ActorSystem
    {
        [EntitySystem]
        public static void Awake(this Actor self, ActorCreateInfo info)
        {
            self.ActorType = info.ActorType;
            self.SideType = info.SideType;
            self.DescId = info.DescId;
            self.PlayerId = info.PlayerId;
            self.InitDataComponents();
            self.InitLogicComponents();
           
        }
        
        
        //加载数据组件，数据是要同步的,按理说只能存放数据
        private static void InitDataComponents(this Actor self)
        {
            self.AddComponent<TransformComponent>();
            self.AddComponent<NumericComponent>();
            self.AddComponent<StatusComponent>();
            self.AddComponent<SkillComponent>();
        }
        
        //加载逻辑功能组件，功能组件是不同步的
        private static void InitLogicComponents(this Actor self)
        {
            self.AddComponent<MoveComponent>();
            
            self.AddComponent<ActionPointComponent>();

            self.AttachAction<SpellActionAbility>();
            self.AttachAction<EffectAssignAbility>();
            self.AttachAction<DamageActionAbility>();
            self.AttachAction<CureActionAbility>();
            self.AttachAction<MoveAbility>();
            self.AttachAction<CreateActorAbility>();

            if (self.ActorType == ActorType.Player)
            {
                var myId = self.Root().GetComponent<PlayerComponent>().PlayerId;
                if (myId == self.PlayerId) //本地玩家AI控制器是在本地上的
                {
                    self.AddComponent<ActorAIComponent>();
                }
            }
            else
            {
                if (self.IsServer())
                {
                    self.AddComponent<ActorAIComponent>();
                }
            }
        }

        #region 能力挂载

        public static T AttachAction<T>(this Actor self) where T : LSEntity, IActionAbility,IAwake, new()
        {
            var action = self.AddComponent<T>();
            action.Enable = true;
            return action;
        }
        
       
        
        public static SkillAbility AttachSkill(this Actor self, SpellDesc desc)
        {
            var skill = self.GetComponent<SkillComponent>().AddChild<SkillAbility, SpellDesc>(desc);
            return skill;
        }

        #endregion

        [EntitySystem]
        public static void Deserialize(this Actor self)
        {
            self.InitLogicComponents();
        }
    }
}