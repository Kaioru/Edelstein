using Edelstein.Network.Packets;
using Edelstein.Service.Game.Conversations.Speakers;

namespace Edelstein.Service.Game.Conversations
{
    public abstract class AbstractConversationMessage<T> : IConversationMessage<T>
    {
        public abstract ConversationMessageType Type { get; }
        protected ISpeaker Speaker { get; }

        protected AbstractConversationMessage(ISpeaker speaker)
        {
            Speaker = speaker;
        }

        public void Encode(IPacket packet)
        {
            packet.Encode<byte>(0); // SpeakerTypeID
            packet.Encode<int>(Speaker.TemplateID);
            packet.Encode<byte>((byte) Type);
            packet.Encode<byte>((byte) Speaker.ParamType);

            EncodeData(packet);
        }

        public abstract void EncodeData(IPacket packet);
        public abstract bool Validate(T response);
    }
}