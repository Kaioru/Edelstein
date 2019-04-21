using System.Threading;
using System.Threading.Tasks;

namespace Edelstein.Core.Scripts
{
    public interface IScript
    {
        Task Register(string key, object value);
        Task Start(CancellationToken token);
    }
}