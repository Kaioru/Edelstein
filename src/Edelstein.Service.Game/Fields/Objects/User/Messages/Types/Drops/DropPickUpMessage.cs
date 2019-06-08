using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects.User.Messages.Types.Drops
{
    public class DropPickUpMessage : AbstractMessage
    {
        public override MessageType Type => MessageType.DropPickUpMessage;
        private readonly byte _result;

        public DropPickUpMessage(byte result)
        {
            _result = result;
        }

        protected override void EncodeData(IPacket packet)
        {
            packet.Encode<byte>(_result);
        }
    }
}