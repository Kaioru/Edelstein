using System.Threading.Tasks;
using Microsoft.Scripting.Hosting;
using MoreLinq;

namespace Edelstein.Core.Scripting.Python
{
    public class PythonScript : AbstractScript
    {
        private readonly CompiledCode _code;

        public PythonScript(CompiledCode code)
            => _code = code;

        public override async Task<dynamic> Run()
        {
            var scope = _code.DefaultScope;
            All().ForEach(kv => scope.SetVariable(kv.Key, kv.Value));
            return _code.Execute();
        }
    }
}