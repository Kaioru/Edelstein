using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Entities.Modifiers;
using Edelstein.Common.Gameplay.Handling.Pipes;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Entities;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;
using Edelstein.Protocol.Services.Session;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Pipes;

public class UserOnDisconnect(
    IAccountRepository accounts,
    IAccountWorldDataRepository accountWorldData,
    ICharacterRepository characters,
    ISessionService sessions
) : BaseUserOnDisconnect<IGameStageSystem, IGameStageSystemUser>(accounts, accountWorldData, characters, sessions)
{
    public override async Task Handle(IPipelineContext ctx, UserOnDisconnect<IGameStageSystem, IGameStageSystemUser> message)
    {
        _ = message.User.FieldUser?.EndConversation();
        
        if (message.User.FieldUser is { Field: not null })
        {
            if (message.User.Character != null)
            {
                new ModifyTemporaryStatContext(message.User.Character.TemporaryStats).ResetAll();
                
                message.User.Character.FieldID = message.User.FieldUser.Field.Template.ForcedReturn ?? message.User.FieldUser.Field.Template.ID;
                message.User.Character.FieldPortal = (byte)(message.User.FieldUser.Field.Template.ForcedReturn != null
                        ? 0
                        : message.User.FieldUser.Field.Template.StartPoints
                            .FindClosest(message.User.FieldUser.Position)
                            .FirstOrDefault()?.ID ?? 0
                    );
            }
            
            await message.User.FieldUser.Field.Leave(message.User.FieldUser);
        }
        
        await base.Handle(ctx, message);
    }
}
