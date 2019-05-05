using System;
using System.Threading.Tasks;

namespace Edelstein.Core.Utils
{
    public interface ITickable
    {
        Task OnTick(DateTime now);
    }
}