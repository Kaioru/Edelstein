using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Edelstein.Protocol.Datastore;
using Edelstein.Protocol.Services.Contracts.Social;
using Edelstein.Protocol.Services.Social;
using Foundatio.Caching;
using Foundatio.Lock;
using Foundatio.Messaging;
using ProtoBuf.Grpc;

namespace Edelstein.Common.Services.Social
{
    public class GuildService : IGuildService
    {
        private static readonly string GuildScope = "guilds";
        private static readonly string GuildLockScope = $"{GuildScope}:lock";
        private static readonly string GuildLockKey = "0";
        private static readonly TimeSpan GuildLockTimeoutDuration = TimeSpan.FromSeconds(6);

        private readonly IMessageBus _messenger;
        private readonly ILockProvider _locker;
        private readonly GuildRepository _repository;
        private readonly ICollection<ChannelWriter<GuildUpdateContract>> _dispatchers;

        public GuildService(ICacheClient cache, IMessageBus messenger, IDataStore store)
        {
            _messenger = messenger;
            _locker = new ScopedLockProvider(new CacheLockProvider(cache, messenger), GuildLockScope);
            _repository = new GuildRepository(cache, store);
            _dispatchers = new List<ChannelWriter<GuildUpdateContract>>();

            _ = _messenger.SubscribeAsync<GuildUpdateEvent>(async e =>
            {
                var contract = new GuildUpdateContract { Guild = e.Guild.ToContract() };

                await Task.WhenAll(_dispatchers.Select(async d => await d.WriteAsync(contract)));
            });
        }

        public async Task<GuildLoadByIDResponse> LoadByID(GuildLoadByIDRequest request)
        {
            var source = new CancellationTokenSource();

            source.CancelAfter(GuildLockTimeoutDuration);

            var @lock = await _locker.AcquireAsync(GuildLockKey, cancellationToken: source.Token);

            if (@lock != null)
            {
                var guild = await _repository.Retrieve(request.Id);
                await @lock.ReleaseAsync();
                return new GuildLoadByIDResponse {
                    Result = GuildServiceResult.Ok,
                    Guild = guild?.ToContract()
                };
            }

            return new GuildLoadByIDResponse { Result = GuildServiceResult.FailedTimeout };
        }

        public async Task<GuildLoadByCharacterResponse> LoadByCharacter(GuildLoadByCharacterRequest request)
        {
            var source = new CancellationTokenSource();

            source.CancelAfter(GuildLockTimeoutDuration);

            var @lock = await _locker.AcquireAsync(GuildLockKey, cancellationToken: source.Token);

            if (@lock != null)
            {
                var guild = await _repository.RetrieveByMember(request.Character);
                await @lock.ReleaseAsync();
                return new GuildLoadByCharacterResponse
                {
                    Result = GuildServiceResult.Ok,
                    Guild = guild?.ToContract()
                };
            }

            return new GuildLoadByCharacterResponse { Result = GuildServiceResult.FailedTimeout };
        }

        public async IAsyncEnumerable<GuildUpdateContract> Subscribe(CallContext context = default)
        {
            var channel = Channel.CreateBounded<GuildUpdateContract>(8);

            _dispatchers.Add(channel);

            await foreach (var dispatch in channel.Reader.ReadAllAsync(context.CancellationToken))
                yield return dispatch;

            _dispatchers.Remove(channel);
            channel.Writer.Complete();
        }
    }
}
