using System;
using System.Threading.Tasks;
using Edelstein.Core.Distributed;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Core.Gameplay.Social.Messages;
using Edelstein.Core.Types;
using Edelstein.Core.Utils.Messaging;
using Edelstein.Database.Store;
using Edelstein.Service.Social.Managers;
using Foundatio.Caching;
using Microsoft.Extensions.Options;

namespace Edelstein.Service.Social.Services
{
    public class SocialService : AbstractPeerService<SocialServiceInfo>
    {
        public IDataStore DataStore { get; }
        public RankingManager RankingManager { get; }

        public SocialService(
            IOptions<SocialServiceInfo> info,
            ICacheClient cacheClient,
            IMessageBusFactory messageBusFactory,
            IDataStore dataStore
        ) : base(info.Value, cacheClient, messageBusFactory)
        {
            DataStore = dataStore;
            RankingManager = new RankingManager(this);

            MessageBus.SubscribeAsync<SocialUpdateStateMessage>(async (message, token) =>
            {
                // TODO
                Console.WriteLine($"{message.CharacterID} is now {message.State} at {message.Service}");
            });

            MessageBus.SubscribeAsync<SocialUpdateLevelMessage>(async (message, token) =>
            {
                Console.WriteLine($"{message.CharacterID} is now level {message.Level}");
            });

            MessageBus.SubscribeAsync<SocialUpdateJobMessage>(async (message, token) =>
            {
                Console.WriteLine($"{message.CharacterID} is now a {(Job) message.Job}");
            });
        }

        public override async Task OnTick(DateTime now)
        {
            await RankingManager.OnTick(now);
        }
    }
}