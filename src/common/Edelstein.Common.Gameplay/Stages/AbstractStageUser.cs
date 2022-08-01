using Edelstein.Protocol.Gameplay.Accounts;
using Edelstein.Protocol.Gameplay.Characters;
using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Util.Buffers.Bytes;

namespace Edelstein.Common.Gameplay.Stages;

public abstract class AbstractStageUser<TStageUser> : IStageUser<TStageUser> where TStageUser : IStageUser<TStageUser>
{
    protected AbstractStageUser(ISocket socket) => Socket = socket;

    public int ID => Character?.ID ?? -1;

    public ISocket Socket { get; }

    public IStage<TStageUser>? Stage { get; set; }

    public IAccount? Account { get; set; }
    public IAccountWorld? AccountWorld { get; set; }
    public ICharacter? Character { get; set; }

    public bool IsMigrating { get; set; }

    public abstract Task OnPacket(IByteBuffer packet);
    public abstract Task OnException(Exception exception);
    public abstract Task OnDisconnect();

    public Task Dispatch(IByteBuffer packet) => Socket.Dispatch(packet);

    public Task Disconnect() => Socket.Close();
}
