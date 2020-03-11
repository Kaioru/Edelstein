using System.Threading.Tasks;

namespace Edelstein.Core.Scripting
{
    public interface IScript
    {
        Task<object> Run(IScriptContext context);
    }
}