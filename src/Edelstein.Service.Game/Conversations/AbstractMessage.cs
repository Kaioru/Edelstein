using Edelstein.Network.Packet;

namespace Edelstein.Service.Game.Conversations
{
    public abstract class AbstractMessage : IMessage
    {
        public abstract ScriptMessageType Type { get; }
        public ISpeaker Speaker { get; }

        public AbstractMessage(ISpeaker speaker)
            => Speaker = speaker;

        public void Encode(IPacket packet)
        {
            packet.Encode<byte>(Speaker.TypeID);
            packet.Encode<int>(Speaker.TemplateID);
            packet.Encode<byte>((byte) Type);
            packet.Encode<byte>((byte) Speaker.Param);

            EncodeData(packet);
        }

        protected abstract void EncodeData(IPacket packet);
    }
}