using Edelstein.Protocol.Gameplay.Stages.Game.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations.Speakers;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Game.Conversations.Messages;

public class AskBoxTextRequest : AbstractConversationMessageRequest<string>
{
    private readonly short _cols;
    private readonly string _default;
    private readonly short _rows;
    private readonly string _text;

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

    public override ConversationMessageType Type => ConversationMessageType.AskBoxText;

    protected override void WriteData(IPacketWriter writer)
    {
        writer.WriteString(_text);
        writer.WriteString(_default);
        writer.WriteShort(_cols);
        writer.WriteShort(_rows);
    }
}
