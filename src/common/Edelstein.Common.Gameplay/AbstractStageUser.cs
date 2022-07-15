using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Accounts;
using Edelstein.Protocol.Gameplay.Characters;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Common.Gameplay;

public class AbstractStageUser<TStage, TStageUser> : IStageUser<TStage, TStageUser>
    where TStage : IStage<TStage, TStageUser>
    where TStageUser : IStageUser<TStage, TStageUser>
{
    public int ID => Character?.ID ?? -1;

    public ISocket Socket { get; }

    public IAccount? Account { get; set; }
    public IAccountWorld? AccountWorld { get; set; }
    public ICharacter? Character { get; set; }

    protected AbstractStageUser(ISocket socket)
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
