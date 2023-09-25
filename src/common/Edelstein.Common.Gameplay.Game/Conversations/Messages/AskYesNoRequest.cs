using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Conversations.Messages;

public class AskYesNoRequest : AbstractConversationMessageRequest<bool>
{
    private readonly string _text;

    public AskYesNoRequest(IConversationSpeaker speaker, string text) : base(speaker) =>
        _text = text;

    public override ConversationMessageType Type => ConversationMessageType.AskYesNo;

    protected override void WriteData(IPacketWriter writer) =>
        writer.WriteString(_text);
}
