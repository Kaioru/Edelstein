using System.Threading.Tasks;
using Edelstein.Core.Scripting;
using Edelstein.Service.Game.Conversations.Speakers;

namespace Edelstein.Service.Game.Conversations.Scripted
{
    public class ScriptedConversation : AbstractConversation
    {
        private readonly IScript _script;

        public ScriptedConversation(
            IScript script,
            IConversationContext context,
            IConversationSpeaker self,
            IConversationSpeaker target
        ) : base(context, self, target)
            => _script = script;

        protected override async Task Run()
        {
            _script.Register("self", Self);
            _script.Register("target", Target);
            await _script.Run();
        }
    }
}