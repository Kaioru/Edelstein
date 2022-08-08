using Edelstein.Protocol.Gameplay.Stages.Game.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations.Speakers;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Game.Conversations.Messages;

public class AskYesNoRequest : AbstractConversationMessageRequest<bool>
{
    private readonly string _text;

    public AskYesNoRequest(IConversationSpeaker speaker, string text) : base(speaker) =>
        _text = text;

    public override ConversationMessageType Type => ConversationMessageType.AskYesNo;

    protected override void WriteData(IPacketWriter writer) =>
        writer.WriteString(_text);
}
