using System.Collections.Generic;
using Edelstein.Protocol.Gameplay.Game.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;

namespace Edelstein.Common.Gameplay.Game.Conversations.Messages;

public record ConversationMessageAskSlideMenu(
    IConversationSpeaker Speaker,
    int SlideMenuType,
    int Selected,
    IDictionary<int, string> Menu
) : IConversationMessage<int>
{
    public ConversationMessageType Type => ConversationMessageType.AskSlideMenu;

    public bool Check(int answer)
        => Menu.ContainsKey(answer);
}
