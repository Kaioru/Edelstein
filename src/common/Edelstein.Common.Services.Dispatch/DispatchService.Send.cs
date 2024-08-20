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
        switch (request.Info.TargetType)
        {
            case DispatchTarget.All:
                await Task.WhenAll(
                    (await _serverIdIndex.RetrieveAll())
                    .Select(async e
                        => await e.Channel.WriteAsync(request.Info, context.CancellationToken)));
                break;
            case DispatchTarget.World:
            {
                var entry = await _worldIdIndex.Retrieve(request.Info.TargetID);

                if (entry == null)
                    return new DispatchServiceResponse
                    {
                        Result = DispatchServiceResult.FailedUnknown
                    };

                await entry.Channel.WriteAsync(request.Info, context.CancellationToken);
                break;
            }
            case DispatchTarget.Channel:
            {
                var entry = await _channelIdIndex.Retrieve(request.Info.TargetID);

                if (entry == null)
                    return new DispatchServiceResponse
                    {
                        Result = DispatchServiceResult.FailedUnknown
                    };

                await entry.Channel.WriteAsync(request.Info, context.CancellationToken);
                break;
            }
            case DispatchTarget.Character:
            {
                var session = await sessions.GetByActiveCharacter(new SessionServiceGetByActiveCharacterRequest
                {
                    CharacterID = request.Info.TargetID
                });

                if (session.Info != null)
                {
                    var entry = await _serverIdIndex.Retrieve(session.Info.ServerID);
                    
                    if (entry == null)
                        return new DispatchServiceResponse
                        {
                            Result = DispatchServiceResult.FailedUnknown
                        };

                    await entry.Channel.WriteAsync(request.Info, context.CancellationToken);
                }
                break;
            }
            default:
                return new DispatchServiceResponse
                {
                    Result = DispatchServiceResult.FailedUnknown
                };
        }

        return new DispatchServiceResponse
        {
            Result = DispatchServiceResult.Success
        };
    }
}
