using System.Threading.Tasks;
using Microsoft.Scripting.Hosting;
using MoreLinq;

namespace Edelstein.Core.Scripting.Python
{
    public class PythonScript : IScript
    {
        private readonly CompiledCode _code;

        public PythonScript(CompiledCode code)
            => _code = code;

        public Task<object> Run(IScriptContext context)
        {
            var scope = _code.DefaultScope;

            context.All().ForEach(kv => scope.SetVariable(kv.Key, kv.Value));
            return Task.Run(_code.Execute());
        }
    }
}