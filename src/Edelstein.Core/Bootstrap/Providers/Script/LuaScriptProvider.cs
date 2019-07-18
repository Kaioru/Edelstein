using Edelstein.Core.Scripts;
using Edelstein.Core.Scripts.Lua;
using Edelstein.Core.Scripts.Python;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Edelstein.Core.Bootstrap.Providers.Script
{
    public class LuaScriptProvider : AbstractScriptProvider
    {
        public LuaScriptProvider(string path) : base(path)
        {
        }

        public override void Provide(HostBuilderContext context, IServiceCollection collection)
        {
            collection.AddSingleton<IScriptManager>(
                new LuaScriptManager(Path)
            );
        }
    }
}