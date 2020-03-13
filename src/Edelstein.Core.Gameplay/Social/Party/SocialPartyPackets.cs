using System;
using System.Collections.Immutable;
using System.Linq;
using Baseline;
using Edelstein.Network.Packets;

namespace Edelstein.Core.Gameplay.Social.Party
{
    public static class SocialPartyPackets
    {
        public static void EncodeData(this ISocialParty party, int channelID, IPacket p)
        {
            var members = party.Members.ToArray();

            Array.Resize(ref members, 6);

            foreach (var member in members)
                p.Encode<int>(member?.CharacterID ?? 0);
            foreach (var member in members)
                p.EncodeFixedString(member?.CharacterName ?? "", 13);
            foreach (var member in members)
                p.Encode<int>(member?.Job ?? 0);
            foreach (var member in members)
                p.Encode<int>(member?.Level ?? 0);
            foreach (var member in members)
                p.Encode<int>(member?.ChannelID ?? 0);
            p.Encode<int>(party.BossCharacterID);

            foreach (var member in members)
                p.Encode<int>(member?.ChannelID == channelID
                    ? member.FieldID
                    : -1
                );
            foreach (var member in members)
            {
                // TownPortal;
                p.Encode<int>(0); // TownID
                p.Encode<int>(0); // FieldID
                p.Encode<int>(0); // SkillID
                p.Encode<long>(0); // FieldPortal X
                p.Encode<long>(0); // FieldPortal Y
            }

            foreach (var member in members)
                p.Encode<int>(0); // PQReward
            foreach (var member in members)
                p.Encode<int>(0); // PQRewardType

            p.Encode<int>(0); // PQRewardMobTemplateID
            p.Encode<int>(0); // PQReward
        }
    }
}