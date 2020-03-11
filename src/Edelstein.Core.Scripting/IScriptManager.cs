using System.Threading.Tasks;

namespace Edelstein.Core.Scripting
{
    public interface IScriptManager
    {
        Task<IScript> Build(string script);
    }
}