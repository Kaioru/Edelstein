using Edelstein.Protocol.Gameplay.Stages.Login.Contexts;
using Edelstein.Protocol.Services.Auth;
using Edelstein.Protocol.Services.Migration;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Session;

namespace Edelstein.Common.Gameplay.Stages.Login.Contexts;

public record LoginContextServices(
    IAuthService Auth,
    IServerService Server,
    ISessionService Session,
    IMigrationService Migration
) : ILoginContextServices;
