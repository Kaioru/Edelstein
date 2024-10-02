using Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations.Speakers;
using Edelstein.Protocol.Utilities.Repositories.Methods;

namespace Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations;

public interface IConversationManager<TSelf, TTarget> :
    IRepositoryMethodRetrieve<string, IConversationManagerEntry<TSelf, TTarget>>,
    IRepositoryMethodRetrieveAll<string, IConversationManagerEntry<TSelf, TTarget>>,
    IRepositoryMethodInsert<string, IConversationManagerEntry<TSelf, TTarget>>,
    IRepositoryMethodDelete<string, IConversationManagerEntry<TSelf, TTarget>>
    where TSelf : IConversationSpeaker
    where TTarget : IConversationSpeaker;
