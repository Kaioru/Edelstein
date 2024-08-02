namespace Edelstein.Protocol.Network;

public interface ISocketUserInitializer<out TSocketUser>
    where TSocketUser : ISocketUser
{
    TSocketUser Initialize(ISocket socket);
}
