using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling.Pipes;
using Edelstein.Protocol.Gameplay.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Entities;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Network.Packets.Types;
using Edelstein.Protocol.Services.Migration;
using Edelstein.Protocol.Services.Session;
using Edelstein.Protocol.Utilities;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Pipes;

public class UserOnPacketMigrateIn(
    IMigrationService migrations,
    ISessionService sessions,
    IDateTimeProvider dateTimeProvider
) : BaseUserOnPacketMigrateIn<IGameStageSystem, IGameStageSystemUser>(migrations, sessions)
{
    public override async Task Handle(IPipelineContext ctx, PipedPacketMessage<IGameStageSystem, IGameStageSystemUser, MigrateIn> message)
    {
        await base.Handle(ctx, message);

        if (message.User.Character == null) return;

        await message.User.Dispatch(new SetField
        {
            ChannelID = message.User.System.Options.ChannelID,
            IsInitialize = true,
            Info = new SetFieldInfoCharacterInit
            {
                Seed1 = 0,
                Seed2 = 0,
                Seed3 = 0,
                Data = message.User.Character.ToStructuredCharacterData()
            },
            DateServer = new FDateTime(dateTimeProvider.Now)
        });
    }
}
