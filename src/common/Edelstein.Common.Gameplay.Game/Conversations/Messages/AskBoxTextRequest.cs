using Edelstein.Protocol.Gameplay.Game.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Conversations.Messages;

public class AskBoxTextRequest : AbstractConversationMessageRequest<string>
{
    private readonly short _cols;
    private readonly string _default;
    private readonly short _rows;
    private readonly string _text;

    public override ConversationMessageType Type => ConversationMessageType.AskBoxText;

    public AskBoxTextRequest(
        IConversationSpeaker speaker,
        string text, string @default, short rows, short cols
    ) : base(speaker)
    {
        _text = text;
        _default = @default;
        _rows = rows;
        _cols = cols;
    }

    protected override void WriteData(IPacketWriter writer)
    {
        writer.WriteString(_text);
        writer.WriteString(_default);
        writer.WriteShort(_cols);
        writer.WriteShort(_rows);
    }
}
