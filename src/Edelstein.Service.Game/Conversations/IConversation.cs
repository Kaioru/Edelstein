using System.Threading.Tasks;

namespace Edelstein.Service.Game.Conversations
{
    public interface IConversation
    {
        IConversationContext Context { get; }
        ISpeaker Self { get; }
        ISpeaker Target { get; }

        Task Start();
    }
}