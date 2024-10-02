using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;

namespace Edelstein.Common.Gameplay.Game.Conversations;

public class ConversationDialog(
    IConversationContext context
) : IConversationDialog
{
    public IConversationContext Context { get; init; } = context;

    public Task OnOpen(IFieldUser user) => Task.CompletedTask;
    public Task OnClose(IFieldUser user) => Task.CompletedTask;
    
    public Task Close(IFieldUser user)
    {
        Context.Dispose();
        return Task.CompletedTask;
    }
}
