using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Conversations
{
    public abstract class AbstractConversationRequest<T> : IConversationRequest<T>
    {
        public abstract ConversationRequestType Type { get; }
        public IConversationSpeaker Speaker { get; }

        protected AbstractConversationRequest(IConversationSpeaker speaker)
            => Speaker = speaker;

        public virtual Task<bool> Check(T response) => Task.FromResult(true);

        public void WriteToPacket(IPacketWriter writer)
        {
            WriteBase(writer);
            WriteData(writer);
        }

        public void WriteBase(IPacketWriter writer)
        {
            writer.WriteByte(0); // SpeakerTypeID
            writer.WriteInt(Speaker.TemplateID);
            writer.WriteByte((byte)Type);
            writer.WriteByte((byte)Speaker.Flags);
        }

        public abstract void WriteData(IPacketWriter writer);
    }
}
