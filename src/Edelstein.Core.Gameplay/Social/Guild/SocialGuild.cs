using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Edelstein.Entities.Social;

namespace Edelstein.Core.Gameplay.Social.Guild
{
    public class SocialGuild : ISocialGuild
    {
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
        public int Level => _guild.Level;

        public SocialGuild(Entities.Social.Guild guild)
        {
            _guild = guild;

            Members = guild.Members
                .Select<GuildMember, ISocialGuildMember>(p => new SocialGuildMember(this, p))
                .ToImmutableList();
        }
    }
}