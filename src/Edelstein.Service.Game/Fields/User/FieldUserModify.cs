using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Core.Extensions;
using Edelstein.Core.Gameplay.Inventories;
using Edelstein.Database.Entities.Inventories;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.User.Stats.Modify;
using Microsoft.Scripting.Utils;

namespace Edelstein.Service.Game.Fields.User
{
    public partial class FieldUser
    {
        public void AvatarModified()
        {
            using (var p = new Packet(SendPacketOperations.UserAvatarModified))
            {
                p.Encode<int>(ID);
                p.Encode<byte>(0x1); // Flag
                Character.EncodeLook(p);

                p.Encode<bool>(false);
                p.Encode<bool>(false);
                p.Encode<bool>(false);
                p.Encode<int>(0); // Completed Set ID

                Field.BroadcastPacket(this, p);
            }
        }

        public async Task ModifyStats(Action<ModifyStatContext> action = null, bool exclRequest = false)
        {
            var context = new ModifyStatContext(Character);

            action?.Invoke(context);
            // TODO: validate stats

            if (!IsInstantiated) return;

            if (context.Flag.HasFlag(ModifyStatType.Skin) ||
                context.Flag.HasFlag(ModifyStatType.Face) ||
                context.Flag.HasFlag(ModifyStatType.Hair))
                AvatarModified();

            using (var p = new Packet(SendPacketOperations.StatChanged))
            {
                p.Encode<bool>(exclRequest);
                context.Encode(p);
                p.Encode<bool>(false);
                p.Encode<bool>(false);
                await SendPacket(p);
            }
        }

        public async Task ModifyInventory(Action<IModifyInventoriesContext> action = null, bool exclRequest = false)
        {
            var context = new ModifyInventoriesContext(Character.Inventories);
            var equipped = Character.Inventories[ItemInventoryType.Equip].Items.Keys
                .Where(k => k < 0)
                .ToList();

            action?.Invoke(context);
            using (var p = new Packet(SendPacketOperations.InventoryOperation))
            {
                p.Encode<bool>(exclRequest);
                context.Encode(p);
                p.Encode<bool>(false);
                await SendPacket(p);
            }

            var newEquipped = Character.Inventories[ItemInventoryType.Equip].Items.Keys
                .Where(k => k < 0)
                .ToList();

            if (equipped.Except(newEquipped).Any() ||
                newEquipped.Except(equipped).Any())
            {
                // TODO: validate stats
                AvatarModified();
            }
        }
    }
}