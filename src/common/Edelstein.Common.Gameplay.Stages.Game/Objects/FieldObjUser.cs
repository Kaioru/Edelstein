using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Users;
using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Users;
using Edelstein.Protocol.Gameplay.Users.Inventories.Modify;
using Edelstein.Protocol.Gameplay.Users.Stats.Modify;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Transport;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects
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
        public bool IsConversing { get; }

        public ICollection<IFieldSplit> Watching { get; }
        public ICollection<IFieldControlledObj> Controlling { get; }

        public FieldObjUser(GameStageUser user)
        {
            GameStageUser = user;
            Watching = new List<IFieldSplit>();
            Controlling = new List<IFieldControlledObj>();
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
            packet.WriteInt(AccountWorld.WorldID);

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

            // Guild
            packet.WriteString("");
            packet.WriteShort(0);
            packet.WriteByte(0);
            packet.WriteShort(0);
            packet.WriteByte(0);

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
            packet.WriteShort(Foothold);
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

        public Task<T> Prompt<T>(Func<
            IConversationSpeaker,
            T
        > function)
        { throw new NotImplementedException(); }
        public Task<T> Prompt<T>(Func<
            IConversationSpeaker,
            IConversationSpeaker,
            T
        > function)
        { throw new NotImplementedException(); }

        public Task Converse(IConversation conversation) { throw new NotImplementedException(); }
        public Task EndConversation() { throw new NotImplementedException(); }

        public Task ModifyStats(Action<IModifyStatContext> action, bool exclRequest = false) { throw new NotImplementedException(); }
        public Task ModifyInventory(Action<IModifyMultiInventoryContext> action, bool exclRequest = false) { throw new NotImplementedException(); }

        public Task Dispatch(IPacket packet) => GameStageUser.Dispatch(packet);
    }
}
