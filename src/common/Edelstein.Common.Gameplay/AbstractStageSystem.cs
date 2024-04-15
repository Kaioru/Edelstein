using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Utilities.Buffers;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay;

public abstract class AbstractStageSystem<TStageUser, TStageSystem> : 
    IStageSystem<TStageUser, TStageSystem> 
    where TStageUser : IStageUser<TStageUser, TStageSystem> 
    where TStageSystem : IStageSystem<TStageUser, TStageSystem>
{
    public abstract string ID { get; }
    
    protected abstract IPipeline<UserOnPacket<TStageUser, TStageSystem>> UserOnPacketPipeline { get; }
    protected abstract IPipeline<UserOnException<TStageUser, TStageSystem>> UserOnExceptionPipeline { get; }
    protected abstract IPipeline<UserOnDisconnect<TStageUser, TStageSystem>> UserOnDisconnectPipeline { get; }
    
    public abstract TStageUser CreateUser(ISocket socket);

    public Task OnPacket(TStageUser user, IPacket packet)
        => UserOnPacketPipeline.Process(new UserOnPacket<TStageUser, TStageSystem>(user, packet));

    public Task OnException(TStageUser user, Exception exception)
        => UserOnExceptionPipeline.Process(new UserOnException<TStageUser, TStageSystem>(user, exception));

    public Task OnDisconnect(TStageUser user)
        => UserOnDisconnectPipeline.Process(new UserOnDisconnect<TStageUser, TStageSystem>(user));
}
