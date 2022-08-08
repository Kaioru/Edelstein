using Edelstein.Protocol.Gameplay.Stages.Game.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations.Speakers;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Game.Conversations.Messages;

public class AskNumberRequest : AbstractConversationMessageRequest<int>
{
    private readonly int _default;
    private readonly int _maxNumber;
    private readonly int _minNumber;
    private readonly string _text;

    public AskNumberRequest(
        IConversationSpeaker speaker,
        string text, int @default, int minNumber, int maxNumber
    ) : base(speaker)
    {
        _text = text;
        _default = @default;
        _minNumber = minNumber;
        _maxNumber = maxNumber;
    }

    public override ConversationMessageType Type => ConversationMessageType.AskNumber;

    public override bool Check(int response) => response >= _minNumber && response <= _maxNumber;

    protected override void WriteData(IPacketWriter writer)
    {
        writer.WriteString(_text);
        writer.WriteInt(_default);
        writer.WriteInt(_minNumber);
        writer.WriteInt(_maxNumber);
    }
}
