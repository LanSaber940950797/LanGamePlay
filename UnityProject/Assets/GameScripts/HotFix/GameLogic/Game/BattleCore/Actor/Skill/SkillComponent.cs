using System.Collections.Generic;
using ET;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace GameLogic.Battle
{
   

    /// <summary>
    /// 技能施法组件
    /// </summary>
    [ComponentOf(typeof(Actor))]
    [MemoryPackable]
    public partial class SkillComponent :LSEntity,IAwake, ISerializeToEntity
    {
        [MemoryPackIgnore]
        [BsonIgnore]
        public Actor Actor => GetParent<Actor>();
    }

    [EntitySystemOf(typeof(SkillComponent))]
    public static partial class SkillComponentSystem
    {
        public static bool TrySpell(this SkillComponent self, SkillAbility skill, SpellCastParam castParam)
        {
            //条件检查
            if (skill.CheckCondition(castParam) == false)
            {
                return false;
            }
            if (self.Actor.GetComponent<SpellActionAbility>().TryMakeAction(out var spellAction))
            {
                spellAction.SkillAbility = skill;
                spellAction.SpellCastParam = castParam;
                spellAction.DoAction();
                return true;
            }
            
            return false;
        }
        
       
        
        public static List<SkillAbility> GetReadySkills(this SkillComponent self)
        {
            List<SkillAbility> skills = new List<SkillAbility>();
            foreach (SkillAbility skill in self.Children.Values)
            {
                if (skill.IsReady())
                {
                    skills.Add(skill);
                }
            }
           

            return skills;
        }
        
        //根据优先级
        public static List<SkillAbility> GetReadySkillsByPriority(this SkillComponent self)
        {
            var skills = self.GetReadySkills();
            //优先级大的在前面
            skills.Sort((a, b) => b.Desc.Priority - a.Desc.Priority);
            return skills;
        }
        
        public static SkillAbility GetSkill(this SkillComponent self, int id)
        {
            foreach (SkillAbility skill in self.Children.Values)
            {
                if (skill.SkillId == id)
                {
                    return skill;
                }
            }
            return null;
        }
    }
}