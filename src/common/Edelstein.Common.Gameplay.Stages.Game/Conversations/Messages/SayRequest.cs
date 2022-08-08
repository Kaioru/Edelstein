using Edelstein.Protocol.Gameplay.Stages.Game.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations.Speakers;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Game.Conversations.Messages;

public class SayRequest : AbstractConversationMessageRequest<byte>
{
    private readonly bool _isNextEnabled;
    private readonly bool _isPrevEnabled;
    private readonly string _text;

    public SayRequest(
        IConversationSpeaker speaker,
        string text, bool isPrevEnabled, bool isNextEnabled
    ) : base(speaker)
    {
        _text = text;
        _isPrevEnabled = isPrevEnabled;
        _isNextEnabled = isNextEnabled;
    }

    public override ConversationMessageType Type => ConversationMessageType.Say;

    protected override void WriteData(IPacketWriter writer)
    {
        if (Speaker.Flags.HasFlag(ConversationSpeakerFlags.NPCReplacedByNPC))
            writer.WriteInt(Speaker.ID);
        writer.WriteString(_text);
        writer.WriteBool(_isPrevEnabled);
        writer.WriteBool(_isNextEnabled);
    }
}
