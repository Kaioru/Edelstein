using System.Threading.Tasks;

namespace Edelstein.Core.Scripts
{
    public interface IScriptManager
    {
        Task<IScript> Build(string script);
    }
}