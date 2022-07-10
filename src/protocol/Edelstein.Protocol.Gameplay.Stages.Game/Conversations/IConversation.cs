using System.Threading.Tasks;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Conversations
{
    public interface IConversation
    {
        IConversationContext Context { get; }
        IConversationSpeaker Self { get; }
        IConversationSpeaker Target { get; }

        Task Start();
    }
}
