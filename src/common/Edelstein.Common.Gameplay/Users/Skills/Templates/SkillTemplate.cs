using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Edelstein.Common.Gameplay.Templating;
using Edelstein.Protocol.Parser;

namespace Edelstein.Common.Gameplay.Users.Skills.Templates
{
    public record SkillTemplate : ITemplate
    {
        public int ID { get; }

        public short MaxLevel { get; }
        public bool IsPSD { get; }
        public bool IsSummon { get; }

        public IDictionary<int, int> ReqSkill { get; }
        public IDictionary<int, ISkillLevelInfo> LevelData { get; }

        public SkillTemplate(int id, IDataProperty property)
        {
            ID = id;

            IsPSD = (property.Resolve<int>("psd") ?? 0) > 0;
            IsSummon = property.Resolve("summon") != null;

            ReqSkill = property.Resolve("req")?.Children
                           .ToImmutableDictionary(
                               c => Convert.ToInt32(c.Name),
                               c => c.Resolve<int>() ?? 0
                           ) ?? ImmutableDictionary<int, int>.Empty;

            var common = property.Resolve("common");

            Console.WriteLine($"{id} : {common == null}");

            if (common != null)
            {
                var maxLevel = common.Resolve<int>("maxLevel") ?? 0;

                LevelData = Enumerable
                    .Range(1, maxLevel)
                    .ToImmutableDictionary(
                        i => i,
                        i => (ISkillLevelInfo)new SkillLevelCommonTemplate(i, common.ResolveAll())
                    );
            }
            else
            {
                var level = property.Resolve("level");

                LevelData = level.Children.ToImmutableDictionary(
                    c => Convert.ToInt32(c.Name),
                    c => (ISkillLevelInfo)new SkillLevelTemplate(Convert.ToInt32(c.Name), c.ResolveAll())
                );
            }

            MaxLevel = (short)LevelData.Count;
        }
    }
}
