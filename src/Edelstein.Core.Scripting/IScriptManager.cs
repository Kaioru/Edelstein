using System.Threading.Tasks;

namespace Edelstein.Core.Scripting
{
    public interface IScriptManager
    {
        Task<object> Run(string script, IScriptContext context);
    }
}