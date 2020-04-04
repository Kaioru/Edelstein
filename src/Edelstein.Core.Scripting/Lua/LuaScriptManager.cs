using System.IO;
using System.Threading.Tasks;
using Edelstein.Core.Scripting.Python;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;
using MoonSharp.Interpreter.Loaders;

namespace Edelstein.Core.Scripting.Lua
{
    public class LuaScriptManager : IScriptManager
    {
        private readonly string _path;

        public LuaScriptManager(string path)
        {
            UserData.RegistrationPolicy = InteropRegistrationPolicy.Automatic;

            _path = path;
        }

        public Task<IScript> Build(string script)
        {
            var path = Path.Join(_path, Path.ChangeExtension(script, "lua"));

            if (!File.Exists(path)) throw new FileNotFoundException(path);

            var code = new Script
            {
                Options =
                {
                    ScriptLoader = new FileSystemScriptLoader
                    {
                        IgnoreLuaPathGlobal = true,
                        ModulePaths = new[] {$"{_path}/?", $"{_path}/?.lua"}
                    }
                }
            };

            return Task.FromResult<IScript>(new LuaScript(code, path));
        }
    }
}