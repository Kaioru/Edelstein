using Edelstein.Common.Gameplay.Handling.Pipes;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Handling;

namespace Edelstein.Common.Gameplay.Game.Handling.Pipes;

public class UserOnPacket(
    IPacketHandlerManager<IGameStageSystem, IGameStageSystemUser> manager
) : BaseUserOnPacket<IGameStageSystem, IGameStageSystemUser>(manager);
