using System;
using System.Threading.Tasks;

namespace Edelstein.Core.Utils.Ticks
{
    public interface ITickBehavior
    {
        Task TryTick(DateTime now);
    }
}