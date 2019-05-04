using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Baseline;
using Edelstein.Core.Distributed;
using Edelstein.Core.Scripts;
using Edelstein.Core.Utils.Messaging;
using Edelstein.Database;
using Edelstein.Provider.Templates;
using Edelstein.Service.Game.Services;
using Edelstein.Service.Login.Services;
using Edelstein.Service.Shop.Services;
using Edelstein.Service.Trade.Services;
using Foundatio.Caching;
using Foundatio.Lock;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Edelstein.Service.All
{
    public class ContainerService : IHostedService
    {
        private readonly ContainerServiceInfo _info;
        private readonly IServiceProvider _serviceProvider;
        private readonly IList<IPeerService> _services;

        public ContainerService(IOptions<ContainerServiceInfo> info, IServiceProvider serviceProvider)
        {
            _info = info.Value;
            _serviceProvider = serviceProvider;
            _services = new List<IPeerService>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _services.AddRange(_info.LoginServices
                .Select(i => new LoginService(
                    Options.Create(i),
                    _serviceProvider.GetService<ICacheClient>(),
                    _serviceProvider.GetService<IMessageBusFactory>(),
                    _serviceProvider.GetService<IDataStore>(),
                    _serviceProvider.GetService<ILockProvider>(),
                    _serviceProvider.GetService<ITemplateManager>()
                )));
            _services.AddRange(_info.GameServices
                .Select(i => new GameService(
                    Options.Create(i),
                    _serviceProvider.GetService<ICacheClient>(),
                    _serviceProvider.GetService<IMessageBusFactory>(),
                    _serviceProvider.GetService<IDataStore>(),
                    _serviceProvider.GetService<ITemplateManager>(),
                    _serviceProvider.GetService<IScriptManager>()
                )));
            _services.AddRange(_info.ShopServices
                .Select(i => new ShopService(
                    Options.Create(i),
                    _serviceProvider.GetService<ICacheClient>(),
                    _serviceProvider.GetService<IMessageBusFactory>(),
                    _serviceProvider.GetService<IDataStore>(),
                    _serviceProvider.GetService<ITemplateManager>()
                )));
            _services.AddRange(_info.TradeServices
                .Select(i => new TradeService(
                    Options.Create(i),
                    _serviceProvider.GetService<ICacheClient>(),
                    _serviceProvider.GetService<IMessageBusFactory>(),
                    _serviceProvider.GetService<IDataStore>()
                )));

            await Task.WhenAll(_services.Select(s => s.StartAsync(cancellationToken)));
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.WhenAll(_services.Select(s => s.StopAsync(cancellationToken)));
        }
    }
}