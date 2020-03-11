using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Scripting.Hosting;

namespace Edelstein.Core.Scripting.Python
{
    public class PythonScriptManager : IScriptManager
    {
        private readonly ScriptEngine _engine;
        private readonly string _path;

        public PythonScriptManager(string path)
        {
            _path = path;
            _engine = IronPython.Hosting.Python.CreateEngine();
            _engine.SetSearchPaths(new List<string> {_path});
        }

        public Task<object> Run(string script, IScriptContext context)
        {
            var path = Path.Join(_path, Path.ChangeExtension(script, "py"));

            if (!File.Exists(path)) throw new FileNotFoundException(path);

            var source = _engine.CreateScriptSourceFromFile(path);
            var code = source.Compile();

            return new PythonScript(code).Run(context);
        }
    }
}