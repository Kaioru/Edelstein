using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Edelstein.Common.Gameplay.Templating;
using Edelstein.Protocol.Parser;

namespace Edelstein.Common.Gameplay.Users.Skills.Templates
{
    public record CharacterSkillTemplate : ITemplate
    {
        public int ID { get; }

        public short MaxLevel { get; }

        public IDictionary<int, int> ReqSkill { get; }
        public IDictionary<int, CharacterSkillLevelTemplate> LevelData { get; }

        public CharacterSkillTemplate(int id, IDataProperty property)
        {
            ID = id;

            var entry = property.Resolve("common");
            ImmutableDictionary<int, CharacterSkillLevelTemplate> levelData;

            if (entry != null)
            {
                var maxLevel = entry.Resolve<int>("maxLevel") ?? 0;

                levelData = Enumerable
                    .Range(1, maxLevel)
                    .ToImmutableDictionary(
                        i => i,
                        i => new CharacterSkillLevelTemplate(i, entry.ResolveAll())
                    );
            }
            else
            {
                entry = property.Resolve("level");
                levelData = entry.Children.ToImmutableDictionary(
                    c => Convert.ToInt32(c.Name),
                    c => new CharacterSkillLevelTemplate(Convert.ToInt32(c.Name), c.ResolveAll(), false)
                );
            }

            MaxLevel = (short)levelData.Count;

            ReqSkill = property.Resolve("req")?.Children
                           .ToImmutableDictionary(
                               c => Convert.ToInt32(c.Name),
                               c => c.Resolve<int>() ?? 0
                           ) ?? ImmutableDictionary<int, int>.Empty;
            LevelData = levelData;
        }
    }
}
