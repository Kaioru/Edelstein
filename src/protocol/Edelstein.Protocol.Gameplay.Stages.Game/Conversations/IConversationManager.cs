namespace Edelstein.Protocol.Gameplay.Stages.Game.Conversations;

public interface IConversationManager
{
    Task<IConversation> Create(string name);
}
