using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Protocol.Datastore;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Services;
using Edelstein.Protocol.Services.Contracts;
using Edelstein.Protocol.Services.Contracts.Social;
using Edelstein.Protocol.Services.Social;
using Foundatio.Caching;
using Foundatio.Lock;
using Foundatio.Messaging;
using Google.Protobuf;
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
        private readonly IDispatchService _dispatcher;
        private readonly PartyRepository _repository;
        private readonly ICollection<ChannelWriter<PartyUpdateContract>> _channels;

        public PartyService(
            ICacheClient cache,
            IMessageBus messenger,
            IDataStore store,
            IDispatchService dispatcher
        )
        {
            _messenger = messenger;
            _locker = new ScopedLockProvider(new CacheLockProvider(cache, messenger), PartyLockScope);
            _dispatcher = dispatcher;
            _repository = new PartyRepository(cache, store);
            _channels = new List<ChannelWriter<PartyUpdateContract>>();

            _ = _messenger.SubscribeAsync<PartyUpdateEvent>(async e =>
            {
                var contract = new PartyUpdateContract { Party = e.Party.ToContract() };

                await Task.WhenAll(_channels.Select(async d => await d.WriteAsync(contract)));
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
                return new PartyLoadByIDResponse
                {
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
                return new PartyLoadByCharacterResponse
                {
                    Result = PartyServiceResult.Ok,
                    Party = party?.ToContract()
                };
            }

            return new PartyLoadByCharacterResponse { Result = PartyServiceResult.FailedTimeout };
        }

        public async Task<PartyCreateResponse> Create(PartyCreateRequest request)
        {
            var source = new CancellationTokenSource();

            source.CancelAfter(PartyLockTimeoutDuration);

            var @lock = await _locker.AcquireAsync(PartyLockKey, cancellationToken: source.Token);

            if (@lock != null)
            {
                var result = PartyServiceResult.Ok;
                var party = await _repository.RetrieveByMember(request.Member.Id);

                if (party != null) result = PartyServiceResult.FailedAlreadyInParty;

                if (result == PartyServiceResult.Ok)
                {
                    party = new PartyRecord { Boss = request.Member.Id };
                    party.Members.Add(new PartyMemberRecord(request.Member));

                    await _repository.Insert(party);
                    await _messenger.PublishAsync(new PartyUpdateEvent { Party = party });
                }

                await @lock.ReleaseAsync();

                return new PartyCreateResponse
                {
                    Result = result,
                    Party = party?.ToContract()
                };
            }

            return new PartyCreateResponse { Result = PartyServiceResult.FailedTimeout };
        }

        public async Task<PartyWithdrawResponse> Withdraw(PartyWithdrawRequest request)
        {
            var source = new CancellationTokenSource();

            source.CancelAfter(PartyLockTimeoutDuration);

            var @lock = await _locker.AcquireAsync(PartyLockKey, cancellationToken: source.Token);

            if (@lock != null)
            {
                var result = PartyServiceResult.Ok;
                var party = await _repository.RetrieveByMember(request.Character);
                var member = party?.Members?.FirstOrDefault(m => m.ID == request.Character);

                if (party == null || member == null) result = PartyServiceResult.FailedNotInParty;

                if (result == PartyServiceResult.Ok)
                {
                    var targets = party.Members.ToList();
                    var isDisband = request.Character == party.Boss;

                    if (isDisband)
                    {
                        party.Members.Clear();
                        await _repository.Delete(party);
                    }
                    else
                    {
                        party.Members.Remove(member);
                        await _repository.Update(party);
                    }

                    await Task.WhenAll(targets.Select(async m =>
                    {
                        var packet = new UnstructuredOutgoingPacket(PacketSendOperations.PartyResult);

                        packet.WriteByte((byte)PartyResultCode.WithdrawParty_Done);
                        packet.WriteInt(party.ID);
                        packet.WriteInt(request.Character);
                        packet.WriteBool(!isDisband);

                        if (!isDisband)
                        {
                            packet.WriteBool(request.IsKick);
                            packet.WriteString(member.Name);
                            packet.WritePartyData(party, m.Channel);
                        }

                        var dispatchRequest = new DispatchToCharactersRequest { Data = ByteString.CopyFrom(packet.Buffer) };

                        dispatchRequest.Characters.Add(m.ID);

                        await _dispatcher.DispatchToCharacters(dispatchRequest);
                    }));
                    await _messenger.PublishAsync(new PartyUpdateEvent { Party = party });
                }

                await @lock.ReleaseAsync();

                return new PartyWithdrawResponse { Result = result };
            }

            return new PartyWithdrawResponse { Result = PartyServiceResult.FailedTimeout };
        }

        public async Task<PartyJoinResponse> Join(PartyJoinRequest request)
        {
            var source = new CancellationTokenSource();

            source.CancelAfter(PartyLockTimeoutDuration);

            var @lock = await _locker.AcquireAsync(PartyLockKey, cancellationToken: source.Token);

            if (@lock != null)
            {
                var result = PartyServiceResult.Ok;
                var party = await _repository.Retrieve(request.Party);

                if (await _repository.RetrieveByMember(request.Member.Id) != null) result = PartyServiceResult.FailedAlreadyInParty;
                if (party == null) result = PartyServiceResult.FailedNonExistentParty;
                else if (party.Members.Any(m => m.ID == request.Member.Id)) result = PartyServiceResult.FailedAlreadyInParty;
                else if (party.Members.Count >= 6) result = PartyServiceResult.FailedFullParty;

                if (result == PartyServiceResult.Ok)
                {
                    party.Members.Add(new PartyMemberRecord(request.Member));

                    await _repository.Update(party);
                    await Task.WhenAll(party.Members.Select(async m =>
                    {
                        var packet = new UnstructuredOutgoingPacket(PacketSendOperations.PartyResult);

                        packet.WriteByte((byte)PartyResultCode.JoinParty_Done);
                        packet.WriteInt(party.ID);
                        packet.WriteString(request.Member.Name);
                        packet.WritePartyData(party, m.Channel);

                        var dispatchRequest = new DispatchToCharactersRequest { Data = ByteString.CopyFrom(packet.Buffer) };

                        dispatchRequest.Characters.Add(m.ID);

                        await _dispatcher.DispatchToCharacters(dispatchRequest);
                    }));
                    await _messenger.PublishAsync(new PartyUpdateEvent { Party = party });
                }

                await @lock.ReleaseAsync();

                return new PartyJoinResponse { Result = result, Party = party?.ToContract() };
            }

            return new PartyJoinResponse { Result = PartyServiceResult.FailedTimeout };
        }

        public async Task<PartyChangeBossResponse> ChangeBoss(PartyChangeBossRequest request)
        {
            var source = new CancellationTokenSource();

            source.CancelAfter(PartyLockTimeoutDuration);

            var @lock = await _locker.AcquireAsync(PartyLockKey, cancellationToken: source.Token);

            if (@lock != null)
            {
                var result = PartyServiceResult.Ok;
                var party = await _repository.RetrieveByMember(request.Character);

                if (party == null) result = PartyServiceResult.FailedNotInParty;
                else if (party.Boss == request.Character) result = PartyServiceResult.FailedAlreadyBoss;

                if (result == PartyServiceResult.Ok)
                {
                    var targets = party.Members.Select(m => m.ID).ToImmutableList();
                    var packet = new UnstructuredOutgoingPacket(PacketSendOperations.PartyResult);

                    party.Boss = request.Character;

                    await _repository.Update(party);

                    packet.WriteByte((byte)PartyResultCode.ChangePartyBoss_Done);
                    packet.WriteInt(request.Character);
                    packet.WriteBool(request.IsDisconnect);

                    var dispatchRequest = new DispatchToCharactersRequest { Data = ByteString.CopyFrom(packet.Buffer) };

                    dispatchRequest.Characters.Add(targets);

                    await _dispatcher.DispatchToCharacters(dispatchRequest);
                    await _messenger.PublishAsync(new PartyUpdateEvent { Party = party });
                }

                await @lock.ReleaseAsync();

                return new PartyChangeBossResponse { Result = result };
            }

            return new PartyChangeBossResponse { Result = PartyServiceResult.FailedTimeout };
        }

        public async Task<PartyUpdateChangeLevelOrJobResponse> UpdateChangeLevelOrJob(PartyUpdateChangeLevelOrJobRequest request)
        {
            var source = new CancellationTokenSource();

            source.CancelAfter(PartyLockTimeoutDuration);

            var @lock = await _locker.AcquireAsync(PartyLockKey, cancellationToken: source.Token);

            if (@lock != null)
            {
                var result = PartyServiceResult.Ok;
                var party = await _repository.RetrieveByMember(request.Character);
                var member = party?.Members?.FirstOrDefault(m => m.ID == request.Character);

                if (party == null || member == null) result = PartyServiceResult.FailedNotInParty;

                if (result == PartyServiceResult.Ok)
                {
                    var targets = party.Members.Select(m => m.ID).ToImmutableList();
                    var packet = new UnstructuredOutgoingPacket(PacketSendOperations.PartyResult);

                    member.Level = request.Level;
                    member.Job = request.Job;

                    await _repository.Update(party);

                    packet.WriteByte((byte)PartyResultCode.ChangeLevelOrJob);
                    packet.WriteInt(member.ID);
                    packet.WriteInt(member.Level);
                    packet.WriteInt(member.Job);

                    var dispatchRequest = new DispatchToCharactersRequest { Data = ByteString.CopyFrom(packet.Buffer) };

                    dispatchRequest.Characters.Add(targets);

                    await _dispatcher.DispatchToCharacters(dispatchRequest);
                    await _messenger.PublishAsync(new PartyUpdateEvent { Party = party });
                }

                await @lock.ReleaseAsync();

                return new PartyUpdateChangeLevelOrJobResponse { Result = result };
            }

            return new PartyUpdateChangeLevelOrJobResponse { Result = PartyServiceResult.FailedTimeout };
        }

        public async Task<PartyUpdateUserMigrationResponse> UpdateUserMigration(PartyUpdateUserMigrationRequest request)
        {
            var source = new CancellationTokenSource();

            source.CancelAfter(PartyLockTimeoutDuration);

            var @lock = await _locker.AcquireAsync(PartyLockKey, cancellationToken: source.Token);

            if (@lock != null)
            {
                var result = PartyServiceResult.Ok;
                var party = await _repository.RetrieveByMember(request.Character);
                var member = party?.Members?.FirstOrDefault(m => m.ID == request.Character);

                if (party == null || member == null) result = PartyServiceResult.FailedNotInParty;

                if (result == PartyServiceResult.Ok)
                {
                    member.Channel = request.Channel;
                    member.Field = request.Field;

                    await _repository.Update(party);
                    await Task.WhenAll(party.Members.Select(async m =>
                    {
                        var packet = new UnstructuredOutgoingPacket(PacketSendOperations.PartyResult);

                        packet.WriteByte((byte)PartyResultCode.UserMigration);
                        packet.WriteInt(party.ID);
                        packet.WritePartyData(party, m.Channel);

                        var dispatchRequest = new DispatchToCharactersRequest { Data = ByteString.CopyFrom(packet.Buffer) };

                        dispatchRequest.Characters.Add(m.ID);

                        await _dispatcher.DispatchToCharacters(dispatchRequest);
                    }));
                    await _messenger.PublishAsync(new PartyUpdateEvent { Party = party });
                }

                await @lock.ReleaseAsync();

                return new PartyUpdateUserMigrationResponse { Result = result };
            }

            return new PartyUpdateUserMigrationResponse { Result = PartyServiceResult.FailedTimeout };
        }

        public async IAsyncEnumerable<PartyUpdateContract> Subscribe(CallContext context = default)
        {
            var channel = Channel.CreateBounded<PartyUpdateContract>(8);

            _channels.Add(channel);

            await foreach (var dispatch in channel.Reader.ReadAllAsync(context.CancellationToken))
                yield return dispatch;

            _channels.Remove(channel);
            channel.Writer.Complete();
        }
    }
}
