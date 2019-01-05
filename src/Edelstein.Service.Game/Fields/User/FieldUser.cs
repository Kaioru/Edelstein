using System;
using System.Drawing;
using System.Threading.Tasks;
using Edelstein.Core.Commands;
using Edelstein.Core.Extensions;
using Edelstein.Core.Services;
using Edelstein.Data.Entities;
using Edelstein.Network.Packet;
using Edelstein.Service.Game.Conversations;
using Edelstein.Service.Game.Fields.User.Messages;
using Edelstein.Service.Game.Fields.User.Messages.Types;
using Edelstein.Service.Game.Fields.User.Stats;
using Edelstein.Service.Game.Interactions;
using Edelstein.Service.Game.Logging;
using Edelstein.Service.Game.Sockets;

namespace Edelstein.Service.Game.Fields.User
{
    public partial class FieldUser : AbstractFieldLife, ICommandSender
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
        public override FieldObjType Type => FieldObjType.User;

        public WvsGameSocket Socket { get; }
        public Character Character { get; }

        public BasicStat BasicStat { get; }
        public ForcedStat ForcedStat { get; }

        public IConversationContext ConversationContext { get; private set; }
        public IDialog Dialog { get; private set; }

        public FieldUser(WvsGameSocket socket, Character character)
        {
            Socket = socket;
            Character = character;

            BasicStat = new BasicStat(this);
            ForcedStat = new ForcedStat();
            ValidateStat();
        }

        public Task Message(string text)
            => Message(new SystemMessage(text));

        public Task Message(IMessage message)
        {
            using (var p = new Packet(SendPacketOperations.Message))
            {
                message.Encode(p);
                return SendPacket(p);
            }
        }

        public async Task<T> Prompt<T>(Func<ISpeaker, ISpeaker, T> func,
            ScriptMessageParam param = ScriptMessageParam.NoESC)
        {
            var result = default(T);

            await Prompt(new Action<ISpeaker, ISpeaker>(
                (self, target) => result = func.Invoke(self, target)
            ), param);
            return result;
        }

        public Task Prompt(Action<ISpeaker, ISpeaker> action, ScriptMessageParam param = 0)
        {
            var context = new ConversationContext(Socket);
            var conversation = new Conversation(
                context,
                new Speaker(context, param: param),
                new Speaker(context, 9010000, param | ScriptMessageParam.NPCReplacedByUser),
                action
            );
            return Converse(conversation);
        }

        public Task Converse(IConversation conversation)
        {
            if (ConversationContext != null)
                throw new Exception("Already having a conversation"); // TODO: custom exception
            ConversationContext = conversation.Context;

            return Task
                .Run(conversation.Start, ConversationContext.TokenSource.Token)
                .ContinueWith(async t =>
                {
                    if (t.IsFaulted)
                    {
                        var exception = t.Exception.Flatten().InnerException;

                        if (!(exception is TaskCanceledException))
                            Logger.Error(exception, "Caught exception when executing conversation");
                    }

                    ConversationContext?.Dispose();
                    ConversationContext = null;
                    await ModifyStats(exclRequest: true);
                });
        }

        public async Task<bool> Interact(IDialog dialog, bool close = false)
        {
            if (close)
            {
                Dialog = null;
                return true;
            }

            if (Dialog != null) return false;
            if (!await dialog.Enter(this)) return false;
            
            Dialog = dialog;
            return true;
        }

        public IPacket GetSetFieldPacket()
        {
            using (var p = new Packet(SendPacketOperations.SetField))
            {
                p.Encode<short>(0); // ClientOpt

                p.Encode<int>(Socket.WvsGame.Info.ID);
                p.Encode<int>(Socket.WvsGame.Info.WorldID);

                p.Encode<bool>(true); // sNotifierMessage._m_pStr
                p.Encode<bool>(!Socket.IsInstantiated);
                p.Encode<short>(0); // nNotifierCheck, loops

                if (!Socket.IsInstantiated)
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

                p.Encode<long>(0);
                return p;
            }
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

                p.Encode<long>(0);
                p.Encode<long>(0);
                p.Encode<byte>(0);
                p.Encode<byte>(0);

                p.Encode<short>(Character.Job);
                Character.EncodeLook(p);

                p.Encode<int>(0);
                p.Encode<int>(0);
                p.Encode<int>(0);
                p.Encode<int>(0);
                p.Encode<int>(0);
                p.Encode<int>(0);

                p.Encode<Point>(Position);
                p.Encode<byte>(MoveAction);
                p.Encode<short>(Foothold);
                p.Encode<byte>(0);

                p.Encode<byte>(0);

                p.Encode<int>(0);
                p.Encode<int>(0);
                p.Encode<int>(0);

                p.Encode<byte>(0);

                p.Encode<bool>(false);

                p.Encode<byte>(0);
                p.Encode<byte>(0);
                p.Encode<byte>(0);

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

        public Task SendPacket(IPacket packet) => Socket.SendPacket(packet);
    }
}