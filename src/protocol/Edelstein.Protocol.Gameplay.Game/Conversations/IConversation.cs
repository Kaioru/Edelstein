using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;

namespace Edelstein.Protocol.Gameplay.Game.Conversations;

public interface IConversation<in TSelf, in TTarget>
    where TSelf : IConversationSpeaker
    where TTarget : IConversationSpeaker
{
    Task Start(
        IConversationContext ctx,
        TSelf self,
        TTarget target
    );
}
