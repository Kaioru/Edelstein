using Edelstein.Protocol.Gameplay.Stages.Game.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations.Speakers;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Game.Conversations.Messages;

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
