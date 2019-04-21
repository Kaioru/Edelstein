using System.Threading.Tasks;
using Edelstein.Core.Scripts;

namespace Edelstein.Service.Game.Conversations.Scripts
{
    public class ScriptedConversationManager : IConversationManager
    {
        private readonly IScriptManager _manager;

        public ScriptedConversationManager(IScriptManager manager)
        {
            _manager = manager;
        }

        public async Task<IConversation> Build(
            string name,
            IConversationContext context,
            ISpeaker self,
            ISpeaker target)
        {
            var script = await _manager.Build(name);
            var conversation = new ScriptedConversation(
                script,
                context,
                self,
                target
            );

            return conversation;
        }
    }
}