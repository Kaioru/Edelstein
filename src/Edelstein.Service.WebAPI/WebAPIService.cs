using System;
using System.Threading.Tasks;
using Edelstein.Core.Distributed;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Core.Utils.Messaging;
using Foundatio.Caching;
using Microsoft.Extensions.Options;

namespace Edelstein.Service.WebAPI
{
    public class WebAPIService : AbstractPeerService<WebAPIInfo>
    {
        private readonly WebAPIInfo _info;

        public WebAPIService(
            IOptions<WebAPIInfo> info,
            ICacheClient cacheClient,
            IMessageBusFactory messageBusFactory
        ) : base(info.Value, cacheClient, messageBusFactory)
        {
            _info = info.Value;
        }

        public override Task OnTick(DateTime now)
            => Task.CompletedTask;
    }
}