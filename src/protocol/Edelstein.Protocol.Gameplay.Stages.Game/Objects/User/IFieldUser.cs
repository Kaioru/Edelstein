using Edelstein.Protocol.Gameplay.Accounts;
using Edelstein.Protocol.Gameplay.Characters;
using Edelstein.Protocol.Gameplay.Inventories.Modify;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations.Speakers;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Messages;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Movements;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Commands;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;

public interface IFieldUser :
    IFieldLife<IUserMovePath, IUserMoveAction>,
    IFieldSplitObserver, IFieldController,
    ICommandContext, ISpeakable
{
    IGameStageUser StageUser { get; }

    IAccount Account { get; }
    IAccountWorld AccountWorld { get; }
    ICharacter Character { get; }

    IConversationContext? Conversation { get; }

    bool IsInstantiated { get; set; }
    bool IsConversing { get; }

    IPacket GetSetFieldPacket();

    Task Message(IFieldMessage message);

    Task Converse(
        IConversation conversation,
        IConversationSpeaker? speaker1 = null,
        IConversationSpeaker? speaker2 = null
    );

    Task EndConversation();

    Task ModifyInventory(Action<IModifyInventoryGroupContext>? action = null, bool exclRequest = false);
}
