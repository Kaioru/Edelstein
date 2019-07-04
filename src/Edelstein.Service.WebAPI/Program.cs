using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Bootstrap;
using Edelstein.Core.Distributed.Peers.Info;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Edelstein.Service.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
            => new Startup(Host.CreateDefaultBuilder(args)
                    .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<WebAPIStartup>(); }))
                .FromConfiguration(args)
                .ForService<WebAPIService, WebAPIInfo>()
                .StartAsync();
    }
}