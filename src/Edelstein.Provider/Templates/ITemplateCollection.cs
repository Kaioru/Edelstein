using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edelstein.Provider.Templates
{
    public interface ITemplateCollection
    {
        IEnumerable<ITemplate> Cache { get; }

        ITemplate Get(int id);
        IEnumerable<ITemplate> GetAll();
        Task<ITemplate> GetAsync(int id);
    }
}