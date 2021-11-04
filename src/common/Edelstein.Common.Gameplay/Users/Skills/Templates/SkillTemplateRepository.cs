using System;
using System.Diagnostics;
using System.Linq;
using Edelstein.Common.Gameplay.Templating;
using Edelstein.Protocol.Parser;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MoreLinq;

namespace Edelstein.Common.Gameplay.Users.Skills.Templates
{
    public class SkillTemplateRepository : TemplateRepository<SkillTemplate>
    {
        private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

        public SkillTemplateRepository(
            IDataDirectoryCollection collection,
            ILogger<SkillTemplateRepository> logger = null
        ) : base(CacheDuration)
        {
            logger ??= new NullLogger<SkillTemplateRepository>();

            var dirSkills = collection.Resolve("Skill").ResolveAll();
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            dirSkills.Children
                .Where(c => c.Name.Split(".")[0].All(char.IsDigit))
                .SelectMany(c => c.Resolve("skill").Children)
                .Select(n =>
                {
                    var id = Convert.ToInt32(n.Name);
                    return new TemplateProvider<SkillTemplate>(
                        id,
                        () => new SkillTemplate(
                            id,
                            n.ResolveAll()
                        )
                    );
                })
                .DistinctBy(t => t.ID)
                .ForEach(t => Register(t));

            logger.LogInformation($"Loaded {Count} character skill templates in {stopwatch.Elapsed}");

            stopwatch.Stop();
        }
    }
}
