using System;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Core.Distributed;
using Edelstein.Core.Gameplay.Migrations;
using Edelstein.Core.Gameplay.Social.Party;
using Edelstein.Database;
using Edelstein.Entities.Characters;
using Edelstein.Entities.Social;
using Foundatio.Caching;
using Foundatio.Lock;

namespace Edelstein.Core.Gameplay.Social.Guild
{
    public class SocialGuildManager : ISocialGuildManager
    {
        private const string GuildLockKey = "lock:guild";

        private readonly int _channelID;
        private readonly INode _node;
        private readonly IDataStore _store;
        private readonly ILockProvider _lockProvider;
        private readonly ICacheClient _characterCache;

        public SocialGuildManager(
            int channelID,
            INode node,
            IDataStore store,
            ILockProvider lockProvider,
            ICacheClient cache
        )
        {
            _channelID = channelID;
            _node = node;
            _store = store;
            _lockProvider = lockProvider;
            _characterCache = new ScopedCacheClient(cache, MigrationScopes.StateCharacter);
        }

        private async Task Lock(Func<Task> func)
        {
            var token = new CancellationTokenSource();

            token.CancelAfter(TimeSpan.FromSeconds(5));

            var @lock =
                await _lockProvider.AcquireAsync(
                    GuildLockKey,
                    cancellationToken: token.Token
                );

            if (@lock == null)
                throw new GuildException("Request timed out");

            try
            {
                await func.Invoke();
            }
            catch (Exception e)
            {
                throw new GuildException(e.Message);
            }
            finally
            {
                await @lock.ReleaseAsync();
            }
        }

        private async Task<T> Lock<T>(Func<Task<T>> func)
        {
            var token = new CancellationTokenSource();

            token.CancelAfter(TimeSpan.FromSeconds(5));

            var @lock =
                await _lockProvider.AcquireAsync(
                    GuildLockKey,
                    cancellationToken: token.Token
                );

            if (@lock == null)
                throw new GuildException("Request timed out");

            try
            {
                return await func.Invoke();
            }
            catch (Exception e)
            {
                throw new GuildException(e.Message);
            }
            finally
            {
                await @lock.ReleaseAsync();
            }
        }

        private Task<ISocialGuild> LoadInner(int guildID)
        {
            using var store = _store.StartSession();
            var record = store
                .Query<Entities.Social.Guild>()
                .Where(p => p.ID == guildID)
                .FirstOrDefault();

            if (record == null)
                throw new GuildException("Tried to load non-existent guild");

            var members = store
                .Query<GuildMember>()
                .Where(m => m.GuildID == record.ID)
                .ToImmutableList();

            return Task.FromResult<ISocialGuild?>(new SocialGuild(this, record, members));
        }

        private Task<ISocialGuild?> LoadInner(Character character)
        {
            using var store = _store.StartSession();
            var member = store
                .Query<GuildMember>()
                .Where(m => m.CharacterID == character.ID)
                .FirstOrDefault();

            if (member == null) return Task.FromResult<ISocialGuild?>(null);
            return LoadInner(member.GuildID);
        }

        private Task CreateInner(Character character)
        {
            throw new System.NotImplementedException();
        }

        private Task JoinInner(ISocialGuild guild, Character character)
        {
            throw new System.NotImplementedException();
        }

        private Task DisbandInner(ISocialGuild guild)
        {
            throw new System.NotImplementedException();
        }

        private Task WithdrawInner(ISocialGuild guild, ISocialGuildMember member)
        {
            throw new System.NotImplementedException();
        }

        private Task KickInner(ISocialGuild guild, ISocialGuildMember member)
        {
            throw new System.NotImplementedException();
        }

        private Task UpdateChangeLevelOrJobInner(ISocialParty guild, int characterID, int level, int job)
        {
            throw new System.NotImplementedException();
        }

        private Task UpdateChangeJobInner(ISocialParty guild, int characterID, int job)
        {
            throw new System.NotImplementedException();
        }

        private Task UpdateSetGradeNameInner(ISocialParty guild, int characterID, string[] name)
        {
            throw new System.NotImplementedException();
        }

        private Task UpdateSetMemberGradeInner(ISocialParty guild, int characterID, int grade)
        {
            throw new System.NotImplementedException();
        }

        private Task UpdateSetMarkInner(ISocialGuild guild, short markBG, byte markBGColor, short mark, byte markColor)
        {
            throw new System.NotImplementedException();
        }

        private Task UpdateSetNoticeInner(ISocialGuild guild, string notice)
        {
            throw new System.NotImplementedException();
        }

        public Task<ISocialGuild> Load(int guildID)
            => Lock(() => LoadInner(guildID));

        public Task<ISocialGuild?> Load(Character character)
            => Lock(() => LoadInner(character));

        public Task Create(Character character)
            => Lock(() => CreateInner(character));

        public Task Join(ISocialGuild guild, Character character)
            => Lock(() => JoinInner(guild, character));

        public Task Disband(ISocialGuild guild)
            => Lock(() => DisbandInner(guild));

        public Task Withdraw(ISocialGuild guild, ISocialGuildMember member)
            => Lock(() => WithdrawInner(guild, member));

        public Task Kick(ISocialGuild guild, ISocialGuildMember member)
            => Lock(() => KickInner(guild, member));

        public Task UpdateChangeLevelOrJob(ISocialParty guild, int characterID, int level, int job)
            => Lock(() => UpdateChangeLevelOrJobInner(guild, characterID, level, job));

        public Task UpdateSetGradeName(ISocialParty guild, int characterID, string[] name)
            => Lock(() => UpdateSetGradeName(guild, characterID, name));

        public Task UpdateSetMemberGrade(ISocialParty guild, int characterID, int grade)
            => Lock(() => UpdateSetMemberGradeInner(guild, characterID, grade));

        public Task UpdateSetMark(ISocialGuild guild, short markBG, byte markBGColor, short mark, byte markColor)
            => Lock(() => UpdateSetMarkInner(guild, markBG, markBGColor, mark, markColor));

        public Task UpdateSetNotice(ISocialGuild guild, string notice)
            => Lock(() => UpdateSetNoticeInner(guild, notice));
    }
}