using System.Threading.Tasks;
using Edelstein.Service.Game.Conversations.Speakers;

namespace Edelstein.Service.Game.Conversations
{
    public interface IConversationManager
    {
        Task<IConversation> Build(
            string name,
            IConversationContext context,
            IConversationSpeaker self,
            IConversationSpeaker target
        );
    }
}