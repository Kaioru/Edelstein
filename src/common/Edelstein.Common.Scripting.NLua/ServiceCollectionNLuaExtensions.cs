using Edelstein.Protocol.Scripting;
using Microsoft.Extensions.DependencyInjection;

namespace Edelstein.Common.Scripting.NLua
{
    public static class ServiceCollectionNLuaExtensions
    {
        public static void AddLuaScripting(this IServiceCollection c, string directory)
        {
            c.AddSingleton<IScriptEngine>(new LuaScriptEngine(directory));
        }
    }
}
