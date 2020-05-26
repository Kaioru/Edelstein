using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Baseline;
using Edelstein.Core.Entities.Inventories.Items;
using Edelstein.Core.Gameplay.Extensions.Packets;
using Edelstein.Core.Gameplay.Inventories;
using Edelstein.Core.Gameplay.Inventories.Operations;
using Edelstein.Core.Network.Packets;
using Edelstein.Core.Templates.Items;
using Edelstein.Core.Templates.Items.ItemOption;
using Edelstein.Core.Utils.Packets;
using Edelstein.Service.Game.Conversations;
using Edelstein.Service.Game.Conversations.Speakers;
using Edelstein.Service.Game.Conversations.Util;
using Edelstein.Service.Game.Fields.Objects.User.Messages;
using Edelstein.Service.Game.Fields.Objects.User.Messages.Impl;
using Edelstein.Service.Game.Fields.Objects.User.Stats.Modify;
using Edelstein.Service.Game.Logging;
using MoreLinq.Extensions;

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
            using var p = new OutPacket(SendPacketOperations.UserAvatarModified);

            p.EncodeInt(user.ID);
            p.EncodeByte(0x1); // Flag
            user.Character.EncodeLook(p);

            p.EncodeBool(false);
            p.EncodeBool(false);
            p.EncodeBool(false);
            p.EncodeInt(user.BasicStat.CompletedSetItemID);

            await user.BroadcastPacket(p);
        }

        public static Task Message(this FieldUser user, string text)
        {
            return user.Message(new SystemMessage(text));
        }

        public static Task Message(this FieldUser user, IMessage message)
        {
            using var p = new OutPacket(SendPacketOperations.Message);
            message.Encode(p);
            return user.SendPacket(p);
        }

        public static async Task Command(this FieldUser user, string text)
        {
            try
            {
                await user.Service.CommandManager.Process(
                    user,
                    text
                );
            }
            catch (Exception exception)
            {
                await user.Message("An error has occured while executing that command.");
                Logger.Error(exception, "Caught exception when executing command.");
            }
        }

        public static Task<T> Prompt<T>(
            this FieldUser user,
            Func<IConversationSpeaker, T> func,
            ConversationSpeakerType type = 0
        )
            => user.Prompt((self, target) => func.Invoke(target), type);

        public static async Task<T> Prompt<T>(
            this FieldUser user,
            Func<IConversationSpeaker, IConversationSpeaker, T> func,
            ConversationSpeakerType type = 0
        )
        {
            var error = true;
            var result = default(T);

            await user.Prompt((self, target) =>
            {
                result = func.Invoke(self, target);
                error = false;
            }, type);

            if (error) throw new TaskCanceledException();
            return result;
        }

        public static Task Prompt(
            this FieldUser user,
            Action<IConversationSpeaker, IConversationSpeaker> action,
            ConversationSpeakerType type = 0
        )
        {
            var context = new ConversationContext(user.Adapter.Socket);
            var conversation = new ActionConversation(
                context,
                new DefaultSpeaker(context, type: type),
                new DefaultSpeaker(context, type: type | ConversationSpeakerType.NPCReplacedByUser),
                action
            );
            return user.Converse(context, conversation);
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

                    user.ConversationContext?.SafeDispose();
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

            using (var p = new OutPacket(SendPacketOperations.StatChanged))
            {
                p.EncodeBool(exclRequest);
                context.Encode(p);
                p.EncodeBool(false);
                p.EncodeBool(false);

                await user.SendPacket(p);
            }

            if (context.Flag.HasFlag(ModifyStatType.Skin) ||
                context.Flag.HasFlag(ModifyStatType.Face) ||
                context.Flag.HasFlag(ModifyStatType.Hair)) 
                await user.UpdateAvatar();

            if (context.Flag.HasFlag(ModifyStatType.Level) ||
                context.Flag.HasFlag(ModifyStatType.Job)) 
            {
                if (user.Party != null)
                    await user.Party
                        .UpdateChangeLevelOrJob(
                            user.Character.ID,
                            user.Character.Level,
                            user.Character.Job
                        );
                if (user.Guild != null)
                    await user.Guild
                        .UpdateChangeLevelOrJob(
                            user.Character.ID,
                            user.Character.Level,
                            user.Character.Job
                        );
            }

            if (user.Party != null && 
                (context.Flag.HasFlag(ModifyStatType.HP) ||
                 context.Flag.HasFlag(ModifyStatType.MaxHP)))
            {
                using var p = new OutPacket(SendPacketOperations.UserHP);

                p.EncodeInt(user.ID);
                p.EncodeInt(user.Character.HP);
                p.EncodeInt(user.Character.MaxHP);

                await user.Field.BroadcastPacket(user, user.Party, p);
            }
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

            using var p = new OutPacket(SendPacketOperations.ForcedStatSet);
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
            using (var p = new OutPacket(SendPacketOperations.InventoryOperation))
            {
                p.EncodeBool(exclRequest);
                context.Encode(p);
                p.EncodeBool(false);
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

        public static async Task UnreleaseItemOption(
            this FieldUser user,
            ItemSlotEquip equip,
            ItemOptionGrade? grade = null,
            ItemOptionUnreleaseType type = ItemOptionUnreleaseType.Basic
        )
        {
            if (!(user.Service.TemplateManager.Get<ItemTemplate>(equip.TemplateID) is ItemEquipTemplate template))
                return;

            var random = new Random();

            if (!grade.HasValue)
            {
                grade = (ItemOptionGrade) (equip.Grade - 4);
                grade = (ItemOptionGrade) Math.Min(Math.Max((int) grade, 1), 3);

                if (grade == ItemOptionGrade.Rare && random.Next(100) <= 12 ||
                    grade == ItemOptionGrade.Epic && random.Next(100) <= 4)
                    grade++;
            }

            var gradeMax = grade;
            var gradeMin = grade - 1;

            gradeMax = (ItemOptionGrade) Math.Min(Math.Max((int) gradeMax, 1), 3);
            gradeMin = (ItemOptionGrade) Math.Min(Math.Max((int) gradeMin, 0), 2);

            var optionsAll = user.Service.TemplateManager
                .GetAll<ItemOptionTemplate>()
                .Where(o => template.ReqLevel >= o.ReqLevel)
                // TODO: proper optionType checks
                .Where(o => o.Type == ItemOptionType.AnyEquip)
                .ToImmutableList();
            var optionsPrimary = optionsAll
                .Where(o => o.Grade == gradeMax)
                .ToImmutableList();
            var optionsSecondary = optionsAll
                .Where(o => o.Grade == gradeMin)
                .ToImmutableList();

            equip.Grade = (byte) grade;
            equip.Option1 = (short) (optionsPrimary.Shuffle(random).FirstOrDefault()?.ID ?? 0);
            if (equip.Option2 > 0 ||
                type == ItemOptionUnreleaseType.Premium && random.Next(100) <= 12)
                equip.Option2 = random.Next(100) <= 6
                    ? (short) (optionsPrimary.Shuffle(random).FirstOrDefault()?.ID ?? 0)
                    : (short) (optionsSecondary.Shuffle(random).FirstOrDefault()?.ID ?? 0);
            if (equip.Option3 > 0 ||
                type == ItemOptionUnreleaseType.Premium && random.Next(100) <= 4)
                equip.Option3 = random.Next(100) <= 4
                    ? (short) (optionsPrimary.Shuffle(random).FirstOrDefault()?.ID ?? 0)
                    : (short) (optionsSecondary.Shuffle(random).FirstOrDefault()?.ID ?? 0);

            equip.Option1 = (short) -equip.Option1;
            equip.Option2 = (short) -equip.Option2;
            equip.Option3 = (short) -equip.Option3;

            await user.ModifyInventory(i => i.Update(equip));
        }

        public static async Task ReleaseItemOption(this FieldUser user, ItemSlotEquip equip)
        {
            equip.Grade = (byte) (equip.Grade + 4);
            equip.Option1 = Math.Abs(equip.Option1);
            equip.Option2 = Math.Abs(equip.Option2);
            equip.Option3 = Math.Abs(equip.Option3);

            await user.ModifyInventory(i => i.Update(equip));
        }
    }
}