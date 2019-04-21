using System.IO;
using System.Threading.Tasks;
using Microsoft.Scripting.Hosting;

namespace Edelstein.Core.Scripts.Python
{
    public class PythonScriptManager : IScriptManager
    {
        private const string Extension = "py";

        private readonly ScriptEngine _engine;
        private readonly string _path;

        public PythonScriptManager(string path)
        {
            _path = path;
            _engine = IronPython.Hosting.Python.CreateEngine();
        }

        public Task<IScript> Build(string script)
        {
            var path = Path.Join(_path, Path.ChangeExtension(script, Extension));

            if (!File.Exists(path)) throw new FileNotFoundException(path);

            var source = _engine.CreateScriptSourceFromFile(path);
            var code = source.Compile();
            var scope = code.DefaultScope;

            return Task.FromResult<IScript>(new PythonScript(code, scope));
        }
    }
}