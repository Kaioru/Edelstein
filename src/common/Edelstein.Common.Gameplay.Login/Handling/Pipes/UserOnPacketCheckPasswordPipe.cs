using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Recv;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handling.Pipes;

public class UserOnPacketCheckPasswordPipe : IPipe<PipedPacketMessage<ILoginStageSystem, ILoginStageSystemUser, CheckPassword>>
{
    public Task Handle(IPipelineContext ctx, PipedPacketMessage<ILoginStageSystem, ILoginStageSystemUser, CheckPassword> message)
    {
        Console.WriteLine(message.Packet);
        return Task.CompletedTask;
    }
}
