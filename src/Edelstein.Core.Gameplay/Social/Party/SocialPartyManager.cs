using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Core.Gameplay.Migrations;
using Edelstein.Core.Utils;
using Edelstein.Database;
using Edelstein.Entities.Characters;
using Edelstein.Entities.Social;
using Foundatio.Caching;
using Foundatio.Lock;

namespace Edelstein.Core.Gameplay.Social.Party
{
    public class SocialPartyManager : ISocialPartyManager
    {
        public const string PartyLockKey = "lock:party";
        public const string PartyInviteKey = "party:invite";

        private readonly IDataStore _store;
        private readonly ICacheClient _characterCache;
        private readonly ICacheClient _inviteCache;
        private readonly ILockProvider _lockProvider;
        private readonly int _channelID;

        public SocialPartyManager(IDataStore store, ICacheClient cache, ILockProvider lockProvider, int channelID)
        {
            _store = store;
            _characterCache = new ScopedCacheClient(cache, MigrationScopes.StateCharacter);
            _inviteCache = new ScopedCacheClient(cache, PartyInviteKey);
            _lockProvider = lockProvider;
            _channelID = channelID;
        }

        private async Task<ILock> Lock()
        {
            var token = new CancellationTokenSource();
            token.CancelAfter(TimeSpan.FromSeconds(3));
            var @lock = await _lockProvider.AcquireAsync(
                PartyLockKey,
                cancellationToken: token.Token
            );

            if (@lock == null) throw new TimeoutException();
            return @lock;
        }

        public Task Unlock(ILock @lock)
            => @lock.ReleaseAsync();

        public async Task<ISocialParty?> Load(Character character)
        {
            var @lock = await Lock();

            try
            {
                using var store = _store.StartSession();
                var party = store
                    .Query<Entities.Social.Party>()
                    .Where(p => p.Members.Count(m => m.CharacterID == character.ID) > 0)
                    .First();

                return new SocialParty(this, party);
            }
            catch
            {
                return null;
            }
            finally
            {
                await Unlock(@lock);
            }
        }

        public async Task<ISocialParty> Create(Character character)
        {
            var @lock = await Lock();

            try
            {
                using var store = _store.StartSession();
                var party = new Entities.Social.Party();
                var member = new PartyMember
                {
                    CharacterID = character.ID,
                    CharacterName = character.Name,
                    Job = character.Job,
                    Level = character.Level,
                    ChannelID = _channelID
                };

                party.BossCharacterID = character.ID;
                party.Members.Add(member);
                
                store.Insert(party);

                return new SocialParty(this, party);
            }
            catch
            {
                return null;
            }
            finally
            {
                await Unlock(@lock);
            }
        }

        public Task Join(ISocialParty party, Character character)
        {
            throw new System.NotImplementedException();
        }

        public Task Invite(ISocialParty party, Character character)
        {
            throw new System.NotImplementedException();
        }

        public Task ChangeBoss(ISocialParty party, ISocialPartyMember member)
        {
            throw new System.NotImplementedException();
        }

        public Task Withdraw(ISocialParty party, ISocialPartyMember member)
        {
            throw new System.NotImplementedException();
        }

        public Task Kick(ISocialParty party, ISocialPartyMember member)
        {
            throw new System.NotImplementedException();
        }

        public Task Disband(ISocialParty party)
        {
            throw new System.NotImplementedException();
        }
    }
}