using System;
using System.Drawing;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Core.Extensions;
using Edelstein.Database;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Conversations;
using Edelstein.Service.Game.Fields.Objects;
using Edelstein.Service.Game.Logging;
using Edelstein.Service.Game.Services;

namespace Edelstein.Service.Game.Fields.User
{
    public partial class FieldUser : AbstractFieldLife, IFieldUser
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
        public override FieldObjType Type => FieldObjType.User;

        public GameService Service => Socket.Service;
        public GameSocket Socket { get; }
        public Character Character => Socket.Character;
        public bool IsInstantiated { get; set; }

        public IConversationContext ConversationContext { get; private set; }

        public FieldUser(GameSocket socket)
        {
            Socket = socket;
        }

        public Task Converse(IConversation conversation)
        {
            if (ConversationContext != null)
                throw new InvalidOperationException("Already having a conversation");
            ConversationContext = conversation.Context;

            return Task
                .Run(conversation.Start, ConversationContext.TokenSource.Token)
                .ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        var exception = t.Exception?.Flatten().InnerException;

                        if (!(exception is TaskCanceledException))
                            Logger.Error(exception, "Caught exception when executing conversation");
                    }

                    ConversationContext?.Dispose();
                    ConversationContext = null;
                    // TODO: exclRequest
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

                p.Encode<long>(0);
                p.Encode<long>(0);
                p.Encode<byte>(0); // nDefenseAtt
                p.Encode<byte>(0); // nDefenseState

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

                p.Encode<long>(0);
                return p;
            }
        }

        public void Dispose()
        {
            // TODO
        }

        public Task SendPacket(IPacket packet)
            => Socket.SendPacket(packet);

        public Task OnPacket(RecvPacketOperations operation, IPacket packet)
        {
            switch (operation)
            {
                case RecvPacketOperations.NpcMove:
                case RecvPacketOperations.NpcSpecialAction:
                    return Field
                        .GetControlledObject<FieldNPC>(this, packet.Decode<int>())?
                        .OnPacket(operation, packet);
            }

            return operation switch {
                RecvPacketOperations.UserTransferFieldRequest => OnUserTransferFieldRequest(packet),
                RecvPacketOperations.UserMove => OnUserMove(packet),
                RecvPacketOperations.UserChat => OnUserChat(packet),
                RecvPacketOperations.UserEmotion => OnUserEmotion(packet),
                RecvPacketOperations.UserScriptMessageAnswer => OnUserScriptMessageAnswer(packet),
                _ => Task.Run(() => Logger.Warn($"Unhandled packet operation {operation}"))
                };
        }
    }
}