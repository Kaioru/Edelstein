using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Models.Accounts;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay;

public abstract class AbstractStageUser<TStageUser> : IStageUser<TStageUser>
    where TStageUser : IStageUser<TStageUser>
{

    protected AbstractStageUser(ISocket socket)
        => Socket = socket;
    public int ID => Character?.ID ?? -1;

    public ISocket Socket { get; }

    public IStage<TStageUser>? Stage { get; set; }

    public IAccount? Account { get; set; }
    public IAccountWorld? AccountWorld { get; set; }
    public ICharacter? Character { get; set; }

    public long Key { get; set; }

    public bool IsMigrating { get; set; }

    public abstract Task Migrate(string serverID, IPacket? packet = null);
    public abstract Task OnPacket(IPacket packet);
    public abstract Task OnException(Exception exception);
    public abstract Task OnDisconnect();

    public Task Dispatch(IPacket packet) => Socket.Dispatch(packet);

    public Task Disconnect() => Socket.Close();
}
