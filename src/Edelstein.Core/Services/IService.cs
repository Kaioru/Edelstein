using System.Threading.Tasks;

namespace Edelstein.Core.Services
{
    public interface IService
    {
        Task Start();
        Task Stop();
    }
}