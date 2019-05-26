using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Core.Extensions;
using Edelstein.Core.Utils;
using Edelstein.Database.Entities;
using Edelstein.Database.Entities.Characters;
using Edelstein.Database.Entities.Inventories;
using Edelstein.Database.Entities.Inventories.Items;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Conversations;
using Edelstein.Service.Game.Conversations.Speakers;
using Edelstein.Service.Game.Fields.Objects.User.Broadcasts;
using Edelstein.Service.Game.Fields.Objects.User.Effects;
using Edelstein.Service.Game.Fields.Objects.User.Messages;
using Edelstein.Service.Game.Fields.Objects.User.Messages.Types;
using Edelstein.Service.Game.Fields.Objects.User.Stats;
using Edelstein.Service.Game.Interactions;
using Edelstein.Service.Game.Logging;
using Edelstein.Service.Game.Services;
using Marten.Util;
using MoreLinq;

namespace Edelstein.Service.Game.Fields.Objects.User
{
    public partial class FieldUser : AbstractFieldLife, IFieldUser, ITickable
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
        public override FieldObjType Type => FieldObjType.User;

        public GameService Service => Socket.Service;
        public GameSocket Socket { get; }
        public Account Account => Socket.Account;
        public AccountData AccountData => Socket.AccountData;
        public Character Character => Socket.Character;
        public bool IsInstantiated { get; set; }

        public ICollection<IFieldControlledObj> Controlled { get; }
        public ICollection<IFieldOwnedObj> Owned { get; }
        public ICollection<FieldUserPet> Pets { get; }

        public BasicStat BasicStat { get; }
        public ForcedStat ForcedStat { get; }
        public IDictionary<TemporaryStatType, TemporaryStat> TemporaryStats { get; }
        public int? PortableChairID { get; set; }

        public IConversationContext ConversationContext { get; private set; }
        public IDialog Dialog { get; private set; }

        public FieldUser(GameSocket socket)
        {
            Socket = socket;

            Controlled = new List<IFieldControlledObj>();
            Owned = new List<IFieldOwnedObj>();
            Pets = Character.Pets
                .Where(sn => sn > 0)
                .Select(sn => Character
                    .Inventories[ItemInventoryType.Cash].Items.Values
                    .OfType<ItemSlotPet>()
                    .FirstOrDefault(i => i.CashItemSN == sn))
                .Where(item => item != null)
                .Select(item => new FieldUserPet(
                    this,
                    item,
                    (byte) Character.Pets.IndexOf(item.CashItemSN ?? 0)
                ))
                .ToList();

            BasicStat = new BasicStat(this);
            ForcedStat = new ForcedStat(this);
            TemporaryStats = new Dictionary<TemporaryStatType, TemporaryStat>();
            ValidateStat();
        }

        public Task Message(string text)
        {
            return Message(new SystemMessage(text));
        }

        public Task Message(IMessage message)
        {
            using (var p = new Packet(SendPacketOperations.Message))
            {
                message.Encode(p);
                return SendPacket(p);
            }
        }

        public Task Message(IBroadcastMessage message)
        {
            using (var p = new Packet(SendPacketOperations.BroadcastMsg))
            {
                message.Encode(p);
                return SendPacket(p);
            }
        }

        public Task Effect(EffectType type, bool local = true, bool remote = false)
        {
            return Effect(new Effect(type), local, remote);
        }

        public async Task Effect(IEffect effect, bool local = true, bool remote = false)
        {
            if (local)
            {
                using (var p = new Packet(SendPacketOperations.UserEffectLocal))
                {
                    effect.Encode(p);
                    await SendPacket(p);
                }
            }

            if (remote)
            {
                using (var p = new Packet(SendPacketOperations.UserEffectRemote))
                {
                    p.Encode<int>(ID);
                    effect.Encode(p);
                    await Field.BroadcastPacket(this, p);
                }
            }
        }

        public async Task Interact(IDialog dialog = null, bool close = false)
        {
            if (close)
            {
                Dialog?.Leave();
                Dialog = null;
                return;
            }

            if (Dialog != null) return;

            Dialog = dialog;
            Dialog?.Enter();
        }

        public Task<T> Prompt<T>(Func<ISpeaker, T> func, SpeakerParamType param = 0)
            => Prompt((self, target) => func.Invoke(target), param);

        public async Task<T> Prompt<T>(Func<ISpeaker, ISpeaker, T> func, SpeakerParamType param = 0)
        {
            var error = true;
            var result = default(T);

            await Prompt((self, target) =>
            {
                result = func.Invoke(self, target);
                error = false;
            }, param);

            if (error) throw new TaskCanceledException();
            return result;
        }

        public Task Prompt(Action<ISpeaker, ISpeaker> action, SpeakerParamType param = 0)
        {
            var context = new ConversationContext(Socket);
            var conversation = new ActionConversation(
                context,
                new Speaker(context, param: param),
                new Speaker(context, param: param | SpeakerParamType.NPCReplacedByUser),
                action
            );
            return Converse(conversation);
        }

        public Task Converse(IConversation conversation)
        {
            if (ConversationContext != null)
                throw new InvalidOperationException("Already having a conversation");
            ConversationContext = conversation.Context;

            return Task
                .Run(conversation.Start, ConversationContext.TokenSource.Token)
                .ContinueWith(async t =>
                {
                    if (t.IsFaulted)
                    {
                        var exception = t.Exception?.Flatten().InnerException;

                        if (!(exception is TaskCanceledException))
                            Logger.Error(exception, "Caught exception when executing conversation");
                    }

                    ConversationContext?.Dispose();
                    ConversationContext = null;
                    await ModifyStats(exclRequest: true);
                });
        }

        public override IPacket GetEnterFieldPacket()
        {
            using (var p = new Packet(SendPacketOperations.UserEnterField))
            {
                p.Encode<int>(ID);

                p.Encode<byte>(Character.Level);
                p.Encode<string>(Character.Name);

                // Guild
                p.Encode<string>("");
                p.Encode<short>(0);
                p.Encode<byte>(0);
                p.Encode<short>(0);
                p.Encode<byte>(0);

                TemporaryStats.EncodeRemote(p);

                p.Encode<short>(Character.Job);
                Character.EncodeLook(p);

                p.Encode<int>(0);
                p.Encode<int>(0);
                p.Encode<int>(0);
                p.Encode<int>(0);
                p.Encode<int>(BasicStat.CompletedSetItemID);
                p.Encode<int>(PortableChairID ?? 0);

                p.Encode<Point>(Position);
                p.Encode<byte>(MoveAction);
                p.Encode<short>(Foothold);
                p.Encode<byte>(0);

                Pets.ForEach(pet =>
                {
                    p.Encode<bool>(true);
                    pet.EncodeData(p);
                });
                p.Encode<bool>(false);

                p.Encode<int>(0);
                p.Encode<int>(0);
                p.Encode<int>(0);

                p.Encode<byte>(0);

                p.Encode<bool>(false);

                p.Encode<bool>(false);
                p.Encode<bool>(false);
                p.Encode<bool>(false);

                p.Encode<byte>(0);

                p.Encode<byte>(0);
                p.Encode<int>(0);
                return p;
            }
        }

        public override IPacket GetLeaveFieldPacket()
        {
            using (var p = new Packet(SendPacketOperations.UserLeaveField))
            {
                p.Encode<int>(ID);
                return p;
            }
        }

        public IPacket GetSetFieldPacket()
        {
            using (var p = new Packet(SendPacketOperations.SetField))
            {
                p.Encode<short>(0); // ClientOpt

                p.Encode<int>(Service.Info.ID);
                p.Encode<int>(Service.Info.WorldID);

                p.Encode<bool>(true); // sNotifierMessage._m_pStr
                p.Encode<bool>(!IsInstantiated);
                p.Encode<short>(0); // nNotifierCheck, loops

                if (!IsInstantiated)
                {
                    p.Encode<int>(0);
                    p.Encode<int>(0);
                    p.Encode<int>(0);

                    Character.EncodeData(p);

                    p.Encode<int>(0);
                    for (var i = 0; i < 3; i++) p.Encode<int>(0);
                }
                else
                {
                    p.Encode<byte>(0);
                    p.Encode<int>(Character.FieldID);
                    p.Encode<byte>(Character.FieldPortal);
                    p.Encode<int>(Character.HP);
                    p.Encode<bool>(false);
                }

                p.Encode<DateTime>(DateTime.Now);
                return p;
            }
        }

        public async Task Dispose()
        {
            ConversationContext?.Dispose();
            ForcedStat.Reset();
            await Interact(close: true);
        }

        public Task SendPacket(IPacket packet)
            => Socket.SendPacket(packet);

        public async Task OnTick(DateTime now)
        {
            var expiredStats = TemporaryStats.Values
                .Where(i => i.DateExpire != null && now > i.DateExpire.Value)
                .ToList();

            if (expiredStats.Any())
                await ModifyTemporaryStats(s => expiredStats.ForEach(e => s.Reset(e.Type)));
        }
    }
}