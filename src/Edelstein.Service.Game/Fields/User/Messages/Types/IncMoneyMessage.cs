using Edelstein.Network.Packet;

namespace Edelstein.Service.Game.Fields.User.Messages.Types
{
    public class IncMoneyMessage : AbstractMessage
    {
        public override MessageType Type => MessageType.IncMoneyMessage;
        private readonly int _money;

        public IncMoneyMessage(int money)
            => _money = money;

        protected override void EncodeData(IPacket packet)
            => packet.Encode<int>(_money);
    }
}