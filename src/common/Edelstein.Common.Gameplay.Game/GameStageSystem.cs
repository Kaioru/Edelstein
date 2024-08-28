using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Contexts;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game;

public class GameStageSystem(
    IGameStageSystemOptions options,
    GameContext context
) : AbstractStageSystem<IGameStageSystem, IGameStageSystemUser>, IGameStageSystem
{
    public override string ID => options.ID;
    
    public IGameStageSystemOptions Options { get; } = options;
    public GameContext Context { get; } = context;

    protected override IPipeline<UserOnPacket<IGameStageSystem, IGameStageSystemUser>> OnPacketPipeline 
        => Context.Pipelines.UserOnPacket;
    protected override IPipeline<UserOnException<IGameStageSystem, IGameStageSystemUser>> OnExceptionPipeline 
        => Context.Pipelines.UserOnException;
    protected override IPipeline<UserOnDisconnect<IGameStageSystem, IGameStageSystemUser>> OnDisconnectPipeline 
        => Context.Pipelines.UserOnDisconnect;

    public override IGameStageSystemUser Initialize(ISocket socket)
        => new GameStageSystemUser(socket, this);
}
