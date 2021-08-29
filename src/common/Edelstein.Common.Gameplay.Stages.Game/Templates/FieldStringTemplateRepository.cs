using System;
using Edelstein.Common.Gameplay.Templating;
using Edelstein.Protocol.Parser;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using MoreLinq;

namespace Edelstein.Common.Gameplay.Stages.Game.Templates
{
    public class FieldStringTemplateRepository : TemplateRepository<FieldStringTemplate>
    {
        private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

        public FieldStringTemplateRepository(
            IDataDirectoryCollection collection,
            ILogger<FieldStringTemplateRepository> logger = null
        ) : base(CacheDuration)
        {
            logger ??= new NullLogger<FieldStringTemplateRepository>();

            var dir = collection.Resolve("String/Map.img").ResolveAll();
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            dir.Children
                .SelectMany(n => n.Children)
                .Select(n =>
                {
                    var id = Convert.ToInt32(n.Name);
                    return new TemplateProvider<FieldStringTemplate>(
                        id,
                        () => new FieldStringTemplate(
                            id,
                            n.ResolveAll()
                        )
                    );
                })
                .DistinctBy(t => t.ID)
                .ForEach(t => Register(t));

            logger.LogInformation($"Loaded {Count} field string templates in {stopwatch.Elapsed}");

            stopwatch.Stop();
        }
    }
}