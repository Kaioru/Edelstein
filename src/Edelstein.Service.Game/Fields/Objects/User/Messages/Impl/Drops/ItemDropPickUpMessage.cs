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

        protected override void EncodeData(IPacket packet)
        {
            packet.Encode<byte>(2);

            packet.Encode<int>(_templateID);
        }
    }
}