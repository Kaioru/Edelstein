using System.Threading.Tasks;
using Edelstein.Service.Game.Conversations.Speakers;

namespace Edelstein.Service.Game.Conversations
{
    public interface IConversation
    {
        IConversationContext Context { get; }
        IConversationSpeaker Self { get; }
        IConversationSpeaker Target { get; }

        Task Start();
    }
}