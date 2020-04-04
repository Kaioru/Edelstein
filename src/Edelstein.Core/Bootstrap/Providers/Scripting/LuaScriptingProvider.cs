using Edelstein.Core.Scripting;
using Edelstein.Core.Scripting.Lua;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Edelstein.Core.Bootstrap.Providers.Scripting
{
    public class LuaScriptingProvider : IProvider
    {
        private readonly string _path;

        public LuaScriptingProvider(string path)
            => _path = path;

        public void Provide(HostBuilderContext context, IServiceCollection collection)
        {
            collection.AddSingleton<IScriptManager, LuaScriptManager>(f =>
                new LuaScriptManager(_path)
            );
        }
    }
}