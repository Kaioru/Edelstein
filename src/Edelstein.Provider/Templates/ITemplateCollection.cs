using System.Collections.Generic;

namespace Edelstein.Provider.Templates
{
    public interface ITemplateCollection
    {
        TemplateCollectionType Type { get; }
        IDataDirectoryCollection Collection { get; }

        ITemplate Get(int id);
        IEnumerable<ITemplate> GetAll();
    }
}