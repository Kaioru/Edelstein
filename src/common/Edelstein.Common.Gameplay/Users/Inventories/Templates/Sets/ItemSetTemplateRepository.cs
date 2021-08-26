using System;
using System.Diagnostics;
using System.Linq;
using Edelstein.Common.Gameplay.Templating;
using Edelstein.Protocol.Parser;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MoreLinq;

namespace Edelstein.Common.Gameplay.Users.Inventories.Templates.Sets
{
    public class ItemSetTemplateRepository : TemplateRepository<ItemSetTemplate>
    {
        private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

        public ItemSetTemplateRepository(
            IDataDirectoryCollection collection,
            ILogger<ItemSetTemplateRepository> logger = null
        ) : base(CacheDuration)
        {
            logger ??= new NullLogger<ItemSetTemplateRepository>();

            var dirItemSets = collection.Resolve("Etc/SetItemInfo.img").ResolveAll();
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            dirItemSets.Children
                .Select(n =>
                {
                    var id = Convert.ToInt32(n.Name);
                    return new TemplateProvider<ItemSetTemplate>(
                        id,
                        () => new ItemSetTemplate(
                            id,
                            n.ResolveAll(),
                            n.Resolve("ItemID").ResolveAll(),
                            n.Resolve("Effect").ResolveAll()
                        )
                    );
                })
                .DistinctBy(t => t.ID)
                .ForEach(t => Register(t));

            logger.LogInformation($"Loaded {Count} item set templates in {stopwatch.Elapsed}");

            stopwatch.Stop();
        }
    }
}