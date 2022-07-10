using System;
using System.Diagnostics;
using System.Linq;
using Edelstein.Common.Gameplay.Templating;
using Edelstein.Protocol.Parser;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using static MoreLinq.Extensions.ForEachExtension;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.Mob.Templates
{
    public class MobTemplateRepository : TemplateRepository<MobTemplate>
    {
        private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

        public MobTemplateRepository(
            IDataDirectoryCollection collection,
            ILogger<MobTemplateRepository> logger = null
        ) : base(CacheDuration)
        {
            logger ??= new NullLogger<MobTemplateRepository>();

            var dirMob = collection.Resolve("Mob").ResolveAll();
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            dirMob.Children
                .Where(n => n.Name.Split(".")[0].All(char.IsDigit))
                .Select(n =>
                {
                    var id = Convert.ToInt32(n.Name.Split(".")[0]);
                    return new TemplateProvider<MobTemplate>(
                        id,
                        () => new MobTemplate(
                            id,
                            n.ResolveAll(),
                            n.Resolve("info").ResolveAll()
                        )
                    );
                })
                .DistinctBy(t => t.ID)
                .ForEach(t => Register(t));

            logger.LogInformation($"Loaded {Count} mob templates in {stopwatch.Elapsed}");

            stopwatch.Stop();
        }
    }
}