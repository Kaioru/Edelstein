using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Gameplay.Game.Conversations;

public interface IConversationManagerEntry<in TSelf, in TTarget> : 
    IConversation<TSelf, TTarget>,
    IRepositoryEntry<string>
    where TSelf : IConversationSpeaker 
    where TTarget : IConversationSpeaker;
