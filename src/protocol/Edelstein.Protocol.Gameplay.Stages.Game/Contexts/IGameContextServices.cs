using Edelstein.Protocol.Services.Auth;
using Edelstein.Protocol.Services.Migration;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Session;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Contexts;

public interface IGameContextServices
{
    IAuthService Auth { get; }
    IServerService Server { get; }
    ISessionService Session { get; }
    IMigrationService Migration { get; }
}
