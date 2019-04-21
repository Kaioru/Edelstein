using System.Threading.Tasks;
using Edelstein.Core.Scripts;

namespace Edelstein.Service.Game.Conversations.Scripts
{
    public class ScriptedConversation : IConversation
    {
        private readonly IScript _script;

        public IConversationContext Context { get; }
        public ISpeaker Self { get; }
        public ISpeaker Target { get; }

        public ScriptedConversation(IScript script, IConversationContext context, ISpeaker self, ISpeaker target)
        {
            _script = script;
            Context = context;
            Self = self;
            Target = target;
        }

        public async Task Start()
        {
            await _script.Register("self", Self);
            await _script.Register("target", Target);
            await _script.Start(Context.TokenSource.Token);
        }
    }
}