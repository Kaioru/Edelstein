using Edelstein.Common.Services.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

var builder = Host.CreateApplicationBuilder();

builder.Services.AddSerilog((_, logger) 
    => logger.ReadFrom.Configuration(builder.Configuration));

builder.Services.AddDbContextFactory<ServerDbContext>(options 
    => options.UseNpgsql(builder.Configuration.GetConnectionString(ServerDbContext.ConnectionStringKey)));
builder.Services.AddAutoMapper(typeof(ServerDbContext));

var host = builder.Build();

await host.RunAsync();
