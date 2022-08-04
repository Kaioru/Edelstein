using Edelstein.Protocol.Gameplay.Stages.Game.Contexts;
using Edelstein.Protocol.Services.Auth;
using Edelstein.Protocol.Services.Migration;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Session;

namespace Edelstein.Common.Gameplay.Stages.Game.Contexts;

public record GameContextServices(
    IAuthService Auth,
    IServerService Server,
    ISessionService Session,
    IMigrationService Migration
) : IGameContextServices;
