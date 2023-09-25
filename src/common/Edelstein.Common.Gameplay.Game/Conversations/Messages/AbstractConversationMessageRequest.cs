using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Conversations.Messages;

public abstract class AbstractConversationMessageRequest<T> : IConversationMessageRequest<T>
{

    protected AbstractConversationMessageRequest(IConversationSpeaker speaker) => Speaker = speaker;
    protected IConversationSpeaker Speaker { get; }
    public abstract ConversationMessageType Type { get; }

    public virtual bool Check(T response) => true;

    public void WriteTo(IPacketWriter writer)
    {
        WriteHeader(writer);
        WriteData(writer);
    }

    private void WriteHeader(IPacketWriter writer)
    {
        writer.WriteByte(0); // SpeakerTypeID
        writer.WriteInt(Speaker.ID);
        writer.WriteByte((byte)Type);
        writer.WriteByte((byte)Speaker.Flags);
    }

    protected abstract void WriteData(IPacketWriter writer);
}
