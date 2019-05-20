using System.Threading.Tasks;
using Edelstein.Service.Game.Conversations.Speakers;

namespace Edelstein.Service.Game.Conversations.Scripted
{
    public class FallbackConversation : IConversation
    {
        public IConversationContext Context { get; }
        public ISpeaker Self { get; }
        public ISpeaker Target { get; }

        private readonly string _script;

        public FallbackConversation(IConversationContext context, ISpeaker self, ISpeaker target, string script)
        {
            Context = context;
            Self = self;
            Target = target;
            _script = script;
        }

        public async Task Start()
        {
            await Context.Socket.FieldUser.Message($"The '{_script}' script does not exist.");
        }
    }
}