using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;
using MoreLinq;

namespace Edelstein.Service.Game.Services.Handlers
{
    public class UserCharacterInfoRequestHandler : AbstractFieldUserHandler
    {
        public override async Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
        {
            packet.Decode<int>();
            var target = user.Field.GetObject<FieldUser>(packet.Decode<int>());

            if (target == null) return;

            using (var p = new Packet(SendPacketOperations.CharacterInfo))
            {
                var c = target.Character;

                p.Encode<int>(target.ID);
                p.Encode<byte>(c.Level);
                p.Encode<short>(c.Job);
                p.Encode<short>(c.POP); // TODO: use basic stat POP

                p.Encode<byte>(0);

                p.Encode<string>(""); // sCommunity
                p.Encode<string>(""); // sAlliance

                p.Encode<byte>(0); // Medal?

                var petCount = target.Pets.Count;
                p.Encode<bool>(petCount > 0);
                target.Pets
                    .OrderBy(pet => pet.IDx)
                    .ForEach(pet =>
                    {
                        p.Encode<int>(pet.Item.TemplateID);
                        p.Encode<string>(pet.Item.PetName);
                        p.Encode<byte>(pet.Item.Level);
                        p.Encode<short>(pet.Item.Tameness);
                        p.Encode<byte>(pet.Item.Repleteness);
                        p.Encode<short>(pet.Item.PetSkill);
                        p.Encode<int>(0); // Pet Equip
                        p.Encode<bool>(--petCount > 0);
                    });

                p.Encode<byte>(0); // TamingMobInfo
                p.Encode<byte>(0); // Wishlist

                p.Encode<int>(0); // MedalAchievementInfo
                p.Encode<short>(0);

                var chairs = c.Inventories.Values
                    .SelectMany(i => i.Items)
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
}