namespace Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations;

public interface IConversationDialog : IDialog
{
    IConversationContext Context { get; }
}
