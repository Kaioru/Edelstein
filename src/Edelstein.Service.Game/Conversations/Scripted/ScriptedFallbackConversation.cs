using System.Threading.Tasks;
using Edelstein.Service.Game.Conversations.Speakers;

namespace Edelstein.Service.Game.Conversations.Scripted
{
    public class ScriptedFallbackConversation : AbstractConversation
    {
        private readonly string _script;

        public ScriptedFallbackConversation(
            string script,
            IConversationContext context,
            IConversationSpeaker self,
            IConversationSpeaker target
        ) : base(context, self, target)
            => _script = script;

        protected override Task Run()
            => Self.SayAsync($"The '{_script}' script does not exist.");
    }
}