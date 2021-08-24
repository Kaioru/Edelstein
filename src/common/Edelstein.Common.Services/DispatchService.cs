using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using Edelstein.Protocol.Services;
using Edelstein.Protocol.Services.Contracts;
using Foundatio.Messaging;
using Google.Protobuf;
using ProtoBuf.Grpc;

namespace Edelstein.Common.Services
{
    public class DispatchService : IDispatchService
    {
        private readonly IMessageBus _messenger;
        private readonly ICollection<ChannelWriter<DispatchContract>> _dispatchers;

        public DispatchService(IMessageBus messenger)
        {
            _messenger = messenger;
            _dispatchers = new List<ChannelWriter<DispatchContract>>();

            _ = _messenger.SubscribeAsync<DispatchEvent>(async e =>
            {
                var contract = new DispatchContract { Data = ByteString.CopyFrom(e.Data) };

                contract.TargetServers.Add(e.TargetServers);
                contract.TargetCharacters.Add(e.TargetCharacters);

                await Task.WhenAll(_dispatchers.Select(async d => await d.WriteAsync(contract)));
            });
        }


        public async Task<DispatchResponse> Dispatch(DispatchRequest request)
        {
            await _messenger.PublishAsync(new DispatchEvent(
                Data: request.Data.ToArray(),
                TargetServers: new List<string>(),
                TargetCharacters: new List<int>()
            ));
            return new DispatchResponse { Result = DispatchServiceResult.Ok };
        }

        public async Task<DispatchToServersResponse> DispatchToServers(DispatchToServersRequest request)
        {
            var @event = new DispatchEvent(
                Data: request.Data.ToArray(),
                TargetServers: new List<string>(),
                TargetCharacters: new List<int>()
            );

            foreach (var server in request.Servers)
                @event.TargetServers.Add(server);

            await _messenger.PublishAsync(@event);
            return new DispatchToServersResponse { Result = DispatchServiceResult.Ok };
        }

        public async Task<DispatchToCharactersResponse> DispatchToCharacters(DispatchToCharactersRequest request)
        {

            var @event = new DispatchEvent(
                Data: request.Data.ToArray(),
                TargetServers: new List<string>(),
                TargetCharacters: new List<int>()
            );

            foreach (var character in request.Characters)
                @event.TargetCharacters.Add(character);

            return new DispatchToCharactersResponse { Result = DispatchServiceResult.Ok };
        }

        public async IAsyncEnumerable<DispatchContract> Subscribe(DispatchSubscription request, CallContext context = default)
        {
            var channel = Channel.CreateBounded<DispatchContract>(8);

            _dispatchers.Add(channel);

            await foreach (var dispatch in channel.Reader.ReadAllAsync(context.CancellationToken))
            {
                if (dispatch.TargetServers.Count > 0 && !dispatch.TargetServers.Contains(request.Server))
                    continue;

                yield return dispatch;
            }

            _dispatchers.Remove(channel);
            channel.Writer.Complete();
        }
    }
}
