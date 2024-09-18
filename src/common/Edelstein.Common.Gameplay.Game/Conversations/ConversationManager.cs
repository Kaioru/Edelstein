using Edelstein.Common.Utilities.Repositories;
using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;

namespace Edelstein.Common.Gameplay.Game.Conversations;

public class ConversationManager<TSelf, TTarget> : 
    Repository<string, IConversationManagerEntry<TSelf, TTarget>>,
    IConversationManager<TSelf, TTarget> 
    where TSelf : IConversationSpeaker 
    where TTarget : IConversationSpeaker;
