using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects.User.Messages.Types.Quests
{
    public abstract class AbstractQuestMessage : AbstractMessage
    {
        private readonly short _templateID;

        protected AbstractQuestMessage(short templateID)
        {
            _templateID = templateID;
        }

        protected override void EncodeData(IPacket packet)
        {
            packet.Encode<short>(_templateID);
        }
    }
}