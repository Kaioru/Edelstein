using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Edelstein.Common.Gameplay.Templating;
using Edelstein.Protocol.Parser;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MoreLinq;

namespace Edelstein.Common.Gameplay.Users.Inventories.Templates
{
    public class ItemStringTemplateRepository : TemplateRepository<ItemStringTemplate>
    {
        private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

        public ItemStringTemplateRepository(
            IDataDirectoryCollection collection,
            ILogger<ItemStringTemplateRepository> logger = null
        ) : base(CacheDuration)
        {
            logger ??= new NullLogger<ItemStringTemplateRepository>();

            var results = new List<TemplateProvider<ItemStringTemplate>>();
            var dir = collection.Resolve("String").ResolveAll();
            var dirEqp = dir.Resolve("Eqp.img").ResolveAll();
            var dirEtc = dir.Resolve("Etc.img").ResolveAll();
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var templatesEquip = dirEqp.Children
                .SelectMany(n => n.Children)
                .SelectMany(n => n.Children)
                .Where(c => c.Name.All(char.IsDigit))
                .Select(n =>
                {
                    var id = Convert.ToInt32(n.Name);
                    return new TemplateProvider<ItemStringTemplate>(
                        id,
                        () => new ItemStringTemplate(
                            id,
                            n.ResolveAll()
                        )
                    );
                })
                .DistinctBy(t => t.ID)
                .ToList();

            results.AddRange(templatesEquip);
            logger.LogInformation($"Loaded {templatesEquip.Count} equip item string templates in {stopwatch.Elapsed}");

            stopwatch.Reset();

            var templatesEtc = dirEtc.Children
                .SelectMany(n => n.Children)
                .Where(c => c.Name.All(char.IsDigit))
                .Select(n =>
                {
                    var id = Convert.ToInt32(n.Name);
                    return new TemplateProvider<ItemStringTemplate>(
                        id,
                        () => new ItemStringTemplate(
                            id,
                            n.ResolveAll()
                        )
                    );
                })
                .DistinctBy(t => t.ID)
                .ToList();

            new[] { "Consume", "Ins", "Cash" }.ForEach(d =>
            {
                dir.Resolve($"{d}.img").Children
                    .DistinctBy(c => c.Name)
                    .Where(c => c.Name.All(char.IsDigit))
                    .Select(n =>
                    {
                        var id = Convert.ToInt32(n.Name);
                        return new TemplateProvider<ItemStringTemplate>(
                            id,
                            () => new ItemStringTemplate(
                                id,
                                n.ResolveAll()
                            )
                        );
                    })
                    .DistinctBy(t => t.ID)
                    .ForEach(templatesEtc.Add);
            });

            results.AddRange(templatesEtc);
            logger.LogInformation($"Loaded {templatesEtc.Count} bundle item string templates in {stopwatch.Elapsed}");

            stopwatch.Stop();

            results
                .DistinctBy(t => t.ID)
                .ForEach(t => Register(t));
        }
    }
}