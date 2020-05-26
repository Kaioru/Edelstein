using System;
using System.Linq;
using Edelstein.Core.Network.Packets;

namespace Edelstein.Core.Gameplay.Social.Party
{
    public static class SocialPartyPackets
    {
        public static void EncodeData(this ISocialParty party, int channelID, IPacketEncoder p)
        {
            var members = party.Members.ToArray();

            Array.Resize(ref members, 6);

            foreach (var member in members)
                p.EncodeInt(member?.CharacterID ?? 0);
            foreach (var member in members)
                p.EncodeString(member?.CharacterName ?? "", 13);
            foreach (var member in members)
                p.EncodeInt(member?.Job ?? 0);
            foreach (var member in members)
                p.EncodeInt(member?.Level ?? 0);
            foreach (var member in members)
                p.EncodeInt(member?.ChannelID ?? 0);
            p.EncodeInt(party.BossCharacterID);

            foreach (var member in members)
                p.EncodeInt(member?.ChannelID == channelID
                    ? member.FieldID
                    : -1
                );
            foreach (var member in members)
            {
                // TownPortal;
                p.EncodeInt(0); // TownID
                p.EncodeInt(0); // FieldID
                p.EncodeInt(0); // SkillID
                p.EncodeLong(0); // FieldPortal X
                p.EncodeLong(0); // FieldPortal Y
            }

            foreach (var member in members)
                p.EncodeInt(0); // PQReward
            foreach (var member in members)
                p.EncodeInt(0); // PQRewardType

            p.EncodeInt(0); // PQRewardMobTemplateID
            p.EncodeInt(0); // PQReward
        }
    }
}