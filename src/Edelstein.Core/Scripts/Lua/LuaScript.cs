using System;
using System.Threading;
using System.Threading.Tasks;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;

namespace Edelstein.Core.Scripts.Lua
{
    public class LuaScript : IScript
    {
        private readonly string _path;
        private readonly Script _script;

        public LuaScript(string path, string searchPath)
        {
            _path = path;
            _script = new Script
            {
                Options =
                {
                    ScriptLoader = new FileSystemScriptLoader
                    {
                        IgnoreLuaPathGlobal = true,
                        ModulePaths = new[] {$"{searchPath}/?", $"{searchPath}/?.lua"}
                    }
                }
            };
        }

        public Task Register(string key, object value)
        {
            _script.Globals[key] = value;
            return Task.CompletedTask;
        }

        public Task Start(CancellationToken token)
            => Task.Run(() => _script.DoFile(_path), token);
    }
}