namespace Edelstein.Protocol.Gameplay.Game.Conversations;

public interface IConversationManager
{
    Task<IConversation> Create(string name);
}
