using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Util.Ticks;

namespace Playground
{
    public class TestingTickerBehavior : ITickerBehavior
    {
        public Task OnTick(DateTime now)
        {
            Console.WriteLine($"Ticked at {now}");
            return Task.Delay(10);
        }
    }
}
