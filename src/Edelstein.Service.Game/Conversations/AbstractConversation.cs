using System.Threading.Tasks;
using Edelstein.Service.Game.Conversations.Speakers;

namespace Edelstein.Service.Game.Conversations
{
    public abstract class AbstractConversation : IConversation
    {
        public IConversationContext Context { get; }
        public IConversationSpeaker Self { get; }
        public IConversationSpeaker Target { get; }

        public AbstractConversation(
            IConversationContext context,
            IConversationSpeaker self,
            IConversationSpeaker target
        )
        {
            Context = context;
            Self = self;
            Target = target;
        }

        public Task Start()
            => Task.Run(Run, Context.TokenSource.Token);

        protected abstract Task Run();
    }
}