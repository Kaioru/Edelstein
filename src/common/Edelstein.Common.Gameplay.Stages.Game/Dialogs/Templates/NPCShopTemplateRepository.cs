using System;
using System.Diagnostics;
using System.Linq;
using Edelstein.Common.Gameplay.Templating;
using Edelstein.Protocol.Parser;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MoreLinq;

namespace Edelstein.Common.Gameplay.Stages.Game.Dialogs.Templates
{
    public class NPCShopTemplateRepository : TemplateRepository<NPCShopTemplate>
    {
        private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

        public NPCShopTemplateRepository(
            IDataDirectoryCollection collection,
            ILogger<NPCShopTemplateRepository> logger = null
        ) : base(CacheDuration)
        {
            logger ??= new NullLogger<NPCShopTemplateRepository>();

            var dirNPCShop = collection.Resolve("Server/NpcShop.img").ResolveAll();
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            dirNPCShop.Children
                .Select(n =>
                {
                    var id = Convert.ToInt32(n.Name);
                    return new TemplateProvider<NPCShopTemplate>(
                        id,
                        () => new NPCShopTemplate(
                            id,
                            n.ResolveAll()
                        )
                    );
                })
                .DistinctBy(t => t.ID)
                .ForEach(t => Register(t));

            logger.LogInformation($"Loaded {Count} NPC shop templates in {stopwatch.Elapsed}");

            stopwatch.Stop();
        }
    }
}