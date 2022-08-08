using Edelstein.Protocol.Gameplay.Stages.Game.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations.Speakers;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Game.Conversations.Messages;

public class AskSlideMenuRequest : AbstractConversationMessageRequest<int>
{
    private readonly IDictionary<int, string> _menu;
    private readonly int _selected;
    private readonly int _slideMenuType;

    public AskSlideMenuRequest(
        IConversationSpeaker speaker,
        int slideMenuType, int selected, IDictionary<int, string> menu
    ) : base(speaker)
    {
        _slideMenuType = slideMenuType;
        _selected = selected;
        _menu = menu;
    }

    public override ConversationMessageType Type => ConversationMessageType.AskSlideMenu;

    public override bool Check(int response) => _menu.ContainsKey(response);

    protected override void WriteData(IPacketWriter writer)
    {
        writer.WriteInt(_slideMenuType);
        writer.WriteInt(_selected);
        writer.WriteString(string.Join(
            "\r\n",
            _menu.Select(p => "#L" + p.Key + "#" + p.Value + "#l")
        ));
    }
}
