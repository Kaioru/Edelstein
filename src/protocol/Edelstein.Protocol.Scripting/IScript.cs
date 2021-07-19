using System.Threading.Tasks;

namespace Edelstein.Protocol.Scripting
{
    public interface IScript
    {
        Task<object> Evaluate(IScriptScope scope);
        Task<IScriptState> Run(IScriptScope scope);
    }
}
