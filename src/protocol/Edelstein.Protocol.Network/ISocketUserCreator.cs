namespace Edelstein.Protocol.Network;

public interface ISocketUserCreator<out TSocketUser>
    where TSocketUser : ISocketUser
{
    TSocketUser CreateUser(ISocket socket);
}
