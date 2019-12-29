using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Core.Utils.Messaging;
using Edelstein.Database;
using Edelstein.Service.Game.Services;
using Edelstein.Service.Login;
using Foundatio.Caching;
using Foundatio.Lock;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Edelstein.Service.All.Services
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
                    _provider.GetService<ILockProvider>()
                )));
            Services.AddRange(_state.GameServices
                .Select(i => new GameService(
                    Options.Create(i),
                    _provider.GetService<IDataStore>(),
                    _provider.GetService<ICacheClient>(),
                    _provider.GetService<IMessageBusFactory>()
                )));

            await Task.WhenAll(Services.Select(s => s.StartAsync(cancellationToken)));
        }

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.WhenAll(Services.Select(s => s.StopAsync(cancellationToken)));
    }
}