using System;
using System.Collections.Generic;
using System.Linq;

namespace Edelstein.Provider.Templates.Skill
{
    public class SkillTemplate : ITemplate
    {
        public int ID { get; }

        public short MaxLevel { get; set; }
        public bool Summon { get; set; }

        public IDictionary<int, int> ReqSkill;
        public IDictionary<int, SkillLevelTemplate> LevelData;

        public SkillTemplate(int id, IDataProperty property)
        {
            ID = id;

            var entry = property.Resolve("common");
            var levelData = new Dictionary<int, SkillLevelTemplate>();

            if (entry != null)
            {
                var maxLevel = entry.Resolve<int>("maxLevel");

                for (var i = 1; i <= maxLevel; i++)
                    levelData.Add(i, new SkillLevelTemplate(i, id, entry.ResolveAll()));
            }
            else
            {
                entry = property.Resolve("level");
                levelData = entry.Children.ToDictionary(
                    c => Convert.ToInt32(c.Name),
                    c => new SkillLevelTemplate(Convert.ToInt32(c.Name), id, c)
                );
            }

            MaxLevel = (short) levelData.Count;
            Summon = property.Resolve("summon") != null;
            ReqSkill = property.Resolve("req")?.Children
                           .ToDictionary(
                               c => Convert.ToInt32(c.Name),
                               c => c.Resolve<int>() ?? 0
                           ) ?? new Dictionary<int, int>();
            LevelData = levelData;
        }
    }
}