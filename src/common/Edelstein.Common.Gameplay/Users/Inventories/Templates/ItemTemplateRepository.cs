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
    public class ItemTemplateRepository : TemplateRepository<ItemTemplate>
    {
        private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

        public ItemTemplateRepository(
            IDataDirectoryCollection collection,
            ILogger<ItemTemplateRepository> logger = null
        ) : base(CacheDuration)
        {
            logger ??= new NullLogger<ItemTemplateRepository>();

            var results = new List<TemplateProvider<ItemTemplate>>();
            var dirCharacter = collection.Resolve("Character").ResolveAll();
            var dirItem = collection.Resolve("Item").ResolveAll();

            var nodesEquip = new List<IDataProperty>() {
                dirCharacter.Resolve("Accessory"),
                dirCharacter.Resolve("Cap"),
                dirCharacter.Resolve("Cape"),
                dirCharacter.Resolve("Coat"),
                dirCharacter.Resolve("Dragon"),
                dirCharacter.Resolve("Glove"),
                dirCharacter.Resolve("Longcoat"),
                dirCharacter.Resolve("Mechanic"),
                dirCharacter.Resolve("Pants"),
                dirCharacter.Resolve("PetEquip"),
                dirCharacter.Resolve("Ring"),
                dirCharacter.Resolve("Shield"),
                dirCharacter.Resolve("Shoes"),
                dirCharacter.Resolve("TamingMob"),
                dirCharacter.Resolve("Weapon"),
            };
            var nodesItem = new List<IDataProperty>() {
                dirItem.Resolve("Cash"),
                dirItem.Resolve("Consume"),
                dirItem.Resolve("Etc"),
                dirItem.Resolve("Install"),
            };
            var nodesPet = dirItem.Resolve("Pet");
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var templatesEquip = nodesEquip
                .SelectMany(n => n.Children)
                .Select(n =>
                {
                    var id = Convert.ToInt32(n.Name.Split(".")[0]);
                    return new TemplateProvider<ItemTemplate>(
                        id,
                        () => new ItemEquipTemplate(id, n.Resolve("info").ResolveAll())
                    );
                })
                .ToList();

            results.AddRange(templatesEquip);
            logger.LogInformation($"Loaded {templatesEquip.Count} equip item templates in {stopwatch.Elapsed}");

            stopwatch.Reset();

            var templatesItem = nodesItem
                .SelectMany(n => n.Children)
                .SelectMany(n => n.Children)
                .Select(n =>
                {
                    var id = Convert.ToInt32(n.Name);
                    return new TemplateProvider<ItemTemplate>(
                        id,
                        () => new ItemBundleTemplate(id, n.Resolve("info").ResolveAll())
                    );
                })
                .ToList();

            results.AddRange(templatesItem);
            logger.LogInformation($"Loaded {templatesItem.Count} bundle item templates in {stopwatch.Elapsed}");

            stopwatch.Reset();

            var templatesPet = nodesItem
                .SelectMany(n => n.Children)
                .Select(n =>
                {
                    var id = Convert.ToInt32(n.Name.Split(".")[0]);
                    return new TemplateProvider<ItemTemplate>(
                        id,
                        () => new ItemPetTemplate(id, n.Resolve("info").ResolveAll())
                    );
                })
                .ToList();

            results.AddRange(templatesPet);
            logger.LogInformation($"Loaded {templatesPet.Count} pet item templates in {stopwatch.Elapsed}");

            stopwatch.Stop();

            results
                .DistinctBy(t => t.ID)
                .ForEach(t => Register(t));
        }
    }
}
