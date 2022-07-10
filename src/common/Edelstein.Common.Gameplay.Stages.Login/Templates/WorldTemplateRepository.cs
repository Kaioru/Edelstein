using System;
using System.Diagnostics;
using System.Linq;
using Edelstein.Common.Gameplay.Templating;
using Edelstein.Protocol.Parser;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using static MoreLinq.Extensions.ForEachExtension;

namespace Edelstein.Common.Gameplay.Stages.Login.Templates
{
    public class WorldTemplateRepository : TemplateRepository<WorldTemplate>
    {
        private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

        public WorldTemplateRepository(
            IDataDirectoryCollection collection,
            ILogger<WorldTemplateRepository> logger = null
        ) : base(CacheDuration)
        {
            logger ??= new NullLogger<WorldTemplateRepository>();

            var dirWorld = collection.Resolve("Server/World.img").ResolveAll();
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            dirWorld.Children
                .Select(n =>
                {
                    var id = Convert.ToInt32(n.Name.Split(".")[0]);
                    return new TemplateProvider<WorldTemplate>(
                        id,
                        () => new WorldTemplate(
                            id,
                            n.ResolveAll()
                        )
                    );
                })
                .DistinctBy(t => t.ID)
                .ForEach(t => Register(t));

            logger.LogInformation($"Loaded {Count} world templates in {stopwatch.Elapsed}");

            stopwatch.Stop();
        }
    }
}
