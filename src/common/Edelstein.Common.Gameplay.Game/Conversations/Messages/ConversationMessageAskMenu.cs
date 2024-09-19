using System.Collections.Generic;
using Edelstein.Protocol.Gameplay.Game.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;

namespace Edelstein.Common.Gameplay.Game.Conversations.Messages;

public record ConversationMessageAskMenu(
    IConversationSpeaker Speaker,
    string Text,
    IDictionary<int, string> Menu
) : IConversationMessage<int>
{
    public ConversationMessageType Type => ConversationMessageType.AskMenu;

    public bool Check(int answer)
        => Menu.ContainsKey(answer);
}
