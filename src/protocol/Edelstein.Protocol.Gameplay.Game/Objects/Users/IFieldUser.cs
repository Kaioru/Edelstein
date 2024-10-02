using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Entities;
using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Gameplay.Game.Dialogs;
using Edelstein.Protocol.Gameplay.Game.Objects.Users.Stats;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Objects.Users;

public interface IFieldUser : 
    IFieldLife<IFieldUserMovePath, IFieldUserMoveAction>, 
    IFieldObjectController,
    IFieldSplitObserver
{
    IGameStageSystem System { get; }
    
    Account Account { get; }
    AccountWorldData AccountWorldData { get; }
    Character Character { get; }
    
    IFieldUserStats Stats { get; }
    
    IDialog? ActiveDialog { get; }
    
    bool IsFirstEnter { get; set; }

    IDispatchable GetDispatchSetField();

    Task Initialize();
    Task Modify(Action<IFieldUserModify> action);

    Task<T?> Prompt<T>(Func<IConversationSpeaker, T> prompt) where T : struct;
    Task<T?> Prompt<T>(Func<IConversationSpeaker, IConversationSpeaker, T> prompt) where T : struct;
    
    Task Converse<TSelf, TTarget>(
        IConversation<TSelf, TTarget> conversation,
        Func<IConversationContext, TSelf> getSpeakerSelf,
        Func<IConversationContext, TTarget> getSpeakerTarget
    ) 
        where TSelf : IConversationSpeaker 
        where TTarget : IConversationSpeaker;

    Task Dialog(IDialog dialog);
    
    Task EndDialog();
}
