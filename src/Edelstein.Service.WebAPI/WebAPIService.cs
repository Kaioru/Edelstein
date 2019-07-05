using System;
using System.Threading.Tasks;
using Edelstein.Core.Distributed;
using Edelstein.Core.Utils.Messaging;
using Edelstein.Database.Store;
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

        public override Task OnStart()
        {
            WebHost = Host.CreateDefaultBuilder(new string[]{})
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
            return WebHost.StartAsync();
        }

        public override Task OnStop()
        {
            return WebHost?.StopAsync();
        }

        public override Task OnTick(DateTime now)
            => Task.CompletedTask;
    }
}