using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Entities;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Recv;
using Edelstein.Protocol.Services.Auth;
using Edelstein.Protocol.Services.Auth.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handling.Pipes;

public class UserOnPacketCheckPasswordPipe(
    IAuthService service,
    IAccountRepository repository
) : IPipe<PipedPacketMessage<ILoginStageSystem, ILoginStageSystemUser, CheckPassword>>
{
    public async Task Handle(IPipelineContext ctx, PipedPacketMessage<ILoginStageSystem, ILoginStageSystemUser, CheckPassword> message)
    {
        var request = new AuthServiceRequest
        {
            Username = message.Packet.Username.Value,
            Password = message.Packet.Password.Value
        };
        
        Console.WriteLine(await service.Register(request));
        Console.WriteLine(await service.Login(request));
        Console.WriteLine(message.Packet);
    }
}
