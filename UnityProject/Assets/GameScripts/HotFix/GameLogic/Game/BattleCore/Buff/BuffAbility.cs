using Cysharp.Threading.Tasks;
using ET;
using GameConfig;
using TEngine;

namespace GameLogic.Battle
{
    public class BuffAbility : LSEntity,IAbilityEntity, IAwake<BuffDesc>, IDestroy
    {
        public Actor Owner { get; set; }
        public Actor ParentActor { get => GetParent<Actor>(); }
        public bool Enable { get; set; }
        public BuffDesc Desc;
        public SkillTreeSO TreeSo;
        public int DescId;
        public int _layer;
        public int Layer
        {
            get => _layer;
            set
            {
                _layer = value;
            }
        }
        private EntityRef<SkillTreeComponent> skillTreeComponent;

        public SkillTreeComponent SkillTreeComponent
        {
            get
            {
                return skillTreeComponent;
            }
            set
            {
                skillTreeComponent = value;
            }
        }
        public float Duration;
        public EBuffRefreshMode RefreshMode;
        
        private EntityRef<BuffExecution> buffExecution;

        public BuffExecution BuffExecution
        {
            get
            {
                return buffExecution;
            }
            set
            {
                buffExecution = value;
            }
        }
        
    }

    [EntitySystemOf(typeof(BuffAbility))]
    public static partial class BuffAbilitySystem
    {
        [EntitySystem]
        public static void Awake(this BuffAbility self, BuffDesc desc)
        {
            self.Desc = desc;
            self.DescId = desc.Id;
            self.ViewName = desc.Name;
            self.RefreshMode = desc.RefreshMode;
            self.BuffExecution = null;
            
            
            var effectDesc = ConfigSystem.Instance.Tables.TbEffect.Get(desc.Id, 1);
            self.AddComponent<AbilityEffectComponent, EffectDesc>(effectDesc);
            if (!string.IsNullOrEmpty(desc.TreeName))
            {
                self.SkillTreeComponent = self.AddComponent<SkillTreeComponent,string>(desc.TreeName);
            }
            
        }
        
        
        
        public static void TryActivateAbility(this BuffAbility self)
        {
            self.ActivateAbility();
        }

        public static void ActivateAbility(this BuffAbility self)
        {
            self.Enable = true;
            self.CreateExecution();
            self.BuffExecution.BeginExecute();
        }

        public static void RefreshBuff(this BuffAbility self,int layer)
        {
            if ((self.RefreshMode & EBuffRefreshMode.AddLayer) > 0)
            {
                if (self.Desc.MaxLayer == 0 || self._layer < self.Desc.MaxLayer)
                {
                    self._layer += layer;
                    if (self.Desc.MaxLayer > 0 && self._layer > self.Desc.MaxLayer)
                    {
                        self._layer = self.Desc.MaxLayer;
                    }
                }

                self.OnLayerChange();
            }

            if ((self.RefreshMode & EBuffRefreshMode.AddDuration) > 0)
            {
                if (self.Desc.Duration > 0)
                {
                    self.Duration += self.Desc.Duration;
                }
            }

            if ((self.RefreshMode & EBuffRefreshMode.RefreshDuration) > 0)
            {
                if (self.Desc.Duration > 0)
                {
                    self.Duration = self.Desc.Duration;
                }
            }
            
            
        }
        
        public static void DeactivateAbility(this BuffAbility self)
        {
            self.BuffExecution.EndExecute();
            self.Enable = false;
        }

        public static void EndAbility(this BuffAbility self)
        {
            
        }

        public static void OnLayerChange(this BuffAbility self)
        {
            
        }

        public static BuffExecution CreateExecution(this BuffAbility self)
        {
            self.BuffExecution = self.AddChild<BuffExecution, BuffAbility>(self);
            return self.BuffExecution;
        }

        [EntitySystem]
        public static  void Destroy(this BuffAbility self)
        {
            self.DeactivateAbility();
        }
    }
}