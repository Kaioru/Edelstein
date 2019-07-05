using System;
using System.Threading.Tasks;
using Edelstein.Core.Distributed;
using Edelstein.Core.Utils.Messaging;
using Edelstein.Database.Store;
using Edelstein.Service.WebAPI.Logging;
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
    public class WebAPIService : AbstractPeerService<WebAPIInfo>
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
        
        public IHost WebHost { get; set; }
        public IDataStore DataStore { get; }

        public WebAPIService(
            IOptions<WebAPIInfo> info,
            ICacheClient cacheClient,
            IMessageBusFactory messageBusFactory, 
            IDataStore dataStore
        ) : base(info.Value, cacheClient, messageBusFactory)
        {
            DataStore = dataStore;
        }

        public override async Task OnStart()
        {
            WebHost = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder => webBuilder
                    .UseStartup<WebAPIStartup>()
                    .UseSerilog()
                    .ConfigureServices((context, collection) => collection.AddSingleton(f => this))
                    .ConfigureServices((context, collection) =>
                    {
                        collection.AddControllers();
                        collection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                            .AddJwtBearer(options =>
                            {
                                var signingKey = Convert.FromBase64String(Info.TokenKey);
                                options.TokenValidationParameters = new TokenValidationParameters
                                {
                                    ValidateIssuer = false,
                                    ValidateAudience = false,
                                    ValidateIssuerSigningKey = true,
                                    IssuerSigningKey = new SymmetricSecurityKey(signingKey)
                                };
                            });
                    }))
                .Build();
            
            await base.OnStart();
            await WebHost.StartAsync();
        }

        public override async Task OnStop()
        {
            await base.OnStop();
            await WebHost.StopAsync();
        }

        public override Task OnTick(DateTime now)
            => Task.CompletedTask;
    }
}