using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Services;
using Edelstein.Data.Context;
using Edelstein.Provider.Templates;
using Edelstein.Service.Game;
using Edelstein.Service.Game.Conversations.Scripts;
using Edelstein.Service.Login;
using Edelstein.Service.Shop;
using Foundatio.Caching;
using Foundatio.Lock;
using Foundatio.Messaging;
using MoreLinq;

namespace Edelstein.Service.All
{
    public class WvsContainer : IService
    {
        private readonly WvsContainerOptions _options;
        private readonly ICacheClient _cache;
        private readonly IMessageBus _messageBus;
        private readonly ILockProvider _lockProvider;
        private readonly IDataContextFactory _dataContextFactory;
        private readonly ITemplateManager _templateManager;
        private readonly IScriptConversationManager _conversationManager;
        private readonly ICollection<IService> _services;

        public WvsContainer(
            WvsContainerOptions options,
            ICacheClient cache,
            IMessageBus messageBus,
            ILockProvider lockProvider,
            IDataContextFactory dataContextFactory,
            ITemplateManager templateManager, IScriptConversationManager conversationManager)
        {
            _options = options;
            _cache = cache;
            _messageBus = messageBus;
            _lockProvider = lockProvider;
            _dataContextFactory = dataContextFactory;
            _templateManager = templateManager;
            _conversationManager = conversationManager;
            _services = new List<IService>();
        }

        public Task Start()
        {
            _options.LoginServices
                .Select(o => new WvsLogin(o, _cache, _messageBus, _lockProvider, _dataContextFactory, _templateManager))
                .ForEach(_services.Add);
            _options.GameServices
                .Select(o => new WvsGame(o, _cache, _messageBus, _dataContextFactory, _templateManager, _conversationManager))
                .ForEach(_services.Add);
            _options.ShopServices
                .Select(o => new WvsShop(o, _cache, _messageBus, _dataContextFactory, _templateManager))
                .ForEach(_services.Add);
            return Task.WhenAll(_services.Select(s => s.Start()));
        }

        public Task Stop()
            => Task.WhenAll(_services.Select(s => s.Stop()));
    }
}