using System.IO;
using System.Threading.Tasks;

namespace Edelstein.Core.Scripts.Lua
{
    public class LuaScriptManager : IScriptManager
    {
        private const string Extension = "lua";
        private readonly string _path;

        public LuaScriptManager(string path)
        {
            _path = path;
        }

        public Task<IScript> Build(string script)
        {
            var path = Path.Join(_path, Path.ChangeExtension(script, Extension));
            
            if (!File.Exists(path)) throw new FileNotFoundException(path);

            return Task.FromResult<IScript>(new LuaScript(path));
        }
    }
}