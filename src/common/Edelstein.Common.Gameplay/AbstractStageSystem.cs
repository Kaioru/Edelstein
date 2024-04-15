using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Utilities.Buffers;

namespace Edelstein.Common.Gameplay;

public abstract class AbstractStageSystem<TStageUser, TStageSystem> : 
    IStageSystem<TStageUser, TStageSystem> 
    where TStageUser : IStageUser<TStageUser, TStageSystem> 
    where TStageSystem : IStageSystem<TStageUser, TStageSystem>
{
    public abstract string ID { get; }
    
    public abstract TStageUser CreateUser(ISocket socket);

    public Task OnPacket(TStageUser user, IPacket packet) => Task.CompletedTask;
    public Task OnException(TStageUser user, Exception exception) => Task.CompletedTask;
    public Task OnDisconnect(TStageUser user) => Task.CompletedTask;
}
