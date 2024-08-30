using Edelstein.Common.Gameplay.Handling.Packets;
using Edelstein.Protocol.Gameplay.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Packets;

public class MigrateInHandler(
    IPipeline<PipedPacketMessage<IGameStageSystem, IGameStageSystemUser, MigrateIn>> pipeline
) : BaseMigrateInHandler<IGameStageSystem, IGameStageSystemUser>(pipeline);
