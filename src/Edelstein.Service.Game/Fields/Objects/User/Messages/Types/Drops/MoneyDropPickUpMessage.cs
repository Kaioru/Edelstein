using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects.User.Messages.Types.Drops
{
    public class MoneyDropPickUpMessage : AbstractMessage
    {
        public override MessageType Type => MessageType.DropPickUpMessage;

        public bool Failed { get; set; }
        public int Money { get; set; }
        public short PremiumIPMesoBonus { get; set; }

        protected override void EncodeData(IPacket packet)
        {
            packet.Encode<byte>(1);

            packet.Encode<bool>(Failed);
            packet.Encode<int>(Money);
            packet.Encode<short>(PremiumIPMesoBonus);
        }
    }
}