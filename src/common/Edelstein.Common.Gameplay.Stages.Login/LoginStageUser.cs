using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Transport;

namespace Edelstein.Common.Gameplay.Stages.Login
{
    public class LoginStageUser : AbstractServerStageUser<LoginStage, LoginStageUser, LoginStageConfig>, ILoginStageUser<LoginStage, LoginStageUser>
    {
        public override int ID => Account.ID;

        public LoginState State { get; set; }
        public byte? SelectedWorldID { get; set; }
        public byte? SelectedChannelID { get; set; }

        public LoginStageUser(ISocket socket, IPacketProcessor<LoginStage, LoginStageUser> processor) : base(socket, processor)
        {
            IsLoggingIn = true;
            State = LoginState.LoggedOut;
        }

        protected override IPacket GetMigratePacket(byte[] address, short port)
        {
            var packet = new UnstructuredOutgoingPacket(PacketSendOperations.SelectCharacterResult);

            packet.WriteByte(0);
            packet.WriteByte(0);

            foreach (var b in address)
                packet.WriteByte(b);
            packet.WriteShort(port);

            packet.WriteInt(Character.ID);
            packet.WriteByte(0);
            packet.WriteInt(0);

            return packet;
        }
    }
}
