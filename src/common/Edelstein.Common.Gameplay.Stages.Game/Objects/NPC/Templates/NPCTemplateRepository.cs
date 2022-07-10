using System;
using System.Diagnostics;
using System.Linq;
using Edelstein.Common.Gameplay.Templating;
using Edelstein.Protocol.Parser;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using static MoreLinq.Extensions.ForEachExtension;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.NPC.Templates
{
    public class NPCTemplateRepository : TemplateRepository<NPCTemplate>
    {
        private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

        public NPCTemplateRepository(
            IDataDirectoryCollection collection,
            ILogger<NPCTemplateRepository> logger = null
        ) : base(CacheDuration)
        {
            logger ??= new NullLogger<NPCTemplateRepository>();

            var dirNPC = collection.Resolve("Npc").ResolveAll();
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            dirNPC.Children
                .Select(n =>
                {
                    var id = Convert.ToInt32(n.Name.Split(".")[0]);
                    return new TemplateProvider<NPCTemplate>(
                        id,
                        () => new NPCTemplate(
                            id,
                            n.ResolveAll(),
                            n.Resolve("info").ResolveAll()
                        )
                    );
                })
                .DistinctBy(t => t.ID)
                .ForEach(t => Register(t));

            logger.LogInformation($"Loaded {Count} NPC templates in {stopwatch.Elapsed}");

            stopwatch.Stop();
        }
    }
}