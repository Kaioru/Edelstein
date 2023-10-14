using Edelstein.Protocol.Gameplay.Game.Combat.Damage;
using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Gameplay.Game.Dialogues;
using Edelstein.Protocol.Gameplay.Game.Objects.User.Modify;
using Edelstein.Protocol.Gameplay.Models.Accounts;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Modify;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats.Modify;
using Edelstein.Protocol.Gameplay.Models.Inventories.Modify;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Protocol.Gameplay.Game.Objects.User;

public interface IFieldUser :
    IFieldLife<IFieldUserMovePath, IFieldUserMoveAction>,
    IFieldSplitObserver, 
    IFieldObjectController
{
    IGameStageUser StageUser { get; }

    IAccount Account { get; }
    IAccountWorld AccountWorld { get; }
    ICharacter Character { get; }
    
    IFieldUserStats Stats { get; }
    IFieldUserStatsForced StatsForced { get; }
    IDamageCalculator Damage { get; }
    
    IConversationContext? ActiveConversation { get; }
    IDialogue? ActiveDialogue { get; }

    bool IsInstantiated { get; set; }
    bool IsConversing { get; }
    bool IsDialoguing { get; }
    
    short? ActiveChair { get; }
    int ActivePortableChair { get; }
    
    bool IsDirectionMode { get; }
    bool IsStandAloneMode { get; }
    
    ICollection<IFieldObjectOwned> Owned { get; }

    IPacket GetSetFieldPacket();

    Task Message(string message);
    Task Message(IPacketWritable writable);
    Task MessageScriptProgress(string message);
    Task MessageBalloon(string message, short? width = null, short? duration = null, IPoint2D? position = null);
    
    Task Effect(IPacketWritable writable, bool isLocal = true, bool isRemote = true);
    Task EffectField(IPacketWritable writable);
    
    Task<T> Prompt<T>(Func<IConversationSpeaker, T> prompt, T def);
    Task<T> Prompt<T>(Func<IConversationSpeaker, IConversationSpeaker, T> prompt, T def);

    Task Converse(
        IConversation conversation,
        Func<IConversationContext, IConversationSpeaker>? getSpeaker1 = null,
        Func<IConversationContext, IConversationSpeaker>? getSpeaker2 = null
    );
    Task EndConversation();

    Task Dialogue(IDialogue dialogue, Func<IDialogue, Task<bool>>? handleEnter = null);
    Task EndDialogue(Func<IDialogue, Task<bool>>? handleLeave = null);

    Task SetActiveChair(short? chairID);
    Task SetActivePortableChair(int templateID);
    
    Task SetDirectionMode(bool enable, int delay = 0);
    Task SetStandAloneMode(bool enable);

    Task Modify(Action<IFieldUserModify> action);
    
    Task ModifyStats(Action<IModifyStatContext>? action = null, bool exclRequest = false);
    Task ModifyStats(IModifyStatContext context, bool exclRequest = false);
    
    Task ModifyStatsForced(Action<IModifyStatForcedContext>? action = null);
    Task ModifyStatsForced(IModifyStatForcedContext context);
    
    Task ModifyInventory(Action<IModifyInventoryGroupContext>? action = null, bool exclRequest = false);
    Task ModifyInventory(IModifyInventoryGroupContext context, bool exclRequest = false);
    
    Task ModifySkills(Action<IModifySkillContext>? action = null, bool exclRequest = false);
    Task ModifySkills(IModifySkillContext context, bool exclRequest = false);
    
    Task ModifyTemporaryStats(Action<IModifyTemporaryStatContext>? action = null, bool exclRequest = false);
    Task ModifyTemporaryStats(IModifyTemporaryStatContext context, bool exclRequest = false);
}
