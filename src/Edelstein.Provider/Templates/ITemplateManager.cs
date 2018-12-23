using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edelstein.Provider.Templates
{
    public interface ITemplateManager
    {
        T Get<T>(int id);
        IEnumerable<T> GetAll<T>();
        Task<T> GetAsync<T>(int id);
        Task Load();
    }
}