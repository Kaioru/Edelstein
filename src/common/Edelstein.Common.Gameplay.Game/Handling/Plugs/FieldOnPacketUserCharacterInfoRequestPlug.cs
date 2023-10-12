using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Models.Inventories;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class FieldOnPacketUserCharacterInfoRequestPlug : IPipelinePlug<FieldOnPacketUserCharacterInfoRequest>
{
    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserCharacterInfoRequest message)
    {
        var packet = new PacketWriter(PacketSendOperations.CharacterInfo);

        packet.WriteInt(message.Target.Character.ID);
        packet.WriteByte(message.Target.Character.Level);
        packet.WriteShort(message.Target.Character.Job);
        packet.WriteShort(message.Target.Character.POP);

        packet.WriteByte(0);

        packet.WriteString(""); // sGuild
        packet.WriteString(""); // sAlliance

        packet.WriteByte(0); // Medal?

        packet.WriteBool(false); // Pets

        packet.WriteByte(0); // TamingMobInfo

        var wishlist = message.Target.Character.Wishlist.Records
            .Where(c => c > 0)
            .ToImmutableList();
        
        packet.WriteByte((byte)wishlist.Count);
        foreach (var commodity in wishlist)
            packet.WriteInt(commodity);

        packet.WriteInt(0); // MedalAchievementInfo
        packet.WriteShort(0);

        var chairs = message.Target.Character.Inventories[ItemInventoryType.Install]?.Items
            .Select(kv => kv.Value)
            .Select(i => i.ID)
            .Where(i => i / 10000 == 301)
            .ToList() ?? new List<int>();
        
        packet.WriteInt(chairs.Count);
        foreach (var chair in chairs)
            packet.WriteInt(chair);

        await message.User.Dispatch(packet.Build());
    }
}
