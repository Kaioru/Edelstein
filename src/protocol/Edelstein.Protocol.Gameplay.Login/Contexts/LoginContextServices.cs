using Edelstein.Protocol.Services.Auth;
using Edelstein.Protocol.Services.Server;

namespace Edelstein.Protocol.Gameplay.Login.Contexts;

public record LoginContextServices(
    IAuthService Auth,
    IServerService Server,
    ISessionService Session,
    IMigrationService Migration
);
