using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay;

public abstract class AbstractStageSystem<TStageSystem, TStageSystemUser> : IStageSystem<TStageSystem, TStageSystemUser>
    where TStageSystem : IStageSystem<TStageSystem, TStageSystemUser>
    where TStageSystemUser : IStageSystemUser<TStageSystem, TStageSystemUser>
{
    public abstract string ID { get; }
    
    protected abstract IPipeline<UserOnPacket<TStageSystem, TStageSystemUser>> OnPacketPipeline { get; }
    protected abstract IPipeline<UserOnException<TStageSystem, TStageSystemUser>> OnExceptionPipeline { get; }
    protected abstract IPipeline<UserOnDisconnect<TStageSystem, TStageSystemUser>> OnDisconnectPipeline { get; }

    public Task OnPacket(TStageSystemUser user, IRawPacket packet) 
        => OnPacketPipeline.Process(new UserOnPacket<TStageSystem, TStageSystemUser>(user, packet));

    public Task OnException(TStageSystemUser user, Exception exception) 
        => OnExceptionPipeline.Process(new UserOnException<TStageSystem, TStageSystemUser>(user, exception));
    
    public Task OnDisconnect(TStageSystemUser user)
        => OnDisconnectPipeline.Process(new UserOnDisconnect<TStageSystem, TStageSystemUser>(user));
    
    public abstract TStageSystemUser Initialize(ISocket socket);
}
