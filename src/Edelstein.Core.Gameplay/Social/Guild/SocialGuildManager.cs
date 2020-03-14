using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Core.Distributed;
using Edelstein.Core.Gameplay.Migrations;
using Edelstein.Core.Gameplay.Social.Guild.Events;
using Edelstein.Core.Gameplay.Social.Messages;
using Edelstein.Core.Gameplay.Social.Party;
using Edelstein.Database;
using Edelstein.Entities.Characters;
using Edelstein.Entities.Social;
using Foundatio.Caching;
using Foundatio.Lock;
using MoreLinq;

namespace Edelstein.Core.Gameplay.Social.Guild
{
    public class SocialGuildManager : ISocialGuildManager
    {
        private const string GuildLockKey = "lock:guild";

        private readonly INode _node;
        private readonly IDataStore _store;
        private readonly ILockProvider _lockProvider;
        private readonly ICacheClient _characterCache;

        public SocialGuildManager(
            INode node,
            IDataStore store,
            ILockProvider lockProvider,
            ICacheClient cache
        )
        {
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

        private async Task BroadcastMessage<T>(ISocialGuild guild, T message) where T : class
        {
            var targets = (await _characterCache.GetAllAsync<INodeState>(
                    guild.Members.Select(m => m.CharacterID.ToString())
                )).Values
                .Where(t => t.HasValue)
                .Select(t => t.Value.Name)
                .Distinct();

            await Task.WhenAll((await _node.GetPeers())
                .Where(p => targets.Contains(p.State.Name))
                .Select(p => p.SendMessage<T>(message)));
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

        private async Task CreateInner(string name, ISocialParty party)
        {
            using var store = _store.StartSession();
            var members = store
                .Query<GuildMember>()
                .Where(m => party.Members.Any(c => c.CharacterID == m.CharacterID))
                .ToList();
            var record = store
                .Query<Entities.Social.Guild>()
                .Where(g => g.Name.ToLower() == name.ToLower())
                .FirstOrDefault();

            if (members.Count > 0)
                throw new GuildException("Creating guild when character already in guild");
            if (record != null)
                throw new GuildException("Creating guild with same name as another guild");

            record = new Entities.Social.Guild
            {
                Name = name
            };

            await store.InsertAsync(record);

            using var batch = _store.StartBatch();

            party.Members
                .Select(m => new GuildMember
                {
                    GuildID = record.ID,
                    CharacterID = m.CharacterID,
                    CharacterName = m.CharacterName,
                    Job = m.Job,
                    Level = m.Level,
                    Grade = party.BossCharacterID == m.CharacterID ? 1 : 2,
                    Online = m.ChannelID >= 0
                })
                .ForEach(m => batch.Insert(m));
            await batch.SaveChangesAsync();

            members = store
                .Query<GuildMember>()
                .Where(m => m.GuildID == record.ID)
                .ToList();

            var guild = new SocialGuild(this, record, members);

            await BroadcastMessage(guild, new GuildCreateEvent(
                record.ID,
                record,
                members
            ));
        }

        private async Task JoinInner(ISocialGuild guild, Character character)
        {
            if (guild.Members.Count >= guild.MaxMemberNum)
                throw new GuildException("Joining a full guild");

            using var store = _store.StartSession();
            var record = store
                .Query<Entities.Social.Guild>()
                .Where(p => p.ID == guild.ID)
                .FirstOrDefault();

            if (record == null)
                throw new GuildException("Joining non-existent guild");

            var member = store
                .Query<GuildMember>()
                .Where(m => m.CharacterID == character.ID)
                .FirstOrDefault();

            if (member != null)
                throw new GuildException("Joining guild when character already in guild");

            var members = store
                .Query<GuildMember>()
                .Where(m => m.GuildID == record.ID)
                .ToList();

            if (members.Any(m => m.CharacterID == character.ID))
                throw new GuildException("Joining already joined guild");

            member = new GuildMember
            {
                GuildID = guild.ID,
                CharacterID = character.ID,
                CharacterName = character.Name,
                Job = character.Job,
                Level = character.Level,
                Online = true
            };

            await store.InsertAsync(member);

            members.Add(member);
            guild = new SocialGuild(this, record, members);

            await BroadcastMessage(guild, new GuildJoinEvent(
                guild.ID,
                member.CharacterID,
                record,
                members
            ));
        }

        private async Task DisbandInner(ISocialGuild guild)
        {
            using var store = _store.StartSession();
            var record = store
                .Query<Entities.Social.Guild>()
                .Where(p => p.ID == guild.ID)
                .FirstOrDefault();
            if (record == null)
                throw new GuildException("Disbanding a non-existent guild");
            await Task.WhenAll(guild.Members
                .ToImmutableList()
                .Select(m => WithdrawInner(guild, m, true))
            );
            await store.DeleteAsync(record);
        }

        private async Task WithdrawInner(
            ISocialGuild guild,
            ISocialGuildMember member,
            bool disband = false,
            bool kick = false
        )
        {
            using var store = _store.StartSession();

            var record = store
                .Query<Entities.Social.Guild>()
                .Where(p => p.ID == guild.ID)
                .FirstOrDefault();

            if (record == null)
                throw new GuildException("Withdrawing from a non-existent guild");

            var memberRecord = store
                .Query<GuildMember>()
                .Where(m => m.GuildID == record.ID)
                .Where(m => m.CharacterID == member.CharacterID)
                .FirstOrDefault();

            if (memberRecord == null)
                throw new GuildException("Withdrawing from a guild that user is not apart of");

            await store.DeleteAsync(memberRecord);
            await BroadcastMessage(guild, new GuildWithdrawEvent(
                guild.ID,
                member.CharacterID,
                disband,
                kick,
                member.CharacterName
            ));
        }

        private Task KickInner(ISocialGuild guild, ISocialGuildMember member)
            => WithdrawInner(guild, member, kick: true);

        private async Task UpdateNotifyLoginOrLogoutInner(ISocialGuild guild, int characterID, bool online)
        {
            using var store = _store.StartSession();

            var record = store
                .Query<GuildMember>()
                .Where(m => m.GuildID == guild.ID)
                .Where(m => m.CharacterID == characterID)
                .FirstOrDefault();

            record.Online = online;
            record.DateLastLoginOrLogout = DateTime.UtcNow;
            await store.UpdateAsync(record);
            await BroadcastMessage(guild, new GuildNotifyLoginOrLogoutEvent(
                guild.ID,
                characterID,
                online
            ));
        }

        private async Task UpdateChangeLevelOrJobInner(ISocialGuild guild, int characterID, int level, int job)
        {
            using var store = _store.StartSession();

            var member = store
                .Query<GuildMember>()
                .Where(m => m.GuildID == guild.ID)
                .Where(m => m.CharacterID == characterID)
                .FirstOrDefault();

            if (member == null) return;

            member.Level = level;
            member.Job = job;
            await store.UpdateAsync(member);
            await BroadcastMessage(guild, new GuildChangeLevelOrJobEvent(
                guild.ID,
                characterID,
                level,
                job
            ));
        }

        private async Task UpdateSetGradeNameInner(ISocialGuild guild, string[] name)
        {
            if (name.Length != 5)
                throw new GuildException("Guild grade name not length of 5");

            using var store = _store.StartSession();

            var record = store
                .Query<Entities.Social.Guild>()
                .Where(m => m.ID == guild.ID)
                .FirstOrDefault();

            if (record == null) return;

            record.GradeName = name;
            await store.UpdateAsync(record);
            await BroadcastMessage(guild, new GuildSetGradeNameEvent(
                guild.ID,
                name
            ));
        }

        private async Task UpdateSetMemberGradeInner(ISocialGuild guild, int characterID, byte grade)
        {
            if (grade < 1 || grade > 5)
                throw new GuildException("Grade not within the bounds of 1 to 5");

            using var store = _store.StartSession();

            var member = store
                .Query<GuildMember>()
                .Where(m => m.GuildID == guild.ID)
                .Where(m => m.CharacterID == characterID)
                .FirstOrDefault();

            if (member == null) return;

            member.Grade = grade;
            await store.UpdateAsync(member);
            await BroadcastMessage(guild, new GuildSetMemberGradeEvent(
                guild.ID,
                characterID,
                grade
            ));
        }

        private async Task UpdateSetMarkInner(ISocialGuild guild, short markBg, byte markBgColor, short mark,
            byte markColor)
        {
            // TODO: Template validation
            using var store = _store.StartSession();

            var record = store
                .Query<Entities.Social.Guild>()
                .Where(m => m.ID == guild.ID)
                .FirstOrDefault();

            if (record == null) return;

            record.MarkBg = markBg;
            record.MarkBgColor = markBgColor;
            record.Mark = mark;
            record.MarkColor = markColor;
            await store.UpdateAsync(record);
            await BroadcastMessage(guild, new GuildSetMarkEvent(
                guild.ID,
                markBg,
                markBgColor,
                mark,
                markColor
            ));
        }

        private async Task UpdateSetNoticeInner(ISocialGuild guild, string notice)
        {
            using var store = _store.StartSession();

            var record = store
                .Query<Entities.Social.Guild>()
                .Where(m => m.ID == guild.ID)
                .FirstOrDefault();

            if (record == null) return;

            record.Notice = notice;
            await store.UpdateAsync(record);
            await BroadcastMessage(guild, new GuildSetNoticeEvent(
                guild.ID,
                notice
            ));
        }

        public Task<ISocialGuild> Load(int guildID)
            => Lock(() => LoadInner(guildID));

        public Task<ISocialGuild?> Load(Character character)
            => Lock(() => LoadInner(character));

        public Task Create(string name, ISocialParty party)
            => Lock(() => CreateInner(name, party));

        public Task Join(ISocialGuild guild, Character character)
            => Lock(() => JoinInner(guild, character));

        public Task Disband(ISocialGuild guild)
            => Lock(() => DisbandInner(guild));

        public Task Withdraw(ISocialGuild guild, ISocialGuildMember member)
            => Lock(() => WithdrawInner(guild, member));

        public Task Kick(ISocialGuild guild, ISocialGuildMember member)
            => Lock(() => KickInner(guild, member));

        public Task Chat(ISocialGuild guild, string name, string text)
            => BroadcastMessage(guild, new GroupMessageEvent(
                GroupMessageType.Guild,
                guild.Members
                    .Select(m => m.CharacterID)
                    .ToImmutableList(),
                name,
                text
            ));

        public Task UpdateNotifyLoginOrLogout(ISocialGuild guild, int characterID, bool online)
            => Lock(() => UpdateNotifyLoginOrLogoutInner(guild, characterID, online));

        public Task UpdateChangeLevelOrJob(ISocialGuild guild, int characterID, int level, int job)
            => Lock(() => UpdateChangeLevelOrJobInner(guild, characterID, level, job));

        public Task UpdateSetGradeName(ISocialGuild guild, string[] name)
            => Lock(() => UpdateSetGradeNameInner(guild, name));

        public Task UpdateSetMemberGrade(ISocialGuild guild, int characterID, byte grade)
            => Lock(() => UpdateSetMemberGradeInner(guild, characterID, grade));

        public Task UpdateSetMark(ISocialGuild guild, short markBg, byte markBgColor, short mark, byte markColor)
            => Lock(() => UpdateSetMarkInner(guild, markBg, markBgColor, mark, markColor));

        public Task UpdateSetNotice(ISocialGuild guild, string notice)
            => Lock(() => UpdateSetNoticeInner(guild, notice));
    }
}