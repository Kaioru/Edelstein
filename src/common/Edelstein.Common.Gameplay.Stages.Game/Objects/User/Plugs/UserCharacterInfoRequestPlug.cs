using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Inventories;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts.Pipelines;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Plugs;

public class UserCharacterInfoRequestPlug : IPipelinePlug<IUserCharacterInfoRequest>
{
    public async Task Handle(IPipelineContext ctx, IUserCharacterInfoRequest message)
    {
        var obj = message.User.Field?.GetPool(FieldObjectType.User)?.GetObject(message.CharacterID);

        if (obj is not IFieldUser target) return;

        var packet = new PacketWriter(PacketSendOperations.CharacterInfo);

        packet.WriteInt(target.Character.ID);
        packet.WriteByte(target.Character.Level);
        packet.WriteShort(target.Character.Job);
        packet.WriteShort(target.Character.POP);

        packet.WriteByte(0);

        packet.WriteString(""); // sGuild
        packet.WriteString(""); // sAlliance

        packet.WriteByte(0); // Medal?

        packet.WriteBool(false); // Pets

        packet.WriteByte(0); // TamingMobInfo
        packet.WriteByte(0); // Wishlist

        packet.WriteInt(0); // MedalAchievementInfo
        packet.WriteShort(0);

        var chairs = target.Character.Inventories[ItemInventoryType.Install].Items
            .Select(kv => kv.Value.ID)
            .Where(i => i / 10000 == 301)
            .ToList();

        packet.WriteInt(chairs.Count);
        foreach (var chair in chairs) packet.WriteInt(chair);

        await message.User.Dispatch(packet);
    }
}
