using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Entities.Social;

namespace Edelstein.Core.Gameplay.Social.Guild
{
    public class SocialGuild : ISocialGuild
    {
        private readonly ISocialGuildManager _manager;
        private readonly Entities.Social.Guild _guild;

        public int ID => _guild.ID;
        public string Name => _guild.Name;

        public string[] GradeName => _guild.GradeName;

        public int MaxMemberNum => _guild.MaxMemberNum;
        public ICollection<ISocialGuildMember> Members { get; }

        public short MarkBg => _guild.MarkBg;
        public byte MarkBgColor => _guild.MarkBgColor;
        public short Mark => _guild.Mark;
        public byte MarkColor => _guild.MarkColor;

        public string Notice => _guild.Notice;
        public int Point => _guild.Point;
        public byte Level => _guild.Level;

        public SocialGuild(
            ISocialGuildManager manager,
            Entities.Social.Guild guild,
            IEnumerable<GuildMember> members
        )
        {
            _manager = manager;
            _guild = guild;

            Members = members
                .Select<GuildMember, ISocialGuildMember>(p => new SocialGuildMember(_manager, this, p))
                .ToList();
        }

        public Task OnUpdateJoin(ISocialGuildMember member)
        {
            Members.Add(member);
            return Task.CompletedTask;
        }

        public Task OnUpdateWithdraw(int characterID)
        {
            Members.Remove(Members.FirstOrDefault(m => m.CharacterID == characterID));
            return Task.CompletedTask;
        }

        public Task OnUpdateNotifyLoginOrLogout(int characterID, bool online)
            => Members
                .FirstOrDefault(m => m.CharacterID == characterID)
                ?.OnUpdateNotifyLoginOrLogout(online);

        public Task OnUpdateChangeLevelOrJob(int characterID, int level, int job)
            => Members
                .FirstOrDefault(m => m.CharacterID == characterID)
                ?.OnUpdateChangeLevelOrJob(level, job);

        public Task OnUpdateSetGradeName(string[] name)
        {
            Array.Copy(name, _guild.GradeName, 5);
            return Task.CompletedTask;
        }

        public Task OnUpdateSetMemberGrade(int characterID, int grade)
            => Members
                .FirstOrDefault(m => m.CharacterID == characterID)
                ?.OnUpdateSetMemberGrade(grade);

        public Task OnUpdateSetMark(short markBG, byte markBGColor, short mark, byte markColor)
        {
            _guild.MarkBg = markBG;
            _guild.MarkBgColor = markBGColor;
            _guild.Mark = mark;
            _guild.MarkColor = markColor;
            return Task.CompletedTask;
        }

        public Task OnUpdateSetNotice(string notice)
        {
            _guild.Notice = notice;
            return Task.CompletedTask;
        }
    }
}