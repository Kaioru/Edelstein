using System.Threading.Tasks;

namespace Edelstein.Provider.Templates
{
    public interface ITemplateManager
    {
        T Get<T>(int id);
        Task<T> GetAsync<T>(int id);
        Task Load();
    }
}