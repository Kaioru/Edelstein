using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects.User.Messages.Impl
{
    public class IncGPMessage : AbstractMessage
    {
        public override MessageType Type => MessageType.IncGPMessage;
        private readonly int _gp;

        public IncGPMessage(int gp)
        {
            _gp = gp;
        }

        protected override void EncodeData(IPacket packet)
        {
            packet.EncodeInt(_gp);
        }
    }
}