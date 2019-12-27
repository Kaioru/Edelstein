using System.Threading.Tasks;

namespace Edelstein.Core.Utils.Ticks
{
    public interface ITickBehavior
    {
        Task<bool> TryTick();
    }
}