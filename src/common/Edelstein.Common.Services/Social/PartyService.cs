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
    public class PartyService : IPartyService
    {
        private static readonly string PartyScope = "parties";
        private static readonly string PartyLockScope = $"{PartyScope}:lock";
        private static readonly string PartyLockKey = "0";
        private static readonly TimeSpan PartyLockTimeoutDuration = TimeSpan.FromSeconds(6);

        private readonly IMessageBus _messenger;
        private readonly ILockProvider _locker;
        private readonly PartyRepository _repository;
        private readonly ICollection<ChannelWriter<PartyUpdateContract>> _dispatchers;

        public PartyService(ICacheClient cache, IMessageBus messenger, IDataStore store)
        {
            _messenger = messenger;
            _locker = new ScopedLockProvider(new CacheLockProvider(cache, messenger), PartyLockScope);
            _repository = new PartyRepository(cache, store);
            _dispatchers = new List<ChannelWriter<PartyUpdateContract>>();

            _ = _messenger.SubscribeAsync<PartyUpdateEvent>(async e =>
            {
                var contract = new PartyUpdateContract { Party = e.Party.ToContract() };

                await Task.WhenAll(_dispatchers.Select(async d => await d.WriteAsync(contract)));
            });
        }

        public async Task<PartyLoadByIDResponse> LoadByID(PartyLoadByIDRequest request)
        {
            var source = new CancellationTokenSource();

            source.CancelAfter(PartyLockTimeoutDuration);

            var @lock = await _locker.AcquireAsync(PartyLockKey, cancellationToken: source.Token);

            if (@lock != null)
            {
                var party = await _repository.Retrieve(request.Id);
                await @lock.ReleaseAsync();
                return new PartyLoadByIDResponse {
                    Result = PartyServiceResult.Ok,
                    Party = party?.ToContract()
                };
            }

            return new PartyLoadByIDResponse { Result = PartyServiceResult.FailedTimeout };
        }

        public async Task<PartyLoadByCharacterResponse> LoadByCharacter(PartyLoadByCharacterRequest request)
        {
            var source = new CancellationTokenSource();

            source.CancelAfter(PartyLockTimeoutDuration);

            var @lock = await _locker.AcquireAsync(PartyLockKey, cancellationToken: source.Token);

            if (@lock != null)
            {
                var party = await _repository.RetrieveByMember(request.Character);
                await @lock.ReleaseAsync();
                return new PartyLoadByCharacterResponse {
                    Result = PartyServiceResult.Ok,
                    Party = party?.ToContract()
                };
            }

            return new PartyLoadByCharacterResponse { Result = PartyServiceResult.FailedTimeout };
        }

        public Task<PartyCreateResponse> Create(PartyCreateResponse request) { throw new NotImplementedException(); }
        public Task<PartyWithdrawResponse> Withdraw(PartyWithdrawRequest request) { throw new NotImplementedException(); }

        public async IAsyncEnumerable<PartyUpdateContract> Subscribe(CallContext context = default)
        {
            var channel = Channel.CreateBounded<PartyUpdateContract>(8);

            _dispatchers.Add(channel);

            await foreach (var dispatch in channel.Reader.ReadAllAsync(context.CancellationToken))
                yield return dispatch;

            _dispatchers.Remove(channel);
            channel.Writer.Complete();
        }
    }
}
