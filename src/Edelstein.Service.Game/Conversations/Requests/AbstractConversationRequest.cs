using Edelstein.Network.Packets;
using Edelstein.Service.Game.Conversations.Speakers;

namespace Edelstein.Service.Game.Conversations.Requests
{
    public abstract class AbstractConversationRequest<T> : IConversationRequest<T>
    {
        public abstract ConversationRequestType Type { get; }
        protected IConversationSpeaker Speaker { get; }

        protected AbstractConversationRequest(IConversationSpeaker speaker)
            => Speaker = speaker;

        public void Encode(IPacket packet)
        {
            packet.EncodeByte(0); // SpeakerTypeID
            packet.EncodeInt(Speaker.TemplateID);
            packet.EncodeByte((byte) Type);
            packet.EncodeByte((byte) Speaker.Type);

            EncodeData(packet);
        }

        public abstract void EncodeData(IPacket packet);
        public abstract bool Validate(IConversationResponse<T> response);
    }
}