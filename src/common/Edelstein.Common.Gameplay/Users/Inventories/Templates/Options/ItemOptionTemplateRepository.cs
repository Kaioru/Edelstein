using System;
using System.Diagnostics;
using System.Linq;
using Edelstein.Common.Gameplay.Templating;
using Edelstein.Protocol.Parser;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using static MoreLinq.Extensions.ForEachExtension;

namespace Edelstein.Common.Gameplay.Users.Inventories.Templates.Options
{
    public class ItemOptionTemplateRepository : TemplateRepository<ItemOptionTemplate>
    {
        private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

        public ItemOptionTemplateRepository(
            IDataDirectoryCollection collection,
            ILogger<ItemOptionTemplateRepository> logger = null
        ) : base(CacheDuration)
        {
            logger ??= new NullLogger<ItemOptionTemplateRepository>();

            var dirItemOptions = collection.Resolve("Item/ItemOption.img").ResolveAll();
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            dirItemOptions.Children
                .Select(n =>
                {
                    var id = Convert.ToInt32(n.Name);
                    return new TemplateProvider<ItemOptionTemplate>(
                        id,
                        () => new ItemOptionTemplate(
                            id,
                            n.Resolve("info").ResolveAll(),
                            n.Resolve("level").ResolveAll()
                        )
                    );
                })
                .DistinctBy(t => t.ID)
                .ForEach(t => Register(t));

            logger.LogInformation($"Loaded {Count} item option templates in {stopwatch.Elapsed}");

            stopwatch.Stop();
        }
    }
}
