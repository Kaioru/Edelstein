using System.Threading.Tasks;
using MoonSharp.Interpreter;
using MoreLinq.Extensions;

namespace Edelstein.Core.Scripting.Lua
{
    public class LuaScript : AbstractScript
    {
        private readonly Script _code;
        private readonly string _path;

        public LuaScript(Script code, string path)
        {
            _code = code;
            _path = path;
        }

        public override async Task<dynamic> Run()
        {
            All().ForEach(kv => _code.Globals[kv.Key] = kv.Value);
            return _code.DoFile(_path);
        }
    }
}