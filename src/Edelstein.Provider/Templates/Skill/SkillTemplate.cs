using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Edelstein.Provider.Parsing;

namespace Edelstein.Provider.Templates.Skill
{
    public class SkillTemplate : ITemplate
    {
        public int ID { get; }

        public short MaxLevel { get; }
        public bool Summon { get; }

        public IDictionary<int, int> ReqSkill { get; }
        public IDictionary<int, SkillLevelTemplate> LevelData { get; }

        public SkillTemplate(int id, IDataProperty property)
        {
            ID = id;

            var entry = property.Resolve("common");
            ImmutableDictionary<int, SkillLevelTemplate> levelData;

            if (entry != null)
            {
                var maxLevel = entry.Resolve<int>("maxLevel") ?? 0;

                levelData = Enumerable
                    .Range(1, maxLevel)
                    .ToImmutableDictionary(
                        i => i,
                        i => new SkillLevelTemplate(i, id, entry.ResolveAll())
                    );
            }
            else
            {
                entry = property.Resolve("level");
                levelData = entry.Children.ToImmutableDictionary(
                    c => Convert.ToInt32(c.Name),
                    c => new SkillLevelTemplate(Convert.ToInt32(c.Name), id, c.ResolveAll(), false)
                );
            }

            MaxLevel = (short) levelData.Count;
            Summon = property.Resolve("summon") != null;
            ReqSkill = property.Resolve("req")?.Children
                           .ToImmutableDictionary(
                               c => Convert.ToInt32(c.Name),
                               c => c.Resolve<int>() ?? 0
                           ) ?? ImmutableDictionary<int, int>.Empty;
            LevelData = levelData;
        }
    }
}