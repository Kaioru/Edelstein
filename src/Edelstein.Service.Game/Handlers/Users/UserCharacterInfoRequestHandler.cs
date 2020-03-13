using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Utils;
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
            IPacket packet
        )
        {
            packet.Decode<int>();
            var target = user.Field.GetObject<FieldUser>(packet.Decode<int>());

            if (target == null) return;

            using var p = new Packet(SendPacketOperations.CharacterInfo);
            var c = target.Character;

            p.Encode<int>(target.ID);
            p.Encode<byte>(c.Level);
            p.Encode<short>(c.Job);
            p.Encode<short>(c.POP); // TODO: use basic stat POP

            p.Encode<byte>(0);

            p.Encode<string>(target.Guild?.Name ?? "");
            p.Encode<string>(""); // sAlliance

            p.Encode<byte>(0); // Medal?

            p.Encode<bool>(false); // Pets

            p.Encode<byte>(0); // TamingMobInfo
            p.Encode<byte>(0); // Wishlist

            p.Encode<int>(0); // MedalAchievementInfo
            p.Encode<short>(0);

            var chairs = c.Inventories[ItemInventoryType.Install].Items
                .Select(kv => kv.Value)
                .Select(i => i.TemplateID)
                .Where(i => i / 10000 == 301)
                .ToList();
            p.Encode<int>(chairs.Count);
            chairs.ForEach(i => p.Encode<int>(i));
            await user.SendPacket(p);
        }
    }
}