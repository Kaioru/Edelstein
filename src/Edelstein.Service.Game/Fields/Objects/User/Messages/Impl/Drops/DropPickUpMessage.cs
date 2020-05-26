using Edelstein.Core.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects.User.Messages.Impl.Drops
{
    public class DropPickUpMessage : AbstractMessage
    {
        public override MessageType Type => MessageType.DropPickUpMessage;
        private readonly byte _result;

        public DropPickUpMessage(byte result)
        {
            _result = result;
        }

        protected override void EncodeData(IPacketEncoder packet)
        {
            packet.EncodeByte(_result);
        }
    }
}