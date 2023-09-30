using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Plugs;

public class UserOnPacketCreateSecurityHandlePlug : IPipelinePlug<UserOnPacketCreateSecurityHandlePlug>
{
    public Task Handle(IPipelineContext ctx, UserOnPacketCreateSecurityHandlePlug message)
        => Task.CompletedTask;
}
