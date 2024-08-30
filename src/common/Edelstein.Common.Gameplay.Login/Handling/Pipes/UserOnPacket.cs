using Edelstein.Common.Gameplay.Handling.Pipes;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Login;

namespace Edelstein.Common.Gameplay.Login.Handling.Pipes;

public class UserOnPacket(
    IPacketHandlerManager<ILoginStageSystem, ILoginStageSystemUser> manager
) : BaseUserOnPacket<ILoginStageSystem, ILoginStageSystemUser>(manager);
