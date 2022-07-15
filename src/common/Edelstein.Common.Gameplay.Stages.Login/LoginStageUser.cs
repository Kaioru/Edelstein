using Edelstein.Protocol.Gameplay.Accounts;
using Edelstein.Protocol.Gameplay.Characters;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Common.Gameplay.Stages.Login;

public class LoginStageUser : ILoginStageUser
{
    public int ID => Character?.ID ?? -1;

    public ISocket Socket { get; }
    public ILoginStage? Stage { get; set; }

    public IAccount? Account { get; set; }
    public IAccountWorld? AccountWorld { get; set; }
    public ICharacter? Character { get; set; }

    public LoginState State { get; set; }
    public byte? SelectedWorldID { get; set; }
    public byte? SelectedChannelID { get; set; }

    public LoginStageUser(ISocket socket)
    {
        Socket = socket;
    }

    public Task OnPacket(IPacket packet)
    {
        throw new NotImplementedException();
    }

    public Task OnException(Exception exception)
    {
        throw new NotImplementedException();
    }

    public Task OnDisconnect()
    {
        throw new NotImplementedException();
    }

    public Task Dispatch(IPacket packet) => Socket.Dispatch(packet);
    public Task Disconnect() => Socket.Close();
}
