using Edelstein.Core.Scripting;
using Edelstein.Core.Scripting.Python;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Edelstein.Core.Bootstrap.Providers.Scripting
{
    public class PythonScriptingProvider : IProvider
    {
        private readonly string _path;

        public PythonScriptingProvider(string path)
            => _path = path;

        public void Provide(HostBuilderContext context, IServiceCollection collection)
        {
            collection.AddSingleton<IScriptManager, PythonScriptManager>(f =>
                new PythonScriptManager(_path)
            );
        }
    }
}