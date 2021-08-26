using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Stages.Game.Conversations;
using Edelstein.Common.Gameplay.Stages.Game.Conversations.Speakers;
using Edelstein.Common.Gameplay.Stages.Game.Objects.User.Messages;
using Edelstein.Common.Gameplay.Stages.Game.Objects.User.Stats;
using Edelstein.Common.Gameplay.Users;
using Edelstein.Common.Gameplay.Users.Inventories.Modify;
using Edelstein.Common.Gameplay.Users.Inventories.Modify.Operations;
using Edelstein.Common.Gameplay.Users.Stats.Modify;
using Edelstein.Protocol.Gameplay.Social;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Messages;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats;
using Edelstein.Protocol.Gameplay.Users;
using Edelstein.Protocol.Gameplay.Users.Inventories.Modify;
using Edelstein.Protocol.Gameplay.Users.Stats.Modify;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Transport;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User
{
    public class FieldObjUser : AbstractFieldLife, IFieldObjUser
    {
        public override FieldObjType Type => FieldObjType.User;
        public new int ID => GameStageUser.Character.ID;

        public GameStageUser GameStageUser { get; }
        public GameStage GameStage => GameStageUser.Stage;

        public Account Account => GameStageUser.Account;
        public AccountWorld AccountWorld => GameStageUser.AccountWorld;
        public Character Character => GameStageUser.Character;
        public IField Stage => Field;
        public ISocket Socket => GameStageUser.Socket;

        public bool IsInstantiated { get; set; }
        public bool IsConversing => ConversationContext != null;

        public IConversationContext ConversationContext { get; set; }

        public ICollection<IFieldSplit> Watching { get; }
        public ICollection<IFieldControlledObj> Controlling { get; }

        public ICalculatedRates Rates { get; }
        public ICalculatedStats Stats { get; }

        public IGuild Guild { get; set; }
        public IParty Party { get; set; }

        public FieldObjUser(GameStageUser user)
        {
            GameStageUser = user;
            Watching = new List<IFieldSplit>();
            Controlling = new List<IFieldControlledObj>();

            Rates = new CalculatedRates(this);
            Stats = new CalculatedStats(
                this,
                GameStage.ItemTemplates,
                GameStage.ItemOptionTemplates,
                GameStage.ItemSetTemplates
            );

            _ = UpdateStats();
        }

        public Task OnPacket(IPacketReader packet) => GameStageUser.OnPacket(packet);
        public Task OnException(Exception exception) => GameStageUser.OnException(exception);
        public Task OnDisconnect() => GameStageUser.OnDisconnect();

        public Task Update() => GameStageUser.Update();
        public Task Disconnect() => GameStageUser.Disconnect();

        public IPacket GetSetFieldPacket()
        {
            var packet = new UnstructuredOutgoingPacket(PacketSendOperations.SetField);

            packet.WriteShort(0); // ClientOpt

            packet.WriteInt(GameStage.ChannelID);
            packet.WriteInt(GameStage.WorldID);

            packet.WriteBool(true); // sNotifierMessage._m_pStr
            packet.WriteBool(!IsInstantiated);
            packet.WriteShort(0); // nNotifierCheck, loops

            if (!IsInstantiated)
            {
                packet.WriteInt(0);
                packet.WriteInt(0);
                packet.WriteInt(0);

                packet.WriteCharacterData(Character);

                packet.WriteInt(0);
                for (var i = 0; i < 3; i++) packet.WriteInt(0);
            }
            else
            {
                packet.WriteByte(0);
                packet.WriteInt(Character.FieldID);
                packet.WriteByte(Character.FieldPortal);
                packet.WriteInt(Character.HP);
                packet.WriteBool(false);
            }

            packet.WriteDateTime(DateTime.Now);

            return packet;
        }

        public override IPacket GetEnterFieldPacket()
        {
            var packet = new UnstructuredOutgoingPacket(PacketSendOperations.UserEnterField);
            packet.WriteInt(ID);

            packet.WriteByte(Character.Level);
            packet.WriteString(Character.Name);

            packet.WriteString(Guild?.Name ?? "");
            packet.WriteShort(Guild?.MarkBg ?? 0);
            packet.WriteByte(Guild?.MarkBgColor ?? 0);
            packet.WriteShort(Guild?.Mark ?? 0);
            packet.WriteByte(Guild?.MarkColor ?? 0);

            packet.WriteLong(0);
            packet.WriteLong(0);
            packet.WriteByte(0); // nDefenseAtt
            packet.WriteByte(0); // nDefenseState

            packet.WriteShort(Character.Job);
            packet.WriteCharacterLook(Character);

            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);

            packet.WritePoint2D(Position);
            packet.WriteByte((byte)Action);
            packet.WriteShort((short)(Foothold?.ID ?? 0));
            packet.WriteByte(0);

            packet.WriteBool(false);
            packet.WriteBool(false);
            packet.WriteBool(false);

            packet.WriteBool(false);

            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);

            packet.WriteByte(0);

            packet.WriteBool(false);

            packet.WriteBool(false);
            packet.WriteBool(false);
            packet.WriteBool(false);

            packet.WriteByte(0);

            packet.WriteByte(0);
            packet.WriteInt(0);

            return packet;
        }

        public override IPacket GetLeaveFieldPacket()
            => new UnstructuredOutgoingPacket(PacketSendOperations.UserLeaveField).WriteInt(ID);

        public async Task UpdateStats()
        {
            await Rates.Calculate();
            await Stats.Calculate();

            if (Character.HP > Stats.MaxHP) await ModifyStats(s => s.HP = Stats.MaxHP);
            if (Character.HP > Stats.MaxMP) await ModifyStats(s => s.HP = Stats.MaxMP);
        }

        public async Task UpdateAvatar()
        {
            var avatarPacket = new UnstructuredOutgoingPacket(PacketSendOperations.UserAvatarModified);

            avatarPacket.WriteInt(ID);
            avatarPacket.WriteByte(0x1); // Flag
            avatarPacket.WriteCharacterLook(Character);

            avatarPacket.WriteBool(false);
            avatarPacket.WriteBool(false);
            avatarPacket.WriteBool(false);
            avatarPacket.WriteInt(0);

            await FieldSplit.Dispatch(this, avatarPacket);
        }

        public Task Message(string message)
            => Message(new SystemMessage(message));

        public async Task Message(IMessage message)
        {
            var response = new UnstructuredOutgoingPacket(PacketSendOperations.Message);

            response.Write(message);
            await Dispatch(response);
        }

        public Task<T> Prompt<T>(Func<
            IConversationSpeaker,
            T
        > function)
            => Prompt((self, target) => function.Invoke(target));

        public async Task<T> Prompt<T>(Func<
            IConversationSpeaker,
            IConversationSpeaker,
            T
        > function)
        {
            var result = default(T);
            var error = true;

            var context = new ConversationContext(this);
            var conversation = new BasicConversation(
                context,
                new BasicSpeaker(context),
                new BasicSpeaker(context, flags: ConversationSpeakerFlags.NPCReplacedByUser),
                (self, target) =>
                {
                    result = function.Invoke(self, target);
                    error = false;
                }
            );

            await Converse(conversation);

            if (error) throw new TaskCanceledException();

            return result;
        }

        public async Task Converse(IConversation conversation)
        {
            if (IsConversing) return;

            ConversationContext = conversation.Context;

            await Task.Run(async () =>
            {
                try
                {
                    await conversation.Start();
                }
                finally
                {
                    await EndConversation();
                    await ModifyStats(exclRequest: true);
                }
            });
        }

        public Task EndConversation()
        {
            if (IsConversing)
            {
                ConversationContext.Dispose();
                ConversationContext = null;
            }
            return Task.CompletedTask;
        }

        public async Task ModifyStats(Action<IModifyStatContext> action = null, bool exclRequest = false)
        {
            var context = new ModifyStatContext(Character);

            action?.Invoke(context);
            await UpdateStats();

            if (!IsInstantiated) return;

            var statPacket = new UnstructuredOutgoingPacket(PacketSendOperations.StatChanged);

            statPacket.WriteBool(exclRequest);
            statPacket.Write(context);
            statPacket.WriteBool(false);
            statPacket.WriteBool(false);

            await Dispatch(statPacket);
        }

        public async Task ModifyInventory(Action<IModifyMultiInventoryContext> action = null, bool exclRequest = false)
        {
            var context = new ModifyMultiInventoryContext(Character.Inventories, GameStage.ItemTemplates);

            action?.Invoke(context);

            var inventoryPacket = new UnstructuredOutgoingPacket(PacketSendOperations.InventoryOperation);

            inventoryPacket.WriteBool(exclRequest);
            inventoryPacket.Write(context);
            inventoryPacket.WriteBool(false);

            await Dispatch(inventoryPacket);

            if (
                context.History.Any(o => o.Slot < 0) ||
                context.History.OfType<MoveModifyInventoryOperation>().Any(o => o.ToSlot < 0)
            )
            {
                await UpdateStats();
                await UpdateAvatar();
            }
        }

        public Task Dispatch(IPacket packet) => GameStageUser.Dispatch(packet);
    }
}
