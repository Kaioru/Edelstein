using System.Threading.Tasks;

namespace Edelstein.Service.Game.Conversations
{
    public interface IConversationManager
    {
        Task<IConversation> Build(string name, IConversationContext context, ISpeaker self, ISpeaker target);
    }
}