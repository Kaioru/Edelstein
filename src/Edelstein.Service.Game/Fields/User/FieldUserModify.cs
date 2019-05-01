using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Core.Extensions;
using Edelstein.Core.Gameplay.Inventories;
using Edelstein.Database.Entities.Inventories;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.User.Stats.Modify;

namespace Edelstein.Service.Game.Fields.User
{
    public partial class FieldUser
    {
        public async Task ValidateStat()
        {
            BasicStat.Calculate();

            if (Character.HP > BasicStat.MaxHP) await ModifyStats(s => s.HP = BasicStat.MaxHP);
            if (Character.MP > BasicStat.MaxMP) await ModifyStats(s => s.MP = BasicStat.MaxMP);
        }

        public async Task AvatarModified()
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

                await Field.BroadcastPacket(this, p);
            }
        }

        public async Task ModifyStats(Action<ModifyStatContext> action = null, bool exclRequest = false)
        {
            var context = new ModifyStatContext(Character);

            action?.Invoke(context);
            await ValidateStat();

            if (!IsInstantiated) return;

            if (context.Flag.HasFlag(ModifyStatType.Skin) ||
                context.Flag.HasFlag(ModifyStatType.Face) ||
                context.Flag.HasFlag(ModifyStatType.Hair))
                await AvatarModified();

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
                await ValidateStat();
                await AvatarModified();
            }
        }
    }
}