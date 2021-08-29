using System;
using System.Diagnostics;
using System.Linq;
using Edelstein.Common.Gameplay.Templating;
using Edelstein.Protocol.Parser;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MoreLinq;

namespace Edelstein.Common.Gameplay.Stages.Game.Continent.Templates
{
    public class ContiMoveTemplateRepository : TemplateRepository<ContiMoveTemplate>
    {
        private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

        public ContiMoveTemplateRepository(
            IDataDirectoryCollection collection,
            ILogger<ContiMoveTemplateRepository> logger = null
        ) : base(CacheDuration)
        {
            logger ??= new NullLogger<ContiMoveTemplateRepository>();

            var dir = collection.Resolve("Server/Continent.img").ResolveAll();
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            dir.Children
                .Select(n =>
                {
                    var id = Convert.ToInt32(n.Name);
                    return new TemplateProvider<ContiMoveTemplate>(
                        id,
                        () => new ContiMoveTemplate(
                            id,
                            n.ResolveAll(),
                            n.Resolve("field").ResolveAll(),
                            n.Resolve("scheduler").ResolveAll(),
                            n.Resolve("genMob")?.ResolveAll(),
                            n.Resolve("time").ResolveAll()
                        )
                    );
                })
                .DistinctBy(t => t.ID)
                .ForEach(t => Register(t));

            logger.LogInformation($"Loaded {Count} contimove templates in {stopwatch.Elapsed}");

            stopwatch.Stop();
        }
    }
}