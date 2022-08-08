using Edelstein.Protocol.Gameplay.Stages.Game.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations.Speakers;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Game.Conversations.Messages;

public class AskMenuRequest : AbstractConversationMessageRequest<int>
{
    private readonly IDictionary<int, string> _menu;
    private readonly string _text;

    public AskMenuRequest(
        IConversationSpeaker speaker,
        string text, IDictionary<int, string> menu
    ) : base(speaker)
    {
        _text = text;
        _menu = menu;
    }

    public override ConversationMessageType Type => ConversationMessageType.AskMenu;

    public override bool Check(int response) => _menu.ContainsKey(response);

    protected override void WriteData(IPacketWriter writer) =>
        writer.WriteString(_text + "\r\n#b" + string.Join(
            "\r\n",
            _menu.Select(p => "#L" + p.Key + "#" + p.Value + "#l")
        ));
}
