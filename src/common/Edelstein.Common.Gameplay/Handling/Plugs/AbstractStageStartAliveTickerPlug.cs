﻿using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;
using Edelstein.Protocol.Utilities.Tickers;

namespace Edelstein.Common.Gameplay.Handling.Plugs;

public abstract class AbstractStageStartAliveTickerPlug<TStageUser> : IPipelinePlug<StageStart>, ITickable
    where TStageUser : class, IStageUser<TStageUser>
{
    private readonly IStage<TStageUser> _stage;
    private readonly ITickerManager _ticker;

    public AbstractStageStartAliveTickerPlug(ITickerManager ticker, IStage<TStageUser> stage)
    {
        _ticker = ticker;
        _stage = stage;
    }

    public Task Handle(IPipelineContext ctx, StageStart message) =>
        Task.FromResult(_ticker.Schedule(this, TimeSpan.FromSeconds(20)));

    public async Task OnTick(DateTime now)
    {
        using var packet =  new PacketWriter(PacketSendOperations.AliveReq);
        var built = packet.Build();
        
        foreach (var user in (await _stage.Users.RetrieveAll())
                 .Where(u => now - u.Socket.LastAliveSent > TimeSpan.FromMinutes(2)))
        {
            user.Socket.LastAliveSent = now;
            await user.Socket.Dispatch(built);
        }
    }
}
