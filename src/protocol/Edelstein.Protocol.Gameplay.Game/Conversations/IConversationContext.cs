using System;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Game.Conversations.Messages;

namespace Edelstein.Protocol.Gameplay.Game.Conversations;

public interface IConversationContext : IDisposable
{
    CancellationToken Token { get; }
    
    Task<T> Ask<T>(IConversationMessage<T> message);
    Task<T> Answer<T>(IConversationMessageAnswer<T> message);
}
