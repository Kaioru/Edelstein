using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations;
using Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations.Speakers;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;

namespace Edelstein.Common.Gameplay.Game.Dialogs.Conversations;

public class ConversationDialog<TSelf, TTarget>(
    IFieldUser user,
    IConversationContext context,
    IConversation<TSelf, TTarget> conversation,
    TSelf self,
    TTarget target
) 
: IConversationDialog
    where TSelf : IConversationSpeaker
    where TTarget : IConversationSpeaker
{
    public IConversationContext Context { get; } = context;

    public Task OnOpen(IFieldUser user)
        => conversation.Start(context, self, target);
    
    public async Task OnClose(IFieldUser user)
    {
        Context.Dispose();
        await user.ModifyStats(exclRequest: true);
    }

    public Task Close(IFieldUser user)
        => OnClose(user);
}
