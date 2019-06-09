using System.Collections.Generic;
using Edelstein.Provider.Parsing;

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