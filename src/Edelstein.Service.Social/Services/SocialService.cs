using System;
using System.Threading.Tasks;
using Edelstein.Core.Distributed;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Core.Utils.Messaging;
using Foundatio.Caching;

namespace Edelstein.Service.Social.Services
{
    public class SocialService : AbstractPeerService<SocialServiceInfo>
    {
        public SocialService(
            SocialServiceInfo info,
            ICacheClient cacheClient,
            IMessageBusFactory messageBusFactory
        ) : base(info, cacheClient, messageBusFactory)
        {
            // TODO: MessageBus.SubscribeAsync<>();
        }

        public override Task OnTick(DateTime now)
        {
            return Task.CompletedTask;
        }
    }
}