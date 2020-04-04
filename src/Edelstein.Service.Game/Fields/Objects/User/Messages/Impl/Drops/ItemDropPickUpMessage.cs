using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects.User.Messages.Impl.Drops
{
    public class ItemDropPickUpMessage : AbstractMessage
    {
        public override MessageType Type => MessageType.DropPickUpMessage;
        private readonly int _templateID;

        public ItemDropPickUpMessage(int templateID)
        {
            _templateID = templateID;
        }

        protected override void EncodeData(IPacketEncoder packet)
        {
            packet.EncodeByte(2);

            packet.EncodeInt(_templateID);
        }
    }
}