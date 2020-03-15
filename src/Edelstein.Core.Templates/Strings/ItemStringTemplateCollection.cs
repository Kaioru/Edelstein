using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Provider;
using MoreLinq.Extensions;

namespace Edelstein.Core.Templates.Strings
{
    public class ItemStringTemplateCollection : AbstractEagerDataTemplateCollection
    {
        private readonly IDataDirectoryCollection _collection;

        public ItemStringTemplateCollection(IDataDirectoryCollection collection)
            => _collection = collection;

        protected override async Task<IEnumerable<IDataTemplate>> Load()
        {
            var templates = new List<IDataStringTemplate>();
            var property = _collection.Resolve("String");

            property.ResolveAll(p =>
            {
                p.Resolve("Eqp.img/Eqp").Children
                    .SelectMany(c => c.Children)
                    .DistinctBy(c => c.Name)
                    .Where(c => c.Name.All(char.IsDigit))
                    .Select(c => new ItemStringTemplate(
                        Convert.ToInt32(c.Name),
                        c.ResolveAll()))
                    .ForEach(templates.Add);
                p.Resolve("Etc.img/Etc").Children
                    .DistinctBy(c => c.Name)
                    .Where(c => c.Name.All(char.IsDigit))
                    .Select(c => new ItemStringTemplate(
                        Convert.ToInt32(c.Name),
                        c.ResolveAll()))
                    .ForEach(templates.Add);
                new[] {"Consume", "Ins", "Cash"}.ForEach(d =>
                {
                    p.Resolve($"{d}.img").Children
                        .DistinctBy(c => c.Name)
                        .Where(c => c.Name.All(char.IsDigit))
                        .Select(c => new ItemStringTemplate(
                            Convert.ToInt32(c.Name),
                            c.ResolveAll()))
                        .ForEach(templates.Add);
                });
            });

            return templates;
        }
    }
}