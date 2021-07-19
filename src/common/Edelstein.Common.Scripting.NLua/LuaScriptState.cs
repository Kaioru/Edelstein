using System.Threading.Tasks;
using Edelstein.Protocol.Scripting;
using NLua;

namespace Edelstein.Common.Scripting.NLua
{
    public class LuaScriptState : IScriptState
    {
        private readonly Lua _state;

        public LuaScriptState(Lua luaState)
            => _state = luaState;

        public Task<object> Call(string name, params object[] args)
        {
            var function = _state[name] as LuaFunction;
            var result = function.Call(args)[0];

            return Task.FromResult(result);
        }

        public Task<object> Evaluate(string source)
            => Task.FromResult(_state.DoString(source)[0]);

        public void Dispose()
            => _state.Dispose();
    }
}
