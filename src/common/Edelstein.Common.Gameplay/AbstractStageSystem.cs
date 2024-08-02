using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay;

public abstract class AbstractStageSystem<TStageSystemUser, TStageSystem> : IStageSystem<TStageSystemUser, TStageSystem>
    where TStageSystemUser : IStageSystemUser<TStageSystemUser, TStageSystem>
    where TStageSystem : IStageSystem<TStageSystemUser, TStageSystem>
{
    public abstract string ID { get; }
    
    protected abstract IPipeline<UserOnPacket<TStageSystemUser, TStageSystem>> OnPacketPipeline { get; }
    protected abstract IPipeline<UserOnException<TStageSystemUser, TStageSystem>> OnExceptionPipeline { get; }
    protected abstract IPipeline<UserOnDisconnect<TStageSystemUser, TStageSystem>> OnDisconnectPipeline { get; }

    public Task OnPacket(TStageSystemUser user, IRawPacket packet)
        => OnPacketPipeline.Process(new UserOnPacket<TStageSystemUser, TStageSystem>(user, packet));
    
    public Task OnException(TStageSystemUser user, Exception exception) 
        => OnExceptionPipeline.Process(new UserOnException<TStageSystemUser, TStageSystem>(user, exception));
    
    public Task OnDisconnect(TStageSystemUser user)
        => OnDisconnectPipeline.Process(new UserOnDisconnect<TStageSystemUser, TStageSystem>(user));
    
    public abstract TStageSystemUser Initialize(ISocket socket);
}
