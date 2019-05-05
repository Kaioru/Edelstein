using System.IO;
using System.Threading.Tasks;
using Edelstein.Core.Scripts;
using Edelstein.Service.Game.Conversations.Speakers;

namespace Edelstein.Service.Game.Conversations.Scripted
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
            try
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
            catch (FileNotFoundException)
            {
                return new FallbackConversation(context, self, target, name);
            }
        }
    }
}