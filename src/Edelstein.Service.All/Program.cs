using System.Threading.Tasks;
using Edelstein.Core.Bootstrap;

namespace Edelstein.Service.All
{
    internal static class Program
    {
        private static Task Main(string[] args)
            => new Startup()
                .Start<ContainerService, ContainerServiceInfo>(args);
    }
}