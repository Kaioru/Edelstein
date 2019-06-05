using System;
using System.Threading.Tasks;
using Edelstein.Core.Distributed;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Core.Utils.Messaging;
using Foundatio.Caching;
using Microsoft.Extensions.Options;
using Nancy.Hosting.Self;

namespace Edelstein.Service.WebAPI
{
    public class WebAPIService : AbstractPeerService<ServerServiceInfo>
    {
        private readonly ServerServiceInfo _info;
        private readonly NancyHost _host;

        public WebAPIService(
            IOptions<ServerServiceInfo> info,
            ICacheClient cacheClient,
            IMessageBusFactory messageBusFactory
        ) : base(info.Value, cacheClient, messageBusFactory)
        {
            _info = info.Value;
            _host = new NancyHost(new Uri($"http://{_info.Host}:{_info.Port}"));
        }

        public override async Task OnStart()
        {
            _host.Start();
            await base.OnStart();
        }

        public override async Task OnStop()
        {
            _host.Stop();
            await base.OnStop();
        }

        public override Task OnTick(DateTime now)
            => Task.CompletedTask;
    }
}