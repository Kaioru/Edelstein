using System;
using System.Threading.Tasks;
using Edelstein.Core.Distributed.Peers;
using Microsoft.Extensions.Hosting;

namespace Edelstein.Core.Distributed
{
    public interface IPeerService : IHostedService
    {
        Task OnStart();
        Task OnStop();
        Task OnTick(DateTime now);

        Task BroadcastMessage<T>(T message) where T : class;
        Task SendMessage<T>(PeerServiceInfo info, T message) where T : class;
    }
}