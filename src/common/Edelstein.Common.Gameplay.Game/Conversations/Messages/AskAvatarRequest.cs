using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Conversations.Messages;

public class AskAvatarRequest : AbstractConversationMessageRequest<byte>
{
    private readonly int[] _styles;
    private readonly string _text;

    public AskAvatarRequest(
        IConversationSpeaker speaker,
        string text, int[] styles
    ) : base(speaker)
    {
        _text = text;
        _styles = styles;
    }

    public override ConversationMessageType Type => ConversationMessageType.AskAvatar;

    public override bool Check(byte response) => response < _styles.Length;

    protected override void WriteData(IPacketWriter writer)
    {
        writer.WriteString(_text);
        writer.WriteByte((byte)_styles.Length);
        foreach (var style in _styles) writer.WriteInt(style);
    }
}
