using System;
using System.Collections.Immutable;
using Edelstein.Network.Packets;
using MoreLinq;

namespace Edelstein.Core.Gameplay.Social.Guild
{
    public static class SocialGuildPackets
    {
        public static void EncodeData(this ISocialGuild guild, IPacket p)
        {
            p.Encode<int>(guild.ID);
            p.Encode<string>(guild.Name);

            guild.GradeName.ForEach(n => p.Encode<string>(n));

            var members = guild.Members.ToImmutableList();

            p.Encode<byte>((byte) members.Count);
            members.ForEach(m =>
                p.Encode<int>(m.CharacterID)
            );
            members.ForEach(m => m.EncodeData(p));

            p.Encode<int>(guild.MaxMemberNum);
            p.Encode<short>(guild.MarkBg);
            p.Encode<byte>(guild.MarkBgColor);
            p.Encode<short>(guild.Mark);
            p.Encode<byte>(guild.MarkColor);

            p.Encode<string>(guild.Notice);
            p.Encode<int>(guild.Point);

            p.Encode<int>(0); // AllianceID

            p.Encode<byte>(guild.Level);

            p.Encode<short>(0); // SkillRecord
        }

        public static void EncodeData(this ISocialGuildMember member, IPacket p)
        {
            p.EncodeFixedString(member.CharacterName, 13);
            p.Encode<int>(member.Inactive ? -1 : member.Job);
            p.Encode<int>(member.Inactive ? -1 : member.Level);
            p.Encode<int>(member.Grade);
            p.Encode<int>(member.Online ? 1 : 0);
            p.Encode<int>(member.Commitment);
            p.Encode<int>(0); // AllianceGrade
        }
    }
}