using System.Threading.Tasks;

namespace Edelstein.Service.Game.Conversation
{
    public interface IConversation
    {
        IConversationContext Context { get; }
        ISpeaker Self { get; }
        ISpeaker Target { get; }

        Task Start();
    }
}