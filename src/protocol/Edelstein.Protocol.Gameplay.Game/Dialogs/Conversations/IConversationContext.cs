using System;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations.Messages;

namespace Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations;

public interface IConversationContext : IDisposable
{
    CancellationToken Token { get; }
    
    Task<T> Ask<T>(IConversationMessage<T> message);
    Task<T> Answer<T>(IConversationMessageAnswer<T> message);
}
