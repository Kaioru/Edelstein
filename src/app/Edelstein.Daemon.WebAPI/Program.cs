using Edelstein.Common.Gameplay.Accounts;
using Edelstein.Common.Gameplay.Characters;
using Edelstein.Common.Gameplay.Database;
using Edelstein.Common.Gameplay.Database.Repositories;
using Edelstein.Common.Services.Auth;
using Edelstein.Common.Services.Server;
using Edelstein.Common.Util.Serializers;
using Edelstein.Protocol.Services.Auth;
using Edelstein.Protocol.Services.Migration;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Session;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, logger) => logger.ReadFrom.Configuration(ctx.Configuration));

builder.Services.AddPooledDbContextFactory<AuthDbContext>(o =>
    o.UseNpgsql(builder.Configuration.GetConnectionString(AuthDbContext.ConnectionStringKey)));
builder.Services.AddPooledDbContextFactory<ServerDbContext>(o =>
    o.UseNpgsql(builder.Configuration.GetConnectionString(ServerDbContext.ConnectionStringKey)));
builder.Services.AddPooledDbContextFactory<GameplayDbContext>(o =>
    o.UseNpgsql(builder.Configuration.GetConnectionString(GameplayDbContext.ConnectionStringKey)));

builder.Services.AddSingleton<ISerializer, BinarySerializer>();

builder.Services.AddSingleton<IAccountRepository, AccountRepository>();
builder.Services.AddSingleton<IAccountWorldRepository, AccountWorldRepository>();
builder.Services.AddSingleton<ICharacterRepository, CharacterRepository>();

builder.Services.AddSingleton<IAuthService, AuthService>();
builder.Services.AddSingleton<IServerService, ServerService>();
builder.Services.AddSingleton<ISessionService, SessionService>();
builder.Services.AddSingleton<IMigrationService, MigrationService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
