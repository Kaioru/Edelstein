using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edelstein.Core.Provider
{
    public interface IDataTemplateCollection
    {
        IDataTemplate Get(int id);
        IEnumerable<IDataTemplate> GetAll();

        Task<IDataTemplate> GetAsync(int id);
        Task<IEnumerable<IDataTemplate>> GetAllAsync();
    }
}