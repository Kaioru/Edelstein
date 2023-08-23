using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Gameplay.Models.Accounts;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Gameplay.Models.Inventories.Modify;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Objects.User;

public interface IFieldUser :
    IFieldLife<IFieldUserMovePath, IFieldUserMoveAction>,
    IFieldSplitObserver, IFieldController
{
    IGameStageUser StageUser { get; }

    IAccount Account { get; }
    IAccountWorld AccountWorld { get; }
    ICharacter Character { get; }

    IConversationContext? Conversation { get; }

    bool IsInstantiated { get; set; }
    bool IsConversing { get; }

    IPacket GetSetFieldPacket();

    Task<T?> Prompt<T>(Func<IConversationSpeaker, T> prompt);
    Task<T?> Prompt<T>(Func<IConversationSpeaker, IConversationSpeaker, T> prompt);

    Task Converse(
        IConversation conversation,
        Func<IConversationContext, IConversationSpeaker>? getSpeaker1 = null,
        Func<IConversationContext, IConversationSpeaker>? getSpeaker2 = null
    );

    Task EndConversation();

    Task ModifyInventory(Action<IModifyInventoryGroupContext>? action = null, bool exclRequest = false);
}
