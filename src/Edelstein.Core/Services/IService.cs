using Microsoft.Extensions.Hosting;

namespace Edelstein.Core.Services
{
    public interface IService : IUpdateable, IHostedService
    {
    }
}