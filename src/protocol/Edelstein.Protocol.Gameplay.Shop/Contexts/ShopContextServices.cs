using Edelstein.Protocol.Services.Auth;
using Edelstein.Protocol.Services.Server;

namespace Edelstein.Protocol.Gameplay.Shop.Contexts;

public record ShopContextServices(
    IAuthService Auth,
    IServerService Server,
    ISessionService Session,
    IMigrationService Migration
);
