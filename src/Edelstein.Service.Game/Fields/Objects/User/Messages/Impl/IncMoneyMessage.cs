using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects.User.Messages.Impl
{
    public class IncMoneyMessage : AbstractMessage
    {
        public override MessageType Type => MessageType.IncMoneyMessage;
        private readonly int _money;

        public IncMoneyMessage(int money)
        {
            _money = money;
        }

        protected override void EncodeData(IPacketEncoder packet)
        {
            packet.EncodeInt(_money);
        }
    }
}