using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Extensions;
using Edelstein.Core.Inventories;
using Edelstein.Core.Services;
using Edelstein.Data.Entities.Inventory;
using Edelstein.Network.Packet;
using Edelstein.Service.Game.Fields.User.Stats;
using Edelstein.Service.Game.Fields.User.Stats.Modify;

namespace Edelstein.Service.Game.Fields.User
{
    public partial class FieldUser
    {
        public void ValidateStat()
        {
            BasicStat.Calculate();

            if (Character.HP > BasicStat.MaxHP) ModifyStats(s => s.HP = BasicStat.MaxHP);
            if (Character.MP > BasicStat.MaxMP) ModifyStats(s => s.MP = BasicStat.MaxMP);
        }

        public void AvatarModified()
        {
            using (var p = new Packet(SendPacketOperations.UserAvatarModified))
            {
                p.Encode<int>(ID);
                p.Encode<byte>(0x1); // Flag
                Character.EncodeLook(p);

                EncodeRecord(p);
                p.Encode<int>(BasicStat.CompletedSetItemID);

                Field.BroadcastPacket(this, p);
            }
        }

        public async Task ModifyStats(Action<ModifyStatContext> action = null, bool exclRequest = false)
        {
            var context = new ModifyStatContext(Character);

            action?.Invoke(context);
            ValidateStat();

            if (!Socket.IsInstantiated) return;

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

        public Task ModifyForcedStats(Action<ModifyForcedStatContext> action = null)
        {
            var context = new ModifyForcedStatContext(ForcedStat);

            action?.Invoke(context);
            ValidateStat();

            if (!Socket.IsInstantiated) return Task.CompletedTask;
            using (var p = new Packet(SendPacketOperations.ForcedStatSet))
            {
                context.Encode(p);
                return SendPacket(p);
            }
        }

        public Task ResetForcedStats()
        {
            ForcedStat.Clear();
            ValidateStat();

            return SendPacket(new Packet(SendPacketOperations.ForcedStatReset));
        }

        public Task ModifyTemporaryStat(Action<ModifyTemporaryStatContext> action = null)
        {
            var context = new ModifyTemporaryStatContext(this);

            action?.Invoke(context);

            if (context.ResetOperations.Count > 0)
            {
                using (var p = new Packet(SendPacketOperations.TemporaryStatReset))
                {
                    TemporaryStat.EncodeMask(p, context.ResetOperations);
                    p.Encode<byte>(0); // IsMovementAffectingStat
                    SendPacket(p);
                }

                using (var p = new Packet(SendPacketOperations.UserTemporaryStatReset))
                {
                    p.Encode<int>(ID);
                    TemporaryStat.EncodeMask(p, context.ResetOperations);
                    Field.BroadcastPacket(this, p);
                }
            }

            if (context.SetOperations.Count > 0)
            {
                using (var p = new Packet(SendPacketOperations.TemporaryStatSet))
                {
                    TemporaryStat.EncodeForLocal(p, context.SetOperations);
                    p.Encode<short>(0); // tDelay
                    p.Encode<byte>(0); // IsMovementAffectingStat
                    SendPacket(p);
                }

                using (var p = new Packet(SendPacketOperations.UserTemporaryStatSet))
                {
                    p.Encode<int>(ID);
                    TemporaryStat.EncodeForRemote(p, context.SetOperations);
                    p.Encode<short>(0); // tDelay
                    Field.BroadcastPacket(this, p);
                }
            }

            if (context.ResetOperations.Count > 0 ||
                context.SetOperations.Count > 0)
                ValidateStat();
            return Task.CompletedTask;
        }

        public async Task ModifyInventory(Action<ModifyInventoryContext> action = null, bool exclRequest = false)
        {
            var context = new ModifyInventoryContext(Character);
            var equipped = Character.GetInventory(ItemInventoryType.Equip).Items
                .Where(i => i.Position < 0)
                .Select(i => i.TemplateID)
                .ToList();

            action?.Invoke(context);
            using (var p = new Packet(SendPacketOperations.InventoryOperation))
            {
                p.Encode<bool>(exclRequest);
                context.Encode(p);
                p.Encode<bool>(false);
                await SendPacket(p);
            }

            var newEquippedItems = Character.GetInventory(ItemInventoryType.Equip).Items
                .Where(i => i.Position < 0)
                .ToList();
            var newEquipped = newEquippedItems
                .Select(i => i.TemplateID)
                .ToList();

            if (equipped.Except(newEquipped).Any() ||
                newEquipped.Except(equipped).Any())
            {
                ValidateStat();
                AvatarModified();
            }
        }
    }
}