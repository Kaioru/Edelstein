using Edelstein.Protocol.Utilities.Repositories.Methods;

namespace Edelstein.Protocol.Gameplay.Game.Conversations;

public interface INamedConversationManager :
    IRepositoryMethodInsert<string, INamedConversation>,
    IRepositoryMethodDelete<string, INamedConversation>,
    IRepositoryMethodRetrieve<string, INamedConversation>,
    IRepositoryMethodRetrieveAll<string, INamedConversation>
{
}
