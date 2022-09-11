using Edelstein.Common.Gameplay.Stages.Contracts.Pipelines;
using Edelstein.Protocol.Gameplay.Stages.Chat;
using Edelstein.Protocol.Gameplay.Stages.Chat.Contexts;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Chat;

public class ChatStageUser : AbstractStageUser<IChatStageUser>, IChatStageUser
{
    public ChatStageUser(ISocket socket, IChatContext context) : base(socket) => Context = context;

    public IChatContext Context { get; }

    public override Task OnMigrateIn(int character, long key) =>
        Context.Pipelines.SocketOnMigrateIn.Process(new SocketOnMigrateIn<IChatStageUser>(this, character, key));

    public override Task OnMigrateOut(string server) =>
        Context.Pipelines.SocketOnMigrateOut.Process(new SocketOnMigrateOut<IChatStageUser>(this, server));

    public override Task OnAliveAck(DateTime date) =>
        Context.Pipelines.SocketOnAliveAck.Process(new SocketOnAliveAck<IChatStageUser>(this, date));

    public override Task OnPacket(IPacket packet) =>
        Context.Pipelines.SocketOnPacket.Process(new SocketOnPacket<IChatStageUser>(this, packet));

    public override Task OnException(Exception exception) =>
        Context.Pipelines.SocketOnException.Process(new SocketOnException<IChatStageUser>(this, exception));

    public override Task OnDisconnect() =>
        Context.Pipelines.SocketOnDisconnect.Process(new SocketOnDisconnect<IChatStageUser>(this));
}
