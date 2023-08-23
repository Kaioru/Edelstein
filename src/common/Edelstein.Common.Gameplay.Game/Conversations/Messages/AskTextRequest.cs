using Edelstein.Protocol.Gameplay.Game.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Conversations.Messages;

public class AskTextRequest : AbstractConversationMessageRequest<string>
{
    private readonly string _default;
    private readonly short _maxLength;
    private readonly short _minLength;
    private readonly string _text;

    public AskTextRequest(
        IConversationSpeaker speaker,
        string text, string @default, short minLength, short maxLength
    ) : base(speaker)
    {
        _text = text;
        _default = @default;
        _minLength = minLength;
        _maxLength = maxLength;
    }

    public override ConversationMessageType Type => ConversationMessageType.AskText;

    protected override void WriteData(IPacketWriter writer)
    {
        writer.WriteString(_text);
        writer.WriteString(_default);
        writer.WriteShort(_minLength);
        writer.WriteShort(_maxLength);
    }
}
