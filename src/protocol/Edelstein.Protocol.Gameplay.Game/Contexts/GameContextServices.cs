using Edelstein.Protocol.Services.Auth;
using Edelstein.Protocol.Services.Server;

namespace Edelstein.Protocol.Gameplay.Game.Contexts;

public record GameContextServices(
    IAuthService Auth,
    IServerService Server,
    ISessionService Session,
    IMigrationService Migration
);
