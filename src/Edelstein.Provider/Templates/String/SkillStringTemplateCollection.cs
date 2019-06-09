using System;
using System.Collections.Generic;
using System.Linq;
using Edelstein.Provider.Parsing;

namespace Edelstein.Provider.Templates.String
{
    public class SkillStringTemplateCollection : AbstractEagerTemplateCollection
    {
        public override TemplateCollectionType Type => TemplateCollectionType.SkillString;

        public SkillStringTemplateCollection(IDataDirectoryCollection collection) : base(collection)
        {
        }

        protected override IEnumerable<ITemplate> Load()
        {
            var property = Collection.Resolve("String/Skill.img");

            return property.Children
                .Where(c =>
                {
                    var id = Convert.ToInt32(c.Name);
                    return id > 9999 && id < 90000000;
                })
                .Select(c => new SkillStringTemplate(Convert.ToInt32(c.Name), c.ResolveAll()));
        }
    }
}