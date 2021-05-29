using System.Threading.Tasks;

namespace Edelstein.Protocol.Util.Ticks
{
    public interface ITickerBehavior
    {
        Task Tick();
    }
}
