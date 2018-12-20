using System.Threading.Tasks;

namespace Edelstein.Service.Game.Conversation
{
    public abstract class AbstractConversation : IConversation
    {
        public IConversationContext Context { get; }
        public ISpeaker Self { get; }
        public ISpeaker Target { get; }

        public AbstractConversation(
            IConversationContext context,
            ISpeaker self,
            ISpeaker target
        )
        {
            Context = context;
            Self = self;
            Target = target;
        }

        public abstract Task Start();
    }
}