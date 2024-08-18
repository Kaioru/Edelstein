using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Entities;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Recv;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handling.Pipes;

public class UserOnPacketCheckPasswordPipe(
    IAccountRepository repository
) : IPipe<PipedPacketMessage<ILoginStageSystem, ILoginStageSystemUser, CheckPassword>>
{
    public async Task Handle(IPipelineContext ctx, PipedPacketMessage<ILoginStageSystem, ILoginStageSystemUser, CheckPassword> message)
    {
        Console.WriteLine(await repository.RetrieveByUsername("test"));
        Console.WriteLine(message.Packet);
    }
}
