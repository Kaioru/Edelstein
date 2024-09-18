using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Game.Objects.Users;
using Edelstein.Common.Gameplay.Handling.Pipes;
using Edelstein.Protocol.Gameplay.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Services.Migration;
using Edelstein.Protocol.Services.Session;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Pipes;

public class UserOnPacketMigrateIn(
    IMigrationService migrations,
    ISessionService sessions,
    IFieldManager fields
) : BaseUserOnPacketMigrateIn<IGameStageSystem, IGameStageSystemUser>(migrations, sessions)
{
    public override async Task Handle(IPipelineContext ctx, PipedPacketMessage<IGameStageSystem, IGameStageSystemUser, MigrateIn> message)
    {
        await base.Handle(ctx, message);

        if (message.User.Account == null) return;
        if (message.User.AccountWorldData == null) return;
        if (message.User.Character == null) return;

        var field = await fields.Retrieve(message.User.Character.FieldID);
        if (field == null) return;
        var start = await field.Template.StartPoints.Retrieve(message.User.Character.FieldPortal);
        if (start == null) return;
        
        var fieldUser = new FieldUser(
            message.User, 
            message.User.Account, 
            message.User.AccountWorldData, 
            message.User.Character,
            start.Position
        );

        message.User.FieldUser = fieldUser;
        await field.Enter(fieldUser);
    }
}
