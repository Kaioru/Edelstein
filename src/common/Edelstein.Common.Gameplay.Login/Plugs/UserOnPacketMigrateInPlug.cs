using Edelstein.Common.Gameplay.Plugs;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Services.Migration;
using Edelstein.Protocol.Services.Session;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Login.Plugs;

public class UserOnPacketMigrateInPlug : AbstractUserOnPacketMigrateInPlug<ILoginStage, ILoginStageUser>
{
    public UserOnPacketMigrateInPlug(ILogger<AbstractUserOnPacketMigrateInPlug<ILoginStage, ILoginStageUser>> logger, ILoginStage stage, IMigrationService migrationService, ISessionService sessionService) : base(logger, stage, migrationService, sessionService)
    {
    }
}
