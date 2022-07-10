using Edelstein.Protocol.Gameplay.Social;
using Edelstein.Protocol.Network;
using MoreLinq;

namespace Edelstein.Common.Gameplay.Social
{
    public static class GuildPackets
    {
        public static void WriteGuildData(this IPacketWriter writer, IGuild g)
        {
            writer.WriteInt(g.ID);
            writer.WriteString(g.Name);

            for (var i = 0; i < 6; i++)
            {
                if (i < g.Grade.Length) writer.WriteString(g.Grade[i]);
                else writer.WriteString(string.Empty);
            }

            writer.WriteByte((byte)g.Members.Count);

            g.Members.ForEach(m => writer.WriteInt(m.ID));
            g.Members.ForEach(m => writer.WriteGuildMemberData(m));

            writer.WriteInt(g.MaxMemberNum);

            writer.WriteShort(g.MarkBg);
            writer.WriteByte(g.MarkBgColor);
            writer.WriteShort(g.Mark);
            writer.WriteByte(g.MarkColor);

            writer.WriteString(g.Notice);
            writer.WriteInt(g.Point);

            writer.WriteInt(0); // Alliance

            writer.WriteByte(g.Level);

            writer.WriteShort(0); // GuildSkills
        }

        public static void WriteGuildMemberData(this IPacketWriter writer, IGuildMember m)
        {
            writer.WriteString(m.Name, 13);
            writer.WriteInt(m.Job);
            writer.WriteInt(m.Level);
            writer.WriteInt(m.Grade);
            writer.WriteInt(m.Online ? 1 : 0);
            writer.WriteInt(m.Commitment);
            writer.WriteInt(0); // AllianceGrade
        }
    }
}