using System.Threading.Tasks;
using Edelstein.Core.Services;
using Edelstein.Core.Services.Info;

namespace Edelstein.Service.Login
{
    internal static class Program
    {
        private static Task Main(string[] args)
            => new Startup()
                .WithConfig()
                .WithLogger()
                .WithInferredModel()
                .WithInferredDatabase()
                .WithInferredProvider()
                .WithServiceOption<LoginServiceInfo>()
                .WithService<WvsLogin>()
                .Start();
    }
}