using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Core.Scripting;
using Edelstein.Core.Utils.Messaging;
using Edelstein.Database;
using Edelstein.Provider;
using Edelstein.Service.Game;
using Edelstein.Service.Login;
using Edelstein.Service.Shop;
using Foundatio.Caching;
using Foundatio.Lock;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Edelstein.Service.All
{
    public class ContainerService : IHostedService
    {
        private ContainerServiceState _state;
        private IServiceProvider _provider;

        public List<IHostedService> Services { get; }

        public ContainerService(IOptions<ContainerServiceState> state, IServiceProvider provider)
        {
            _state = state.Value;
            _provider = provider;

            Services = new List<IHostedService>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Services.AddRange(_state.LoginServices
                .Select(i => new LoginService(
                    Options.Create(i),
                    _provider.GetService<IDataStore>(),
                    _provider.GetService<ICacheClient>(),
                    _provider.GetService<IMessageBusFactory>(),
                    _provider.GetService<ILockProvider>(),
                    _provider.GetService<IDataTemplateManager>()
                )));
            Services.AddRange(_state.GameServices
                .Select(i => new GameService(
                    Options.Create(i),
                    _provider.GetService<IDataStore>(),
                    _provider.GetService<ICacheClient>(),
                    _provider.GetService<IMessageBusFactory>(),
                    _provider.GetService<ILockProvider>(),
                    _provider.GetService<IDataTemplateManager>(),
                    _provider.GetService<IScriptManager>()
                )));
            Services.AddRange(_state.ShopServices
                .Select(i => new ShopService(
                    Options.Create(i),
                    _provider.GetService<IDataStore>(),
                    _provider.GetService<ICacheClient>(),
                    _provider.GetService<IMessageBusFactory>(),
                    _provider.GetService<ILockProvider>(),
                    _provider.GetService<IDataTemplateManager>()
                )));

            await Task.WhenAll(Services.Select(s => s.StartAsync(cancellationToken)));
        }

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.WhenAll(Services.Select(s => s.StopAsync(cancellationToken)));
    }
}