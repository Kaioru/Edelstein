using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Services.Dispatch;
using Edelstein.Protocol.Services.Dispatch.Contracts;
using Edelstein.Protocol.Services.Session.Contracts;
using ProtoBuf.Grpc;

namespace Edelstein.Common.Services.Dispatch;

public partial class DispatchService
{
    public async Task<DispatchServiceResponse> Send(DispatchServiceSendRequest request, CallContext context = default)
    {
        var info = request.Info;
        var targets = (await _repository.RetrieveAll()).AsEnumerable();

        if (info.TargetServerID != null)
            targets = targets.Where(t => t.ServerID == info.TargetServerID);
        
        if (info.TargetWorldID.HasValue)
            targets = targets.Where(t => t.WorldID == info.TargetWorldID);
        
        if (info.TargetChannelID.HasValue)
            targets = targets.Where(t => t.ChannelID == info.TargetChannelID);

        if (info.TargetCharacterID.HasValue)
        {
            var session = await sessions.GetByActiveCharacter(new SessionServiceGetByActiveCharacterRequest
            {
                CharacterID = info.TargetCharacterID.Value
            });

            if (session.Info != null)
                targets = targets.Where(t => t.ServerID == session.Info.ServerID);
        }

        await Task.WhenAll(targets
            .Select(async t 
                => await t.Channel.WriteAsync(request.Info)));
        return new DispatchServiceResponse
        {
            Result = DispatchServiceResult.Success
        };
    }
}
