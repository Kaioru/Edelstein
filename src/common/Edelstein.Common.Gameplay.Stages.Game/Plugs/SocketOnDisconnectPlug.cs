using Edelstein.Common.Gameplay.Accounts;
using Edelstein.Common.Gameplay.Characters;
using Edelstein.Common.Gameplay.Stages.Plugs;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Messages;
using Edelstein.Protocol.Services.Session;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Game.Plugs;

public class SocketOnDisconnectPlug : AbstractSocketOnDisconnectPlug<IGameStageUser>
{
    public SocketOnDisconnectPlug(
        ISessionService session,
        IAccountRepository accountRepository,
        IAccountWorldRepository accountWorldRepository,
        ICharacterRepository characterRepository
    ) : base(session, accountRepository, accountWorldRepository, characterRepository)
    {
    }

    public override Task Handle(IPipelineContext ctx, ISocketOnDisconnect<IGameStageUser> message)
    {
        if (
            message.User.Character != null &&
            message.User.Field != null &&
            message.User.FieldUser != null
        )
            message.User.Character.FieldPortal = (byte)(message.User.Field.Template.StartPoints
                .FindClosest(message.User.FieldUser.Position)
                .FirstOrDefault()?.ID ?? 0);
        return base.Handle(ctx, message);
    }
}
