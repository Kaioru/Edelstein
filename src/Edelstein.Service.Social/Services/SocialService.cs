using System;
using System.Threading.Tasks;
using Edelstein.Core.Distributed;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Core.Gameplay.Social.Messages;
using Edelstein.Core.Utils.Messaging;
using Edelstein.Database.Store;
using Edelstein.Service.Social.Logging;
using Edelstein.Service.Social.Managers;
using Edelstein.Service.Social.Managers.Party;
using Foundatio.Caching;
using Foundatio.Messaging;
using Microsoft.Extensions.Options;

namespace Edelstein.Service.Social.Services
{
    public partial class SocialService : AbstractPeerService<SocialServiceInfo>
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
        
        public IDataStore DataStore { get; }
        public RankingManager RankingManager { get; }
        public PartyManager PartyManager { get; }

        public SocialService(
            IOptions<SocialServiceInfo> info,
            ICacheClient cacheClient,
            IMessageBusFactory messageBusFactory,
            IDataStore dataStore
        ) : base(info.Value, cacheClient, messageBusFactory)
        {
            DataStore = dataStore;
            RankingManager = new RankingManager(this);
            PartyManager = new PartyManager(dataStore);

            MessageBus.SubscribeAsync<SocialStateMessage>(HandleSocialUpdateState);
            MessageBus.SubscribeAsync<SocialLevelMessage>(HandleSocialUpdateLevel);
            MessageBus.SubscribeAsync<SocialJobMessage>(HandleSocialUpdateJob);
        }

        public override async Task OnTick(DateTime now)
        {
            await RankingManager.OnTick(now);
        }
    }
}