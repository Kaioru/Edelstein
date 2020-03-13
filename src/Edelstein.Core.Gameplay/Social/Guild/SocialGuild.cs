using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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
                .ToImmutableList();
        }
    }
}