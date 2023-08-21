using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Contracts.Pipelines;
using Edelstein.Protocol.Gameplay.Models.Accounts;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay;

public abstract class AbstractStageUser<TStageUser> : IStageUser<TStageUser> 
    where TStageUser : IStageUser<TStageUser>
{
    public int ID => Character?.ID ?? -1;

    public ISocket Socket { get; }

    public IStage<TStageUser>? Stage { get; set; }

    public IAccount? Account { get; set; }
    public IAccountWorld? AccountWorld { get; set; }
    public ICharacter? Character { get; set; }

    public long Key { get; set; }

    public bool IsMigrating { get; set; }

    protected abstract TStageUser Self { get; }

    private readonly IPipeline<UserMigrate<TStageUser>> _userMigrate;
    private readonly IPipeline<UserOnPacket<TStageUser>> _userOnPacket;
    private readonly IPipeline<UserOnException<TStageUser>> _userOnException;
    private readonly IPipeline<UserOnDisconnect<TStageUser>> _userOnDisconnect;

    protected AbstractStageUser(
        ISocket socket, 
        
        IPipeline<UserMigrate<TStageUser>> userMigrate,
        IPipeline<UserOnPacket<TStageUser>> userOnPacket, 
        IPipeline<UserOnException<TStageUser>> userOnException, 
        IPipeline<UserOnDisconnect<TStageUser>> userOnDisconnect
    )
    {
        Socket = socket;

        _userMigrate = userMigrate;
        _userOnPacket = userOnPacket;
        _userOnException = userOnException;
        _userOnDisconnect = userOnDisconnect;
    }
    
    public Task Migrate(string serverID, IPacket? packet = null) 
        => _userMigrate.Process(new UserMigrate<TStageUser>(Self, serverID, packet));
    
    public Task OnPacket(IPacket packet)
        => _userOnPacket.Process(new UserOnPacket<TStageUser>(Self, packet));
    
    public Task OnException(Exception exception)
        => _userOnException.Process(new UserOnException<TStageUser>(Self, exception));
    
    public Task OnDisconnect()
        => _userOnDisconnect.Process(new UserOnDisconnect<TStageUser>(Self));

    public Task Dispatch(IPacket packet) => Socket.Dispatch(packet);

    public Task Disconnect() => Socket.Close();
}
