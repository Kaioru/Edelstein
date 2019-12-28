using System.Threading.Tasks;

namespace Edelstein.Core.Utils.Ticks
{
    public interface ITicker
    {
        void Start();
        void Stop();
        Task ForceTick();
    }
}