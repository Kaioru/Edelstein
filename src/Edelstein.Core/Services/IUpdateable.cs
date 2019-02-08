using System.Threading.Tasks;

namespace Edelstein.Core.Services
{
    public interface IUpdateable
    {
        Task OnUpdate();
    }
}