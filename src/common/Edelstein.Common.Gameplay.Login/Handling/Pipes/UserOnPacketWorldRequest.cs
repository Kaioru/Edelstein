using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Login.Templates;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Send;
using Edelstein.Protocol.Network.Packets.Types;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Server.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Login.Handling.Pipes;

public class UserOnPacketWorldRequest(
    ITemplateManager<LoginWorldInfoTemplate> templates,
    IServerService servers
) : IPipe<PipedPacketMessage<ILoginStageSystem, ILoginStageSystemUser, WorldRequest>>
{
    public async Task Handle(IPipelineContext ctx, PipedPacketMessage<ILoginStageSystem, ILoginStageSystemUser, WorldRequest> message)
    {
        if (message.User.State != LoginState.SelectWorld) return;
        
        foreach (var worldID in message.User.System.Options.Worlds)
        {
            var template = await templates.Retrieve(worldID);
            var response = await servers.GetGameByWorld(new ServerServiceGetByWorldRequest
            {
                WorldID = worldID
            });

            if (template == null) continue;
            if (response.Result != ServerServiceResult.Success || response.Info == null) continue;

            await message.User.Dispatch(new WorldInformation
            {
                ID = worldID,
                Info = new WorldInformationInfo
                {
                    Name = new LPString(template.Name),
                    State = template.State,
                    IsBlockCharCreation = template.BlockCharCreation,
                    Channels = response.Info
                        .OrderBy(i => i.ChannelID)
                        .Select(i => new WorldInformationInfoChannel
                        {
                            Name = new LPString(i.ID),
                            WorldID = (byte)i.WorldID,
                            ChannelID = (byte)i.ChannelID,
                            IsAdultChannel = i.IsAdultChannel
                        })
                        .ToList()
                }
            });
        }

        await message.User.Dispatch(new WorldInformation
        {
            ID = 0xFF
        });
    }
}
