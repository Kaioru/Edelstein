using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Contexts;
using Edelstein.Protocol.Gameplay.Stages.Messages;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Game.Contexts;

public record GameContextPipelines(
    IPipeline<ISocketOnPacket<IGameStageUser>> SocketOnPacket,
    IPipeline<ISocketOnException<IGameStageUser>> SocketOnException,
    IPipeline<ISocketOnDisconnect<IGameStageUser>> SocketOnDisconnect
) : IGameContextPipelines;
