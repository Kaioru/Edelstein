using Edelstein.Common.Gameplay.Plugs;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Models.Accounts;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Services.Session;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class UserOnDisconnectPlug : AbstractUserOnDisconnectPlug<IGameStageUser>
{
    public UserOnDisconnectPlug(
        ISessionService session,
        IAccountRepository accountRepository,
        IAccountWorldRepository accountWorldRepository,
        ICharacterRepository characterRepository
    ) : base(session, accountRepository, accountWorldRepository, characterRepository)
    {
    }

    public override async Task Handle(IPipelineContext ctx, UserOnDisconnect<IGameStageUser> message)
    {
        if (message.User is { Field: { }, FieldUser: { }, Character: { } })
        {
            message.User.Character.FieldID = message.User.Field.Template.ForcedReturn ?? message.User.Field.Template.ID;
            message.User.Character.FieldPortal = (byte)(message.User.Field.Template.ForcedReturn == null
                ? 0
                : message.User.Field.Template.StartPoints
                    .FindClosest(message.User.FieldUser.Position)
                    .FirstOrDefault()?.ID ?? 0
            );
        }
        
        await base.Handle(ctx, message);
    }
}
