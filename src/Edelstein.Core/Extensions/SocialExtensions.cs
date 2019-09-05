using System;
using Edelstein.Core.Gameplay.Social;
using Edelstein.Network.Packets;

namespace Edelstein.Core.Extensions
{
    public static class SocialExtensions
    {
        public static void Encode(this PartyData data, IPacket p)
        {
            var members = new PartyMemberData[6];

            for (var i = 0; i < 6; i++)
                members[i] = i < data.Members.Count
                    ? data.Members[i]
                    : new PartyMemberData();

            foreach (var member in members)
                p.Encode<int>(member.CharacterID);
            foreach (var member in members)
                p.EncodeFixedString(member.Name, 13);
            foreach (var member in members)
                p.Encode<int>(member.Job);
            foreach (var member in members)
                p.Encode<int>(member.Level);
            foreach (var member in members)
                p.Encode<int>(member.ChannelID);

            p.Encode<int>(data.BossCharacterID);

            foreach (var member in members)
                p.Encode<int>(member.FieldID);

            // TODO: town portal
            foreach (var member in members)
            {
                p.Encode<int>(999999999); // m_dwTownID
                p.Encode<int>(999999999); // m_dwFieldID
                p.Encode<int>(0); // m_nSKillID
                p.Encode<int>(0); // m_ptFieldPortal x
                p.Encode<int>(0); // m_ptFieldPortal y
            }

            foreach (var member in members)
                p.Encode<int>(0); // aPQReward
            foreach (var member in members)
                p.Encode<int>(0); // aPQRewardType

            p.Encode<int>(0); // dwPQRewardMobTemplateID
            p.Encode<int>(0); // bPQReward
        }
    }
}