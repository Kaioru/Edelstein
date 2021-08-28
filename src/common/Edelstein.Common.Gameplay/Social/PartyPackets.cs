using System;
using System.Linq;
using Edelstein.Protocol.Gameplay.Social;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Social
{
    public static class PartyPackets
    {
        public static void WritePartyData(this IPacketWriter writer, IParty p, int channelID = -1)
        {
            var members = p.Members.ToArray();

            Array.Resize(ref members, 6);

            foreach (var member in members)
                writer.WriteInt(member?.ID ?? 0);
            foreach (var member in members)
                writer.WriteString(member?.Name ?? "", 13);
            foreach (var member in members)
                writer.WriteInt(member?.Job ?? 0);
            foreach (var member in members)
                writer.WriteInt(member?.Level ?? 0);
            foreach (var member in members)
                writer.WriteInt(member?.Channel ?? 0);

            writer.WriteInt(p.Boss);

            foreach (var member in members)
                writer.WriteInt(member?.Channel == channelID
                    ? member.Field
                    : -1
                );

            foreach (var member in members)
            {
                // TownPortal;
                writer.WriteInt(0); // TownID
                writer.WriteInt(0); // FieldID
                writer.WriteInt(0); // SkillID
                writer.WriteLong(0); // FieldPortal X
                writer.WriteLong(0); // FieldPortal Y
            }

            foreach (var member in members)
                writer.WriteInt(0); // PQReward
            foreach (var member in members)
                writer.WriteInt(0); // PQRewardType

            writer.WriteInt(0); // PQRewardMobTemplateID
            writer.WriteInt(0); // PQReward
        }
    }
}