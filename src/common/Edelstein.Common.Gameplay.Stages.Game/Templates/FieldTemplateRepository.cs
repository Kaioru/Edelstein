using System;
using System.Diagnostics;
using System.Linq;
using Edelstein.Common.Gameplay.Templating;
using Edelstein.Protocol.Gameplay.Stages.Game.Templates;
using Edelstein.Protocol.Parser;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MoreLinq;

namespace Edelstein.Common.Gameplay.Stages.Game.Templates
{
    public class FieldTemplateRepository : TemplateRepository<FieldTemplate>
    {
        private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

        public FieldTemplateRepository(
            IDataDirectoryCollection collection,
            ILogger<FieldTemplateRepository> logger = null
        ) : base(CacheDuration)
        {
            logger ??= new NullLogger<FieldTemplateRepository>();

            var dirMap = collection.Resolve("Map/Map").ResolveAll();
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            dirMap.Children
                .Where(n => n.Name.StartsWith("Map"))
                .SelectMany(n => n.Children)
                .Select(n =>
                {
                    var id = Convert.ToInt32(n.Name.Split(".")[0]);
                    return new TemplateProvider<FieldTemplate>(
                        id,
                        () => new FieldTemplate(
                            id,
                            n.Resolve("foothold").ResolveAll(),
                            n.Resolve("portal").ResolveAll(),
                            n.Resolve("ladderRope").ResolveAll(),
                            n.Resolve("life").ResolveAll(),
                            n.Resolve("info").ResolveAll()
                        )
                    );
                })
                .DistinctBy(t => t.ID)
                .ForEach(t => Register(t));

            logger.LogInformation($"Loaded {Count} field templates in {stopwatch.Elapsed}");

            stopwatch.Stop();
        }
    }
}