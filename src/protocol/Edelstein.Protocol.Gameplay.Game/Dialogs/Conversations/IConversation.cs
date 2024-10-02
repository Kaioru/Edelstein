using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations.Speakers;

namespace Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations;

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
