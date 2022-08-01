using Edelstein.Common.Gameplay.Stages.Messages;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Contexts;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Util.Buffers.Bytes;

namespace Edelstein.Common.Gameplay.Stages.Game;

public class GameStageUser : AbstractStageUser<IGameStageUser>, IGameStageUser
{
    public GameStageUser(ISocket socket, IGameContext context) : base(socket) => Context = context;

    public IGameContext Context { get; }

    public override Task OnMigrateIn(int character, long key) =>
        Context.Pipelines.SocketOnMigrateIn.Process(new SocketOnMigrateIn<IGameStageUser>(this, character, key));

    public override Task OnMigrateOut(string server) =>
        Context.Pipelines.SocketOnMigrateOut.Process(new SocketOnMigrateOut<IGameStageUser>(this, server));

    public override Task OnPacket(IByteBuffer packet) =>
        Context.Pipelines.SocketOnPacket.Process(new SocketOnPacket<IGameStageUser>(this, packet));

    public override Task OnException(Exception exception) =>
        Context.Pipelines.SocketOnException.Process(new SocketOnException<IGameStageUser>(this, exception));

    public override Task OnDisconnect() =>
        Context.Pipelines.SocketOnDisconnect.Process(new SocketOnDisconnect<IGameStageUser>(this));
}
