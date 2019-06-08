using System;
using System.Collections.Generic;
using System.Linq;
using Edelstein.Provider.Parsing;
using MoreLinq;

namespace Edelstein.Provider.Templates.String
{
    public class ItemStringTemplateCollection : AbstractEagerTemplateCollection
    {
        public override TemplateCollectionType Type => TemplateCollectionType.ItemString;

        public ItemStringTemplateCollection(IDataDirectoryCollection collection) : base(collection)
        {
        }

        protected override IEnumerable<ITemplate> Load()
        {
            var templates = new List<ITemplate>();
            var property = Collection.Resolve("String");

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