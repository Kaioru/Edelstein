using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpx;
using Edelstein.Core.Services;
using Edelstein.Core.Services.Info;
using Edelstein.Data.Context;
using Edelstein.Provider.Templates;
using Edelstein.Service.Game;
using Edelstein.Service.Game.Conversations.Scripts;
using Edelstein.Service.Login;
using Edelstein.Service.Shop;
using Edelstein.Service.Trade;
using Foundatio.Caching;
using Foundatio.Lock;
using Foundatio.Messaging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Edelstein.Service.All
{
    public class WvsContainer : AbstractService<WvsContainerOptions>
    {
        private readonly CancellationTokenSource _token;
        private readonly ICollection<IService> _services;

        public WvsContainer(
            IApplicationLifetime appLifetime,
            ICacheClient cache,
            IMessageBus messageBus,
            ILockProvider lockProvider,
            IOptions<WvsContainerOptions> info,
            IDataContextFactory dataContextFactory,
            ITemplateManager templateManager,
            IScriptConversationManager conversationManager
        ) : base(appLifetime, cache, messageBus, info, dataContextFactory)
        {
            _token = new CancellationTokenSource();
            _services = new List<IService>();

            Info.LoginServices
                .Select(o => new WvsLogin(
                    appLifetime,
                    cache,
                    messageBus,
                    new OptionsWrapper<LoginServiceInfo>(o),
                    dataContextFactory,
                    lockProvider,
                    templateManager
                ))
                .ForEach(_services.Add);
            Info.GameServices
                .Select(o => new WvsGame(
                    appLifetime,
                    cache,
                    messageBus,
                    new OptionsWrapper<GameServiceInfo>(o),
                    dataContextFactory,
                    templateManager,
                    conversationManager
                ))
                .ForEach(_services.Add);
            Info.ShopServices
                .Select(o => new WvsShop(
                    appLifetime,
                    cache,
                    messageBus,
                    new OptionsWrapper<ShopServiceInfo>(o),
                    dataContextFactory,
                    templateManager
                ))
                .ForEach(_services.Add);
            Info.TradeServices
                .Select(o => new WvsTrade(
                    appLifetime,
                    cache,
                    messageBus,
                    new OptionsWrapper<TradeServiceInfo>(o),
                    dataContextFactory,
                    templateManager
                ))
                .ForEach(_services.Add);
        }

        public override Task OnUpdate()
            => Task.WhenAll(_services.Select(s => s.OnUpdate()));

        protected override Task OnStarted()
            => Task.WhenAll(_services.Select(s => s.StartAsync(_token.Token)));

        protected override Task OnStopping()
            => Task.WhenAll(_services.Select(s => s.StopAsync(_token.Token)));
    }
}