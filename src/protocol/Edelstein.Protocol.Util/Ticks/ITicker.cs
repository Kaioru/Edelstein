using System.Threading.Tasks;

namespace Edelstein.Protocol.Util.Ticks
{
    public interface ITicker
    {
        Task Start();
        Task Stop();

        Task Tick();
    }
}
