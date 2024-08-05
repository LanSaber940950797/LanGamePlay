using System;

namespace GameLogic.Battle
{
    
    [Serializable]
    // 角色类型
    public enum ActorType
    {
        None = 0,
        Player = 1, //玩家
        Monster = 2,//怪物
        MagicField = 3,//法术场
        Bullet = 4, //子弹
        System = 5, //系统actor，管理战斗的
    }
    
    [Flags]
    [Serializable]
    public enum SideType
    {
        //阵营A
        SideA = 1,
        //阵容B
        SideB = 2,
        //中立
        Neutral = 3,
        Max = 4,
        All = 5,
    }
    public enum EffectiveActorType
    {
        None = 0,
        Caster, //施法者
        Owner, //拥有者
        Target //当前目标
    }
    [Flags]
    public enum StatusFlag {
        None = 0,
        Rooted = 1, //定身，不能移动，可以正常释放技能和普通攻击
        Disarm = 1<<1,//缴械，不能普通攻击，可以释放技能
        Slienced = 1<<2, //沉默，不能放技能，可以移动和普攻
        TimeFrozen = 1<<3, //时间暂停，身上的计时器也会暂停
        DamageImmune = 1<<4, //伤害免疫
        Unselectable = 1<<5, //不可选中
        ControlImmune = 1<<6, //控制免疫
        StrongControlImmune = 1 << 7,//强控免疫

        Stunned = Rooted | Disarm | Slienced, //不能控制（不能移动、不能释放任何技能包括普通攻击）
        AllSlienced = Disarm|Slienced, //全沉默，不能放技能以及普攻，可以移动
    }
    public enum AttributeSubType{
        None = 0,

        Base = 1,

        BaseAdd = 2,

        BaseAddRate = 3,

        FianlAdd = 4,

        FinalAddRate = 5,

    }
    public enum AttributeType{
        None = 0,

        Init = 1000,

        //基础属性1001-1100
        BaseAttrMin = 1000,

        PointQi = 1001,

        PointLing = 1002,

        PointTi = 1003,

        PointJing = 1004,

        PontHun = 1005,

        Luck = 1006,

        Life = 1007,

        Age = 1008,

        Hp = 1009,

        Mp = 1010,

        BaseAttrMax = 1011,

        //战斗属性2001-3000
        BattleAttrMin = 2000,

        HpMax = 2001,

        MpMax = 2002,

        Attack = 2003,

        MagicAttack = 2004,

        Def = 2005,

        MagicDef = 2006,

        Hit = 2007,

        Dodge = 2008,

        Crit = 2009,

        CritDef = 2010,

        CritDamageDef = 2011,

        CritDamageRate = 2012,

        AttackSpeed = 2013,

        MoveSpeed = 2014,

        DefPierce = 2015,

        MagicDefPierce = 2016,

        CastRange = 2017,

        BattleAttrMax = 2018,

        Max = 2017,

        OverMax = 10000,

    }
    
    [Flags]
    public enum ActionPointType
    {
        //[LabelText("（空）")]
        None = 0,

        //[LabelText("造成伤害前")]
        PreCauseDamage =  1,
        //[LabelText("承受伤害前")]
        PreReceiveDamage =  2,

        //[LabelText("造成伤害后")]
        PostCauseDamage =  3,
        //[LabelText("承受伤害后")]
        PostReceiveDamage =  4,

        //[LabelText("给予治疗后")]
        PostGiveCure =  5,
        //[LabelText("接受治疗后")]
        PostReceiveCure =  6,

        //[LabelText("赋给技能效果")]
        AssignEffect = 7,
        //[LabelText("接受技能效果")]
        ReceiveEffect =  8,

        //[LabelText("赋加状态后")]
        PostGiveBuff =  9,
        //[LabelText("承受状态后")]
        PostReceiveBuff =  10,
        PreMove =  11,
        Move = 12,//移动
        //[LabelText("施法前")]
        PreSpell =  17,
        //[LabelText("施法后")]
        PostSpell =  18,
        
       

        Max,
    }
}