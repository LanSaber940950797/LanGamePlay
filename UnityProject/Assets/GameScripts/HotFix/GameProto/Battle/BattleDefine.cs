using System;

namespace GameLogic.Battle
{
    public enum EBattleWorldType
    {
        TwoDimensional,
        ThreeDimensional
    }
    public static class BattleDefine
    {
        public const int BattleMaxNumber = 100000000;
        public const double BattleEpsilon = 0.001;
        public const string SkillConfigFolderFolder = "config/skill/";
        public static EBattleWorldType WorldType = EBattleWorldType.TwoDimensional;
        public const string SkillTreeFolder = "Assets/AssetRaw/BattleConfig/Skill/";
        
    }
   


    [Flags]
    public enum ERoleType
    {
        None = 0,
        Hero = 1,
        Monster = 1 << 1,
        Bullet = 1 << 2,
        Assistant = 1 << 3,
        MagicField = 1 << 4,
    }

    [Flags]
    public enum ERoleTeamType
    {
        None = 0,
        Good = 1,
        Bad = 2,
        Neutral = 4,
        All = Good | Bad | Neutral
    }

    [Flags]
    public enum ERoleFlag
    {
        None = 0,
        Summoned = 1,
        Dead = 1 << 1,
        Delete = 1 << 2,
    }

    public enum ERoleShape
    {
        None = 0,
        Circle = 1,
        Rect = 2,
    }

    public enum ERoleDeadMethod
    {
        None = 0,
        Hp,
        AttackedTimes,
        // AttackedDamage,
    }

    [Flags]
    public enum ETargetTeamType
    {
        None = 0,
        Friend = 1,
        Enemy = 2,
        Peace = 4
    }

    public enum ECastSelectMode
    {
        None = 0,
        Point = 1,
        Role = 2,
    }

    public enum ETargetSelectShape
    {
        None = 0,
        Single = 1,
        Circle = 2,
        Rect = 3,
        Sector = 4
    }

    public enum ETargetSortMode
    {
        None = 0,
        DistanceNearby = 1,
        DistanceFaraway = 2,
        HpLow = 3,
        HpHigh = 4,
        HpPercentLow = 5,
        HpPercentHigh = 6,
    }

    public enum EEffectiveRoleType
    {
        None = 0,
        Caster, //施法者
        Owner, //拥有者
        Target //当前目标
    }

    public enum EFilterTargetMethod
    {
        None,
        Exclude,
        Include
    }

    public enum EFilterConditionType
    {
        None = 0,
        RoleInstanceId,
        RoleDescId,
    }

    public enum EPositionType
    {
        Select,//选择的位置
        Role,//角色的位置
    }

    public enum EChangeAttributeMode
    {
        Fixed,//固定值
        PercentOnCurrentValue,//当前值的百分比
        PercentOnOriginalValue,//初始值的百分比
    }

    public enum ESkillType
    {
        NormalAttack = 0,
        CounterAttack,
        Proactive,
        Passive,
        Buff,
        Bullet,
    }

    public enum EModifierSourceType
    {
        None,
        Skill,
        Buff,
        Equipment,
        CultivateSystem,
        Mainland,
        LevelReward,
        GM
    }

    public enum EDamageSourceType
    {
        NormalAttack,
        CounterAttack,
        ProactiveSkill,
        PassiveSkill,
        Buff,
        Bullet,
        System,
    }

    public enum EDamageType
    {
        Normal,//普通伤害
        True, //真实伤害
        Fixed, //固定伤害,
        Slay //斩杀伤害
    }

    [Flags]
    public enum EDamageFlag
    {
        Normal = 0,
        Evasion = 1 << 0,       // 闪避
        Crit = 1 << 1,          // 暴击
        Block = 1 << 2,         // 格挡
        Immune = 1 << 3,          // 免疫
    }

    public enum EHealSourceType
    {
        Skill,
        Buff,
        HpSteal,
        System
    }

    public enum EBuffRefreshMode
    {
        None = 0,
        AddLayer = 1,
        RefreshDuration = 1 << 1,
        AddDuration = 1 << 2,
    }

    public enum EBuffTriggerMethod
    {
        None = 0,
        OnEvent = 1,
        Repeat = 1 << 1,
        OnAction = 1 << 2,
    }

    public enum EBuffRemoveMethod
    {
        None = 0,
        OnEvent = 1,
        OnMaxTriggerTimes = 1 << 1,
        OnMaxLayer = 1 << 2,
        OnAction = 1 << 3,
    }

    [Flags]
    public enum EBuffTag
    {
        None = 0,
        Cure = 1,//治疗标签
        Damage = 1 << 1,//伤害标签
        Gain = 1 << 2,//增益标签
        Reduce = 1 << 3,//减益标签
        Control = 1 << 4,//控制标签
        StrongControl = 1 << 5,//强控标签
        IgnoreDispel = 1 << 6,//驱散标签
        IgnoreStrongDispel = 1 << 7,//强驱散标签
    }

    public enum EBuffFilterMode
    {
        DescId,
        Tag
    }

    public enum ESkillFilterMode
    {
        Current,//当前技能
        Source, //来源
        DescId, //表id
        Index, //角色身上的技能索引
        All//所有主动技能
    }

    [Flags]
    public enum ERoleStatusFlag
    {
        None = 0,
        Rooted = 1, //定身，不能移动，可以正常释放技能和普通攻击
        Disarm = 1 << 1,//缴械，不能普通攻击，可以释放技能
        Slienced = 1 << 2, //沉默，不能放技能，可以移动和普攻
        TimeFrozen = 1 << 3, //时间暂停，身上的计时器也会暂停
        DamageImmune = 1 << 4, //伤害免疫
        Unselectable = 1 << 5, //不可选中
        ControlImmune = 1 << 6, //控制免疫
        StrongControlImmune = 1 << 7,//强控免疫
        UnPickable = 1 << 8, //不可拾取
        Combined = 1 << 9, //合体状态

        Stunned = Rooted | Disarm | Slienced, //不能控制（不能移动、不能释放任何技能包括普通攻击）
        AllSlienced = Disarm | Slienced, //全沉默，不能放技能以及普攻，可以移动
    }
    // [Flags]
    // public enum ActionPointType
    // {
    //     //[LabelText("（空）")]
    //     None = 0,
    //
    //     //[LabelText("造成伤害前")]
    //     PreCauseDamage = 1 << 1,
    //     //[LabelText("承受伤害前")]
    //     PreReceiveDamage = 1 << 2,
    //
    //     //[LabelText("造成伤害后")]
    //     PostCauseDamage = 1 << 3,
    //     //[LabelText("承受伤害后")]
    //     PostReceiveDamage = 1 << 4,
    //
    //     //[LabelText("给予治疗后")]
    //     PostGiveCure = 1 << 5,
    //     //[LabelText("接受治疗后")]
    //     PostReceiveCure = 1 << 6,
    //
    //     //[LabelText("赋给技能效果")]
    //     AssignEffect = 1 << 7,
    //     //[LabelText("接受技能效果")]
    //     ReceiveEffect = 1 << 8,
    //
    //     //[LabelText("赋加状态后")]
    //     PostGiveBuff = 1 << 9,
    //     //[LabelText("承受状态后")]
    //     PostReceiveBuff = 1 << 10,
    //     
    //     //[LabelText("施法前")]
    //     PreSpell = 1 << 17,
    //     //[LabelText("施法后")]
    //     PostSpell = 1 << 18,
    //
    //     Max,
    // }
    public enum EBulletEmitMode
    {
        None,
        Point,
        Role
    }

    public enum EBulletMovementType
    {
        None = 0,
        Forward = 1,
        Surround = 1 << 1,
        Follow = 1 << 2,
    }

    public enum ENodeViewType
    {
        Role,
        VFX
    }

    public enum ENodeViewLoadLocation
    {
        CasterBody = 1,
        CasterScenePosition = 2,
        TargetBody = 3,
        TargetScenePosition = 4,
        OwnerBody = 5,
        OwnerScenePosition = 6,
        SelectPosition = 7
    }

    public enum EBodySkewType
    {
        None = 0,
        BodyBone = 1,
        BodyPoint = 2,
        PositionOffset = 3
    }

    public enum EBodyPoint
    {
        None = 0,
        Head = 1,
        Chest = 2,
        Foot = 3,
        Waist = 4
    }

    public enum EBodyBone
    {
        None = 0,
        Head = 1,
        Chest = 2,
        LeftWeapon = 3,
        RightWeapon = 4
    }

    public enum ENumberMathType
    {
        None = 0,
        Add = 1,
        Sub = 2,
        Mul = 3,
        Div = 4,
        Mod = 5,
    }

    public enum EVectorMathType
    {
        None = 0,
        Add = 1,
        Sub = 2,
        Mul = 3,
    }

    public enum EVectorAxis
    {
        X = 0,
        Y,
        Z
    }

    public enum ECompareType
    {
        Greater = 1,
        Equal = 1 << 1,
        Lesser = 1 << 2,
    }

    public enum EFlowLimitMode
    {
        None,
        LimitNumberOfTimes,
        CoolDown
    }

    public enum EControlRepeatMode
    {
        None = 0,
        RepeatWithTimes,
        RepeatWithDuration
    }

    public enum ETouchInputType
    {
        None,
        Move,
        Select,
        Confirm,
        Cancel
    }

    public enum EHpType
    {
        CurrentHp,
        MaxHp
    }

    public enum ERoundType
    {
        Floor,//向下取整
        Ceiling,//向上取整
        Round,//四舍五入取整
    }
    
    public enum TargetSideType
    {
        None = 0,
        Friend = 1,
        Enemy = 2,
    }
    
    /// <summary>
    /// 施法目标类型
    /// </summary>
    public enum SpellTargetType
    {
        None,
        Target = 1,
        Ponit = 2,
    }
}
    
