using Edelstein.Common.Gameplay.Handling.Plugs;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Models.Accounts;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Social;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class UserOnDisconnectPlug : AbstractUserOnDisconnectPlug<IGameStageUser>
{
    public UserOnDisconnectPlug(
        ISessionService session, 
        IAccountRepository accountRepository, 
        IAccountWorldRepository accountWorldRepository,
        ICharacterRepository characterRepository, 
        IFriendService friendService, 
        IPartyService partyService
    ) : base(session, accountRepository, accountWorldRepository, characterRepository, friendService, partyService)
    {
    }
    
    public override async Task Handle(IPipelineContext ctx, UserOnDisconnect<IGameStageUser> message)
    {
        _ = message.User.FieldUser?.EndConversation();
        _ = message.User.FieldUser?.EndDialogue();
        
        if (message.User is { Field: not null, FieldUser: not null, Character: not null })
        {
            message.User.Character.FieldID = message.User.Field.Template.ForcedReturn ?? message.User.Field.Template.ID;
            message.User.Character.FieldPortal = (byte)(message.User.Field.Template.ForcedReturn != null
                ? 0
                : message.User.Field.Template.StartPoints
                    .FindClosest(message.User.FieldUser.Position)
                    .FirstOrDefault()?.ID ?? 0
            );
        }
        
        await base.Handle(ctx, message);
    }
}
