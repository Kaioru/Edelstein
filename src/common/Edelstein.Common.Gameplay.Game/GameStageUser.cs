using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Contexts;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game;

public class GameStageUser : AbstractStageUser<IGameStageUser>, IGameStageUser
{
    public GameStageUser(
        ISocket socket,
        GameContext context
    ) : base(socket) =>
        Context = context;
    public GameContext Context { get; }

    public IField? Field => FieldUser?.Field;
    public IFieldUser? FieldUser { get; set; }

    public override Task Migrate(string serverID, IPacket? packet = null)
        => Context.Pipelines.UserMigrate.Process(new UserMigrate<IGameStageUser>(this, serverID, packet));

    public override Task OnPacket(IPacket packet)
        => Context.Pipelines.UserOnPacket.Process(new UserOnPacket<IGameStageUser>(this, packet));

    public override Task OnException(Exception exception)
        => Context.Pipelines.UserOnException.Process(new UserOnException<IGameStageUser>(this, exception));

    public override Task OnDisconnect()
        => Context.Pipelines.UserOnDisconnect.Process(new UserOnDisconnect<IGameStageUser>(this));
}
