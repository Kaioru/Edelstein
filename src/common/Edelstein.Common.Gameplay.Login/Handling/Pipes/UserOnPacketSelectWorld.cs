using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Recv;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handling.Pipes;

public class UserOnPacketSelectWorld : IPipe<PipedPacketMessage<ILoginStageSystem, ILoginStageSystemUser, SelectWorld>>
{
    public async Task Handle(IPipelineContext ctx, PipedPacketMessage<ILoginStageSystem, ILoginStageSystemUser, SelectWorld> message)
    {
        Console.WriteLine(message.Packet.WorldID);
        Console.WriteLine(message.Packet.ChannelID);
    }
}
