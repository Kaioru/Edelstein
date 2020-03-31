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
            p.EncodeInt(guild.ID);
            p.EncodeString(guild.Name);

            guild.GradeName.ForEach(n => p.EncodeString(n));

            var members = guild.Members.ToImmutableList();

            p.EncodeByte((byte) members.Count);
            members.ForEach(m =>
                p.EncodeInt(m.CharacterID)
            );
            members.ForEach(m => m.EncodeData(p));

            p.EncodeInt(guild.MaxMemberNum);
            p.EncodeShort(guild.MarkBg);
            p.EncodeByte(guild.MarkBgColor);
            p.EncodeShort(guild.Mark);
            p.EncodeByte(guild.MarkColor);

            p.EncodeString(guild.Notice);
            p.EncodeInt(guild.Point);

            p.EncodeInt(0); // AllianceID

            p.EncodeByte(guild.Level);

            p.EncodeShort(0); // SkillRecord
        }

        public static void EncodeData(this ISocialGuildMember member, IPacket p)
        {
            p.EncodeString(member.CharacterName, 13);
            p.EncodeInt(member.Inactive ? -1 : member.Job);
            p.EncodeInt(member.Inactive ? -1 : member.Level);
            p.EncodeInt(member.Grade);
            p.EncodeInt(member.Online ? 1 : 0);
            p.EncodeInt(member.Commitment);
            p.EncodeInt(0); // AllianceGrade
        }
    }
}