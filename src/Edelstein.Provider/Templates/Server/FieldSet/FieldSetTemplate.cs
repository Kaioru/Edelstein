using System.Collections.Generic;
using System.Linq;
using Edelstein.Provider.Parser;

namespace Edelstein.Provider.Templates.Server.FieldSet
{
    public class FieldSetTemplate : ITemplate
    {
        public int ID { get; set; }

        public string Name { get; set; }
        public ICollection<int> Fields { get; set; }

        public static FieldSetTemplate Parse(int id, IDataProperty property)
        {
            var t = new FieldSetTemplate {ID = id};

            property.Resolve(p =>
            {
                t.Name = p.Name;
                t.Fields = p.Children
                    .Where(c => c.Name.All(char.IsDigit))
                    .Select(c => c.Resolve<int>() ?? 999999999)
                    .ToList();
            });

            return t;
        }
    }
}