using System.Threading;
using System.Threading.Tasks;
using Microsoft.Scripting.Hosting;

namespace Edelstein.Core.Scripts.Python
{
    public class PythonScript : IScript
    {
        private readonly CompiledCode _code;
        private readonly ScriptScope _scope;

        public PythonScript(CompiledCode code, ScriptScope scope)
        {
            _code = code;
            _scope = scope;
        }

        public Task Register(string key, object value)
        {
            _scope.SetVariable(key, value);
            return Task.CompletedTask;
        }

        public Task Start(CancellationToken token)
            => Task.Run(() => _code.Execute(), token);
    }
}