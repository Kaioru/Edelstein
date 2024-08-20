using Edelstein.Protocol.Services.Auth;
using Edelstein.Protocol.Services.Dispatch;
using Edelstein.Protocol.Services.Migration;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Session;

namespace Edelstein.Protocol.Gameplay.Login.Contexts;

public record LoginContextServices(
    IDispatchService Dispatch,
    IAuthService Auth,
    IServerService Server,
    ISessionService Session,
    IMigrationService Migration
);
