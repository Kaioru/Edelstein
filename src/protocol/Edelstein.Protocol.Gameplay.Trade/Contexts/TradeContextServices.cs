using Edelstein.Protocol.Services.Auth;
using Edelstein.Protocol.Services.Server;

namespace Edelstein.Protocol.Gameplay.Trade.Contexts;

public record TradeContextServices(
    IAuthService Auth,
    IServerService Server,
    ISessionService Session,
    IMigrationService Migration
);
