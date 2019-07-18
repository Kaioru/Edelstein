using Edelstein.Core.Scripts;
using Edelstein.Core.Scripts.Python;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Edelstein.Core.Bootstrap.Providers.Script
{
    public class PythonScriptProvider : AbstractScriptProvider
    {
        public PythonScriptProvider(string path) : base(path)
        {
        }

        public override void Provide(HostBuilderContext context, IServiceCollection collection)
        {
            collection.AddSingleton<IScriptManager>(
                new PythonScriptManager(Path)
            );
        }
    }
}