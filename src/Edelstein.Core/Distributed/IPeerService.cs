using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Edelstein.Core.Distributed
{
    public interface IPeerService : IHostedService
    {
        Task OnStart();
        Task OnStop();
        Task OnMessage(object msg);
    }
}