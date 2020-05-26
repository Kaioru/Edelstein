using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Utils.Packets;
using Edelstein.Entities.Inventories;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Handlers.Users
{
    public class UserCharacterInfoRequestHandler : AbstractFieldUserHandler
    {
        protected override async Task Handle(
            FieldUser user,
            RecvPacketOperations operation,
            IPacketDecoder packet
        )
        {
            packet.DecodeInt();
            var target = user.GetWatchedObject<FieldUser>(packet.DecodeInt());

            if (target == null) return;

            using var p = new OutPacket(SendPacketOperations.CharacterInfo);
            var c = target.Character;

            p.EncodeInt(target.ID);
            p.EncodeByte(c.Level);
            p.EncodeShort(c.Job);
            p.EncodeShort(c.POP); // TODO: use basic stat POP

            p.EncodeByte(0);

            p.EncodeString(target.Guild?.Name ?? "");
            p.EncodeString(""); // sAlliance

            p.EncodeByte(0); // Medal?

            p.EncodeBool(false); // Pets

            p.EncodeByte(0); // TamingMobInfo
            p.EncodeByte(0); // Wishlist

            p.EncodeInt(0); // MedalAchievementInfo
            p.EncodeShort(0);

            var chairs = c.Inventories[ItemInventoryType.Install].Items
                .Select(kv => kv.Value)
                .Select(i => i.TemplateID)
                .Where(i => i / 10000 == 301)
                .ToList();
            p.EncodeInt(chairs.Count);
            chairs.ForEach(i => p.EncodeInt(i));
            await user.SendPacket(p);
        }
    }
}