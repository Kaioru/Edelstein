using System;
using System.Threading.Tasks;
using Edelstein.Core.Entities.Social;

namespace Edelstein.Core.Gameplay.Social.Guild
{
    public class SocialGuildMember : ISocialGuildMember
    {
        private readonly ISocialGuildManager _manager;
        private readonly ISocialGuild _guild;
        private readonly GuildMember _member;

        public int CharacterID => _member.CharacterID;
        public string CharacterName => _member.CharacterName;
        public int Job => _member.Job;
        public int Level => _member.Level;
        public int Grade => _member.Grade;
        public bool Online => _member.Online;
        public int Commitment => _member.Commitment;
        public bool Inactive { get; private set; }

        public SocialGuildMember(
            ISocialGuildManager manager,
            ISocialGuild guild,
            GuildMember member
        )
        {
            _manager = manager;
            _guild = guild;
            _member = member;

            Inactive = (DateTime.UtcNow - _member.DateLastLoginOrLogout).TotalDays >= 7;
        }

        public Task Withdraw()
            => _guild.Withdraw(this);

        public Task Kick()
            => _guild.Kick(this);

        public Task UpdateNotifyLoginOrLogout(bool online)
            => _guild.UpdateNotifyLoginOrLogout(CharacterID, online);

        public Task UpdateChangeLevelOrJob(int level, int job)
            => _guild.UpdateChangeLevelOrJob(CharacterID, level, job);

        public Task UpdateSetMemberGrade(byte grade)
            => _guild.UpdateSetMemberGrade(CharacterID, grade);

        public Task OnUpdateNotifyLoginOrLogout(bool online)
        {
            _member.Online = online;
            Inactive = false;
            return Task.CompletedTask;
        }

        public Task OnUpdateChangeLevelOrJob(int level, int job)
        {
            _member.Level = level;
            _member.Job = job;
            return Task.CompletedTask;
        }

        public Task OnUpdateSetMemberGrade(int grade)
        {
            _member.Grade = grade;
            return Task.CompletedTask;
        }
    }
}