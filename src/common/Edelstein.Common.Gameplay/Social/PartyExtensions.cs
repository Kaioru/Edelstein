using System.Collections.Immutable;
using Edelstein.Protocol.Services.Social;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Social;

public static class PartyExtensions
{
    public static void WritePartyInfo(this IPacketWriter writer, IPartyMembership party)
    {
        var members = party.Members.Values.ToImmutableList();
        
        for (var i = 0; i < 6; i++)
            writer.WriteInt(members.ElementAtOrDefault(i)?.CharacterID ?? 0);
        for (var i = 0; i < 6; i++)
            writer.WriteString(members.ElementAtOrDefault(i)?.CharacterName ?? string.Empty, 13);
        for (var i = 0; i < 6; i++)
            writer.WriteInt(members.ElementAtOrDefault(i)?.Job ?? 0);
        for (var i = 0; i < 6; i++)
            writer.WriteInt(members.ElementAtOrDefault(i)?.Level ?? 0);
        for (var i = 0; i < 6; i++)
            writer.WriteInt(members.ElementAtOrDefault(i)?.ChannelID ?? -2);

        writer.WriteInt(party.BossCharacterID);
            
        for (var i = 0; i < 6; i++)
            writer.WriteInt(members.ElementAtOrDefault(i)?.FieldID ?? 999999999);

        for (var i = 0; i < 6; i++)
        {
            writer.WriteInt(0);
            writer.WriteInt(0);
            writer.WriteInt(0);
            writer.WriteInt(0);
            writer.WriteInt(0);
        }
            
        for (var i = 0; i < 6; i++)
            writer.WriteInt(0);
        for (var i = 0; i < 6; i++)
            writer.WriteInt(0);

        writer.WriteInt(0);
        writer.WriteInt(0);
    }
}
