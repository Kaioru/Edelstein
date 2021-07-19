using System.Threading.Tasks;

namespace Edelstein.Protocol.Scripting
{
    public interface IScript : IScriptState
    {
        Task<IScriptState> Run();
    }
}
