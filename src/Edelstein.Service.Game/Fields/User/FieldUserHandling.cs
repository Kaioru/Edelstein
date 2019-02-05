using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Constants;
using Edelstein.Core.Extensions;
using Edelstein.Core.Services;
using Edelstein.Core.Services.Info;
using Edelstein.Core.Types;
using Edelstein.Data.Entities;
using Edelstein.Data.Entities.Inventory;
using Edelstein.Network.Packet;
using Edelstein.Provider.Templates.Field;
using Edelstein.Provider.Templates.Server.NPCShop;
using Edelstein.Service.Game.Conversations;
using Edelstein.Service.Game.Fields.Objects;
using Edelstein.Service.Game.Fields.Objects.Drop;
using Edelstein.Service.Game.Interactions;
using Edelstein.Service.Game.Logging;
using MoreLinq;

namespace Edelstein.Service.Game.Fields.User
{
    public partial class FieldUser
    {
        public Task OnPacket(RecvPacketOperations operation, IPacket packet)
        {
            switch (operation)
            {
                case RecvPacketOperations.UserTransferChannelRequest:
                    return OnUserTransferChannelRequest(packet);
                case RecvPacketOperations.UserMigrateToCashShopRequest:
                    return OnUserMigrateToCashShopRequest(packet);
                case RecvPacketOperations.UserMigrateToITCRequest:
                    return OnUserMigrateToITCRequest(packet);
                case RecvPacketOperations.UserMove:
                    return OnUserMove(packet);
                case RecvPacketOperations.UserChat:
                    return OnUserChat(packet);
                case RecvPacketOperations.UserEmotion:
                    return OnUserEmotion(packet);
                case RecvPacketOperations.UserSelectNpc:
                    return OnUserSelectNPC(packet);
                case RecvPacketOperations.UserScriptMessageAnswer:
                    return OnUserScriptMessageAnswer(packet);
                case RecvPacketOperations.UserShopRequest:
                case RecvPacketOperations.UserTrunkRequest:
                    return Dialog?.OnPacket(operation, this, packet);
                case RecvPacketOperations.UserGatherItemRequest:
                    return OnUserGatherItemRequest(packet);
                case RecvPacketOperations.UserSortItemRequest:
                    return OnUserSortItemRequest(packet);
                case RecvPacketOperations.UserChangeSlotPositionRequest:
                    return OnUserChangeSlotPositionRequest(packet);
                case RecvPacketOperations.UserDropMoneyRequest:
                    return OnUserDropMoneyRequest(packet);
                case RecvPacketOperations.UserCharacterInfoRequest:
                    return OnUserCharacterInfoRequest(packet);
                case RecvPacketOperations.MiniRoom:
                    return OnMiniRoom(operation, packet);
                case RecvPacketOperations.MemoRequest:
                    return OnMemoRequest(packet);
                case RecvPacketOperations.DropPickUpRequest:
                    return OnDropPickUpRequest(packet);
                case RecvPacketOperations.NpcMove:
                    return Field
                        .GetObject<FieldNPC>(packet.Decode<int>())?
                        .OnNpcMove(packet);
                default:
                    Logger.Warn($"Unhandled packet operation {operation}");
                    return Task.CompletedTask;
            }
        }

        private async Task OnUserTransferChannelRequest(IPacket packet)
        {
            byte result = 0x0;
            var channel = packet.Decode<byte>();
            var service = Socket.WvsGame.Peers
                .OfType<GameServiceInfo>()
                .Where(g => g.WorldID == Socket.WvsGame.Info.WorldID)
                .OrderBy(g => g.ID)
                .ToList()[channel];

            if (Field.Template.Limit.HasFlag(FieldOpt.MigrateLimit)) return;

            if (service == null) result = 0x1;
            else if (service.AdultChannel) result = 0x1;
            else if (!await Socket.WvsGame.TryMigrateTo(Socket, Character, service)) result = 0x1;

            if (result == 0x0) return;
            using (var p = new Packet(SendPacketOperations.TransferChannelReqIgnored))
            {
                p.Encode<byte>(result);
                await SendPacket(p);
            }
        }

        private async Task OnUserMigrateToCashShopRequest(IPacket packet)
        {
            byte result = 0x0;
            var services = Socket.WvsGame.Peers
                .OfType<ShopServiceInfo>()
                .Where(g => g.Worlds.Contains(Socket.WvsGame.Info.WorldID))
                .OrderBy(g => g.ID)
                .ToList();
            ServerServiceInfo service;

            if (services.Count > 1)
            {
                var id = await Prompt<int>((self, target) => target.AskMenu(
                    "Which service should I connect to?", services.ToDictionary(
                        s => Convert.ToInt32(s.ID),
                        s => s.Name
                    ))
                );
                service = services.FirstOrDefault(s => s.ID == id);
            }
            else service = services.FirstOrDefault();

            if (Field.Template.Limit.HasFlag(FieldOpt.MigrateLimit)) return;

            if (service == null) result = 0x2;
            else if (!await Socket.WvsGame.TryMigrateTo(Socket, Character, service)) result = 0x2;

            if (result == 0x0) return;
            using (var p = new Packet(SendPacketOperations.TransferChannelReqIgnored))
            {
                p.Encode<byte>(result);
                await SendPacket(p);
            }
        }

        private async Task OnUserMigrateToITCRequest(IPacket packet)
        {
            byte result = 0x0;
            var services = Socket.WvsGame.Peers
                .OfType<TradeServiceInfo>()
                .Where(g => g.Worlds.Contains(Socket.WvsGame.Info.WorldID))
                .OrderBy(g => g.ID)
                .ToList();
            ServerServiceInfo service;

            if (services.Count > 1)
            {
                var id = await Prompt<int>((self, target) => target.AskMenu(
                    "Which service should I connect to?", services.ToDictionary(
                        s => Convert.ToInt32(s.ID),
                        s => s.Name
                    ))
                );
                service = services.FirstOrDefault(s => s.ID == id);
            }
            else service = services.FirstOrDefault();


            if (Field.Template.Limit.HasFlag(FieldOpt.MigrateLimit)) return;

            if (service == null) result = 0x3;
            else if (!await Socket.WvsGame.TryMigrateTo(Socket, Character, service)) result = 0x3;

            if (result == 0x0) return;
            using (var p = new Packet(SendPacketOperations.TransferChannelReqIgnored))
            {
                p.Encode<byte>(result);
                await SendPacket(p);
            }
        }

        private async Task OnUserMove(IPacket packet)
        {
            packet.Decode<long>();
            packet.Decode<byte>();
            packet.Decode<long>();
            packet.Decode<int>();
            packet.Decode<int>();
            packet.Decode<int>();

            var path = Move(packet);

            using (var p = new Packet(SendPacketOperations.UserMove))
            {
                p.Encode<int>(ID);
                path.Encode(p);
                await Field.BroadcastPacket(p);
            }
        }

        private async Task OnUserChat(IPacket packet)
        {
            packet.Decode<int>();

            var message = packet.Decode<string>();
            var onlyBalloon = packet.Decode<bool>();

            if (message.StartsWith("!"))
            {
                try
                {
                    await Socket.WvsGame.CommandRegistry.Process(
                        this,
                        message.Substring(1)
                    );
                }
                catch (Exception e)
                {
                    Logger.Error(e, "Caught exception when executing command");
                    await Message("An error has occured while executing that command.");
                }

                return;
            }

            using (var p = new Packet(SendPacketOperations.UserChat))
            {
                p.Encode<int>(ID);
                p.Encode<bool>(false);
                p.Encode<string>(message);
                p.Encode<bool>(onlyBalloon);
                await Field.BroadcastPacket(p);
            }
        }

        private async Task OnUserEmotion(IPacket packet)
        {
            var emotion = packet.Decode<int>();
            var duration = packet.Decode<int>();
            var byItemOption = packet.Decode<bool>();

            using (var p = new Packet(SendPacketOperations.UserEmotion))
            {
                p.Encode<int>(ID);
                p.Encode<int>(emotion);
                p.Encode<int>(duration);
                p.Encode<bool>(byItemOption);
                await Field.BroadcastPacket(this, p);
            }
        }

        private async Task OnUserSelectNPC(IPacket packet)
        {
            var npc = Field.GetObject<FieldNPC>(packet.Decode<int>());

            if (npc == null) return;

            var template = npc.Template;

            if (template.Trunk)
            {
                await Interact(new TrunkDialog(
                    template.ID,
                    Character.Data.Trunk,
                    template.TrunkGet,
                    template.TrunkPut
                ));
                return;
            }

            var shop = Socket.WvsGame.TemplateManager.Get<NPCShopTemplate>(template.ID);
            var script = template.Scripts.FirstOrDefault()?.Script;

            if (shop != null)
            {
                await Interact(new ShopDialog(shop));
                return;
            }

            if (script == null) return;

            var context = new ConversationContext(Socket);
            var conversation = Socket.WvsGame.ConversationManager.Get(
                script,
                context,
                new FieldNPCSpeaker(context, npc),
                new FieldUserSpeaker(context, this)
            );

            await Converse(conversation);
        }

        private async Task OnUserScriptMessageAnswer(IPacket packet)
        {
            if (ConversationContext == null) return;
            var messageType = (ScriptMessageType) packet.Decode<byte>();

            if (messageType != ConversationContext.PreviousScriptMessage.Type) return;
            var answers = ConversationContext.Responses;

            if (messageType == ScriptMessageType.AskQuiz ||
                messageType == ScriptMessageType.AskSpeedQuiz)
            {
                await answers.EnqueueAsync(packet.Decode<string>());
                return;
            }

            var answer = packet.Decode<byte>();

            if (
                messageType != ScriptMessageType.Say &&
                messageType != ScriptMessageType.AskYesNo &&
                messageType != ScriptMessageType.AskAccept &&
                answer == byte.MinValue ||
                (messageType == ScriptMessageType.Say ||
                 messageType == ScriptMessageType.AskYesNo ||
                 messageType == ScriptMessageType.AskAccept) && answer == byte.MaxValue
            )
            {
                ConversationContext.TokenSource.Cancel();
                return;
            }

            switch (messageType)
            {
                case ScriptMessageType.AskText:
                case ScriptMessageType.AskBoxText:
                    await answers.EnqueueAsync(packet.Decode<string>());
                    break;
                case ScriptMessageType.AskNumber:
                case ScriptMessageType.AskMenu:
                case ScriptMessageType.AskSlideMenu:
                    await answers.EnqueueAsync(packet.Decode<int>());
                    break;
                case ScriptMessageType.AskAvatar:
                case ScriptMessageType.AskMembershopAvatar:
                    await answers.EnqueueAsync(packet.Decode<byte>());
                    break;
                default:
                    await answers.EnqueueAsync(answer);
                    break;
            }
        }

        private async Task OnUserGatherItemRequest(IPacket packet)
        {
            packet.Decode<int>();
            var inventoryType = (ItemInventoryType) packet.Decode<byte>();
            var inventoryCopy = Character.GetInventory(inventoryType).Items
                .Where(i => i.Position > 0)
                .OrderBy(i => i.Position)
                .ToList();
            short pos = 1;

            await ModifyInventory(i =>
            {
                inventoryCopy.ForEach(s => i.Remove(s));
                inventoryCopy.ForEach(item => item.Position = pos++);
                inventoryCopy.ForEach(i.Set);
            }, true);

            using (var p = new Packet(SendPacketOperations.GatherItemResult))
            {
                p.Encode<bool>(false);
                p.Encode<byte>((byte) inventoryType);
                await SendPacket(p);
            }
        }

        private async Task OnUserSortItemRequest(IPacket packet)
        {
            packet.Decode<int>();
            var inventoryType = (ItemInventoryType) packet.Decode<byte>();
            var inventoryCopy = Character.GetInventory(inventoryType).Items
                .Where(i => i.Position > 0)
                .OrderBy(i => i.Position)
                .ToList();

            await ModifyInventory(i =>
            {
                inventoryCopy.ForEach(s => i.Remove(s));
                inventoryCopy = inventoryCopy.OrderBy(item => item.TemplateID).ToList();
                inventoryCopy.ForEach(i.Add);
            }, true);

            using (var p = new Packet(SendPacketOperations.SortItemResult))
            {
                p.Encode<bool>(false);
                p.Encode<byte>((byte) inventoryType);
                await SendPacket(p);
            }
        }

        private async Task OnUserChangeSlotPositionRequest(IPacket packet)
        {
            packet.Decode<int>();
            var inventoryType = (ItemInventoryType) packet.Decode<byte>();
            var fromSlot = packet.Decode<short>();
            var toSlot = packet.Decode<short>();
            var number = packet.Decode<short>();

            if (toSlot == 0)
            {
                if (Field.Template.Limit.HasFlag(FieldOpt.DropLimit)) return;

                await ModifyInventory(i =>
                {
                    var item = Character.GetInventory(inventoryType).Items
                        .Single(ii => ii.Position == fromSlot);

                    if (!ItemConstants.IsTreatSingly(item.TemplateID))
                    {
                        if (!(item is ItemSlotBundle bundle)) return;
                        if (bundle.Number < number) return;

                        item = i.Take(bundle, number);
                    }
                    else i.Remove(item);

                    var drop = new FieldDropItem(item) {Position = Position};
                    Field.Enter(drop, () => drop.GetEnterFieldPacket(0x1, this));
                }, true);
                return;
            }

            await ModifyInventory(i => i.Move(inventoryType, fromSlot, toSlot), true);
        }

        private async Task OnUserDropMoneyRequest(IPacket packet)
        {
            packet.Decode<int>();
            var money = packet.Decode<int>();

            if (Field.Template.Limit.HasFlag(FieldOpt.DropLimit)) return;

            await ModifyStats(s =>
            {
                if (s.Money < money) return;
                var drop = new FieldDropMoney(money) {Position = Position};

                s.Money -= money;
                Field.Enter(drop, () => drop.GetEnterFieldPacket(0x1, this));
            }, true);
        }

        private async Task OnUserCharacterInfoRequest(IPacket packet)
        {
            packet.Decode<int>();
            var user = Field.GetObject<FieldUser>(packet.Decode<int>());
            if (user == null) return;

            using (var p = new Packet(SendPacketOperations.CharacterInfo))
            {
                var c = user.Character;

                p.Encode<int>(user.ID);
                p.Encode<byte>(c.Level);
                p.Encode<short>(c.Job);
                p.Encode<short>(c.POP);

                p.Encode<byte>(0);

                p.Encode<string>(""); // sCommunity
                p.Encode<string>(""); // sAlliance

                p.Encode<byte>(0);
                p.Encode<byte>(0);
                p.Encode<byte>(0); // TamingMobInfo
                p.Encode<byte>((byte) c.WishList.Count);
                c.WishList.ForEach(w => p.Encode<int>(w.SN));

                p.Encode<int>(0); // MedalAchievementInfo
                p.Encode<short>(0);

                var chairs = c.Inventories
                    .SelectMany(i => i.Items)
                    .Select(i => i.TemplateID)
                    .Where(i => i / 10000 == 301)
                    .ToList();
                p.Encode<int>(chairs.Count);
                chairs.ForEach(i => p.Encode<int>(i));
                await SendPacket(p);
            }
        }

        private Task OnMiniRoom(RecvPacketOperations operation, IPacket packet)
        {
            return Dialog?.OnPacket(operation, this, packet);
        }

        private async Task OnMemoRequest(IPacket packet)
        {
            var type = (MemoRequest) packet.Decode<byte>();

            switch (type)
            {
                case MemoRequest.Send:
                    break;
                case MemoRequest.Delete:
                    var memos = Character.Memos;

                    await ModifyStats(s => s.POP += (short) memos.Count(m => m.Flag.HasFlag(MemoType.IncPOP)));

                    using (var db = Socket.WvsGame.DataContextFactory.Create())
                    {
                        db.RemoveRange(memos);
                        db.SaveChanges();
                    }

                    memos.Clear();
                    break;
                case MemoRequest.Load:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Task OnDropPickUpRequest(IPacket packet)
        {
            packet.Decode<byte>();
            packet.Decode<int>();
            packet.Decode<short>();
            packet.Decode<short>();
            var objectID = packet.Decode<int>();
            packet.Decode<int>();
            var drop = Field.GetObject<AbstractFieldDrop>(objectID);

            return drop?.PickUp(this);
        }
    }
}