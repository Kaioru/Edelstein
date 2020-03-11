using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Gameplay.Extensions.Packets;
using Edelstein.Core.Gameplay.Inventories;
using Edelstein.Core.Gameplay.Inventories.Operations;
using Edelstein.Core.Utils.Packets;
using Edelstein.Entities.Characters;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Conversations;
using Edelstein.Service.Game.Fields.Objects.User.Stats.Modify;
using Edelstein.Service.Game.Logging;

namespace Edelstein.Service.Game.Fields.Objects.User
{
    public static class FieldUserExtensions
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

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

        public static async Task Converse(this FieldUser user, IConversationContext context, IConversation conversation)
        {
            if (user.ConversationContext != null)
                throw new InvalidOperationException("Already having a conversation");
            user.ConversationContext = context;

            await conversation.Start()
                .ContinueWith(async t =>
                {
                    if (t.IsFaulted)
                    {
                        var exception = t.Exception?.Flatten().InnerException;

                        if (!(exception is TaskCanceledException))
                            Logger.Error(exception, "Caught exception when executing conversation");
                    }

                    user.ConversationContext?.Dispose();
                    user.ConversationContext = null;
                    await user.ModifyStats(exclRequest: true);
                });
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

        public static async Task ModifyInventory(
            this FieldUser user,
            Action<IModifyInventoriesContext> action = null,
            bool exclRequest = false
        )
        {
            var context = new ModifyInventoriesContext(user.Character.Inventories);

            action?.Invoke(context);
            using (var p = new Packet(SendPacketOperations.InventoryOperation))
            {
                p.Encode<bool>(exclRequest);
                context.Encode(p);
                p.Encode<bool>(false);
                await user.SendPacket(p);
            }

            if (
                context.Operations.Any(o => o.Slot < 0) ||
                context.Operations.OfType<MoveInventoryOperation>().Any(o => o.ToSlot < 0)
            )
            {
                await user.UpdateStats();
                await user.UpdateAvatar();
            }
        }
    }
}