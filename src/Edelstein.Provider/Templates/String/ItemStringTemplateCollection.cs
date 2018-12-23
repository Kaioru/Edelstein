using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Provider.Parser;
using MoreLinq.Extensions;

namespace Edelstein.Provider.Templates.String
{
    public class ItemStringTemplateCollection : AbstractEagerTemplateCollection
    {
        public ItemStringTemplateCollection(IDataDirectoryCollection collection) : base(collection)
        {
        }

        public override Task LoadAll()
        {
            var property = Collection.Resolve("String");

            property.Resolve(p =>
            {
                p.Resolve("Eqp.img/Eqp").Children
                    .SelectMany(c => c.Children)
                    .DistinctBy(c => c.Name)
                    .Where(c => c.Name.All(char.IsDigit))
                    .ToDictionary(
                        c => Convert.ToInt32(c.Name),
                        c => ItemStringTemplate.Parse(Convert.ToInt32(c.Name), c)
                    )
                    .ForEach(kv => Templates.Add(kv.Key, kv.Value));
                p.Resolve("Etc.img/Etc").Children
                    .DistinctBy(c => c.Name)
                    .Where(c => c.Name.All(char.IsDigit))
                    .ToDictionary(
                        c => Convert.ToInt32(c.Name),
                        c => ItemStringTemplate.Parse(Convert.ToInt32(c.Name), c)
                    )
                    .ForEach(kv => Templates.Add(kv.Key, kv.Value));
                new[] {"Consume", "Ins", "Cash"}.ForEach(d =>
                {
                    p.Resolve($"{d}.img").Children
                        .SelectMany(c => c.Children)
                        .DistinctBy(c => c.Name)
                        .Where(c => c.Name.All(char.IsDigit))
                        .ToDictionary(
                            c => Convert.ToInt32(c.Name),
                            c => ItemStringTemplate.Parse(Convert.ToInt32(c.Name), c)
                        )
                        .ForEach(kv => Templates.Add(kv.Key, kv.Value));
                });
            });
            return Task.CompletedTask;
        }
    }
}