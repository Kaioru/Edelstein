using Edelstein.Protocol.Gameplay.Game.Dialogs;

namespace Edelstein.Protocol.Gameplay.Game.Conversations;

public interface IConversationDialog : IDialog
{
    IConversationContext Context { get; init; }
}
