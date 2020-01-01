using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Provider;

namespace Edelstein.Core.Templates.Etc.SetItemInfo
{
    public class SetItemInfoTemplateCollection : AbstractEagerDataTemplateCollection
    {
        private readonly IDataDirectoryCollection _collection;

        public SetItemInfoTemplateCollection(IDataDirectoryCollection collection)
            => _collection = collection;

        protected override async Task<IEnumerable<IDataTemplate>> Load()
        {
            var property = _collection.Resolve("Etc/SetItemInfo.img");

            return property.Children
                .Select(p => new SetItemInfoTemplate(
                    Convert.ToInt32(p.Name),
                    p.ResolveAll()
                ));
        }
    }
}