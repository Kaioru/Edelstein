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
            members.ForEach(m =>
            {
                p.EncodeFixedString(m.CharacterName, 13);
                p.Encode<int>(m.Job);
                p.Encode<int>(m.Level);
                p.Encode<int>(m.Grade);
                p.Encode<int>(m.Online ? 1 : 0);
                p.Encode<int>(m.Commitment);
                p.Encode<int>(0); // AllianceGrade
            });

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
    }
}