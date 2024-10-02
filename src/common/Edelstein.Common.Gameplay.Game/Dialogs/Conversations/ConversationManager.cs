using Edelstein.Common.Utilities.Repositories;
using Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations;
using Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations.Speakers;

namespace Edelstein.Common.Gameplay.Game.Dialogs.Conversations;

public class ConversationManager<TSelf, TTarget> : 
    Repository<string, IConversationManagerEntry<TSelf, TTarget>>,
    IConversationManager<TSelf, TTarget> 
    where TSelf : IConversationSpeaker 
    where TTarget : IConversationSpeaker;
