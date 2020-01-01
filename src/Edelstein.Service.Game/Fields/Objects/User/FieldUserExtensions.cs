using System;
using System.Threading.Tasks;
using Edelstein.Core.Gameplay.Extensions.Packets;
using Edelstein.Core.Utils.Packets;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User.Stats.Modify;

namespace Edelstein.Service.Game.Fields.Objects.User
{
    public static class FieldUserExtensions
    {
        public static async Task UpdateStats(this FieldUser user)
        {
            await user.BasicStat.Calculate();

            if (user.Character.HP > user.BasicStat.MaxHP) await user.ModifyStats(s => s.HP = user.BasicStat.MaxHP);
            if (user.Character.MP > user.BasicStat.MaxMP) await user.ModifyStats(s => s.MP = user.BasicStat.MaxMP);
        }

        public static async Task UpdateAvatar(this FieldUser user)
        {
            using var p = new Packet(SendPacketOperations.UserAvatarModified);

            p.Encode<int>(user.ID);
            p.Encode<byte>(0x1); // Flag
            user.Character.EncodeLook(p);

            p.Encode<bool>(false);
            p.Encode<bool>(false);
            p.Encode<bool>(false);
            p.Encode<int>(user.BasicStat.CompletedSetItemID);

            await user.BroadcastPacket(p);
        }

        public static async Task ModifyStats(
            this FieldUser user,
            Action<ModifyStatContext> action = null,
            bool exclRequest = false
        )
        {
            var context = new ModifyStatContext(user.Character);

            action?.Invoke(context);
            await user.UpdateStats();

            if (!user.IsInstantiated) return;

            using (var p = new Packet(SendPacketOperations.StatChanged))
            {
                p.Encode<bool>(exclRequest);
                context.Encode(p);
                p.Encode<bool>(false);
                p.Encode<bool>(false);

                await user.SendPacket(p);
            }

            if (
                context.Flag.HasFlag(ModifyStatType.Skin) ||
                context.Flag.HasFlag(ModifyStatType.Face) ||
                context.Flag.HasFlag(ModifyStatType.Hair)
            ) await user.UpdateAvatar();
        }

        public static async Task ModifyForcedStats(
            this FieldUser user,
            Action<ModifyForcedStatContext> action = null
        )
        {
            var context = new ModifyForcedStatContext(user.ForcedStat);

            action?.Invoke(context);
            await user.UpdateStats();

            if (!user.IsInstantiated) return;

            using var p = new Packet(SendPacketOperations.ForcedStatSet);
            context.Encode(p);
            await user.SendPacket(p);
        }
    }
}