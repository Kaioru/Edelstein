using System.Threading.Tasks;
using Edelstein.Core.Services;
using Edelstein.Service.Game;

namespace Edelstein.Service.All
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
                .WithInferredScripting()
                .WithServiceOption<WvsContainerOptions>()
                .WithService<WvsContainer>()
                .Start();
    }
}