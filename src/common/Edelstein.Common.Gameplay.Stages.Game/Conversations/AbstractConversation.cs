using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations;

namespace Edelstein.Common.Gameplay.Stages.Game.Conversations
{
    public abstract class AbstractConversation : IConversation
    {
        public IConversationContext Context { get; }
        public IConversationSpeaker Self { get; }
        public IConversationSpeaker Target { get; }

        protected AbstractConversation(
            IConversationContext context,
            IConversationSpeaker self,
            IConversationSpeaker target
        )
        {
            Context = context;
            Self = self;
            Target = target;
        }

        public abstract Task Start();
    }
}
