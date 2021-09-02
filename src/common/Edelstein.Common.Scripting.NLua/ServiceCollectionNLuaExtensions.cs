using Edelstein.Protocol.Scripting;
using Microsoft.Extensions.DependencyInjection;

namespace Edelstein.Common.Scripting.NLua
{
    public static class ServiceCollectionNLuaExtensions
    {
        public static void AddLuaScripting(this IServiceCollection c)
        {
            c.AddSingleton<IScriptEngine, LuaScriptEngine>();
        }
    }
}
