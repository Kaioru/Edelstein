using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Core.Extensions;
using Edelstein.Core.Gameplay.Constants;
using Edelstein.Core.Gameplay.Inventories;
using Edelstein.Core.Gameplay.Skills;
using Edelstein.Database.Entities.Inventories;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.Dragons;
using Edelstein.Service.Game.Fields.User.Stats;
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
                p.Encode<int>(BasicStat.CompletedSetItemID); // Completed Set ID

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

            if (context.Flag.HasFlag(ModifyStatType.Job) &&
                SkillConstants.HasEvanDragon(Character.Job) &&
                !Owned.Any(o => o is FieldDragon)
            )
            {
                var dragon = new FieldDragon(this);

                Owned.Add(dragon);
                await Field.Enter(dragon);
            }

            using (var p = new Packet(SendPacketOperations.StatChanged))
            {
                p.Encode<bool>(exclRequest);
                context.Encode(p);
                p.Encode<bool>(false);
                p.Encode<bool>(false);
                await SendPacket(p);
            }
        }

        public async Task ModifyForcedStats(Action<ModifyForcedStatContext> action = null)
        {
            var context = new ModifyForcedStatContext(ForcedStat);

            action?.Invoke(context);
            await ValidateStat();

            if (!IsInstantiated) return;
            using (var p = new Packet(SendPacketOperations.ForcedStatSet))
            {
                context.Encode(p);
                await SendPacket(p);
            }
        }

        public async Task ModifyTemporaryStats(Action<ModifyTemporaryStatContext> action = null)
        {
            var context = new ModifyTemporaryStatContext(this);

            action?.Invoke(context);
            await ValidateStat();

            if (context.ResetOperations.Count > 0)
            {
                using (var p = new Packet(SendPacketOperations.TemporaryStatReset))
                {
                    context.ResetOperations.EncodeMask(p);
                    p.Encode<byte>(0); // IsMovementAffectingStat
                    await SendPacket(p);
                }

                using (var p = new Packet(SendPacketOperations.UserTemporaryStatReset))
                {
                    p.Encode<int>(ID);
                    context.ResetOperations.EncodeMask(p);
                    await Field.BroadcastPacket(this, p);
                }
            }

            if (context.SetOperations.Count > 0)
            {
                using (var p = new Packet(SendPacketOperations.TemporaryStatSet))
                {
                    context.SetOperations.EncodeLocal(p);
                    p.Encode<short>(0); // tDelay
                    p.Encode<byte>(0); // IsMovementAffectingStat
                    await SendPacket(p);
                }

                using (var p = new Packet(SendPacketOperations.UserTemporaryStatSet))
                {
                    p.Encode<int>(ID);
                    context.SetOperations.EncodeRemote(p);
                    p.Encode<short>(0); // tDelay
                    await Field.BroadcastPacket(this, p);
                }
            }
        }

        public async Task ModifyInventory(Action<IModifyInventoriesContext> action = null, bool exclRequest = false)
        {
            var context = new ModifyInventoriesContext(Character.Inventories);

            action?.Invoke(context);
            using (var p = new Packet(SendPacketOperations.InventoryOperation))
            {
                p.Encode<bool>(exclRequest);
                context.Encode(p);
                p.Encode<bool>(false);
                await SendPacket(p);
            }

            if (context.Operations.Any(o => o.Slot < 0))
            {
                await ValidateStat();
                await AvatarModified();
            }
        }

        public async Task ModifySkills(Action<ModifySkillContext> action = null, bool exclRequest = false)
        {
            var context = new ModifySkillContext(Character);

            action?.Invoke(context);
            await ValidateStat();

            using (var p = new Packet(SendPacketOperations.ChangeSkillRecordResult))
            {
                p.Encode<bool>(exclRequest);
                context.Encode(p);
                p.Encode<bool>(true);
                await SendPacket(p);
            }
        }
    }
}