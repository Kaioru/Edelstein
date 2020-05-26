using Edelstein.Core.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects.User.Messages.Impl.Drops
{
    public class MoneyDropPickUpMessage : AbstractMessage
    {
        public override MessageType Type => MessageType.DropPickUpMessage;

        public bool Failed { get; set; }
        public int Money { get; set; }
        public short PremiumIPMesoBonus { get; set; }

        protected override void EncodeData(IPacketEncoder packet)
        {
            packet.EncodeByte(1);

            packet.EncodeBool(Failed);
            packet.EncodeInt(Money);
            packet.EncodeShort(PremiumIPMesoBonus);
        }
    }
}