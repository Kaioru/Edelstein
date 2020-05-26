using System;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Core.Database;
using Edelstein.Core.Distributed.States;
using Edelstein.Core.Gameplay.Migrations;
using Edelstein.Core.Services;
using Edelstein.Core.Utils.Messaging;
using Foundatio.Caching;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Edelstein.Service.WebAPI
{
    public class WebAPIService : NodeService<DefaultNodeState>
    {
        public WebAPIConfig Config { get; }
        public IDataStore DataStore { get; }
        public ICacheClient AccountStateCache { get; }
        public ICacheClient CharacterStateCache { get; }
        public ICacheClient SocketCountCache { get; }

        public IHost WebHost { get; set; }

        public WebAPIService(
            IOptions<DefaultNodeState> state,
            IOptions<WebAPIConfig> config,
            IMessageBusFactory busFactory,
            IDataStore dataStore,
            ICacheClient cache
        ) : base(state.Value, busFactory)
        {
            Config = config.Value;
            DataStore = dataStore;
            AccountStateCache = new ScopedCacheClient(cache, MigrationScopes.StateAccount);
            CharacterStateCache = new ScopedCacheClient(cache, MigrationScopes.StateCharacter);
            SocketCountCache = new ScopedCacheClient(cache, MigrationScopes.NodeSocketCount);
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            WebHost = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder => webBuilder
                    .UseSerilog()
                    .ConfigureKestrel(context => context.AllowSynchronousIO = true)
                    .ConfigureServices((context, collection) => collection.AddSingleton(f => this))
                    .ConfigureServices((context, collection) =>
                    {
                        collection
                            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                            .AddJwtBearer(options =>
                            {
                                var signingKey = Convert.FromBase64String(Config.TokenKey);

                                options.TokenValidationParameters = new TokenValidationParameters
                                {
                                    ValidateIssuer = !string.IsNullOrEmpty(Config.TokenIssuer),
                                    ValidateAudience = !string.IsNullOrEmpty(Config.TokenAudience),
                                    ValidateIssuerSigningKey = true,
                                    ValidIssuer = Config.TokenIssuer,
                                    ValidAudience = Config.TokenAudience,
                                    IssuerSigningKey = new SymmetricSecurityKey(signingKey)
                                };
                            });
                    })
                    .UseStartup<WebAPIStartup>())
                .Build();

            await base.StartAsync(cancellationToken);
            await WebHost.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
            await WebHost.StopAsync(cancellationToken);
        }
    }
}