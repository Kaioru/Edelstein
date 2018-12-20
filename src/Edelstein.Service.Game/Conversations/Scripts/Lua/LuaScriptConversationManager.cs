using System.IO;

namespace Edelstein.Service.Game.Conversations.Scripts.Lua
{
    public class LuaScriptConversationManager : IScriptConversationManager
    {
        public string ScriptPath { get; }

        public LuaScriptConversationManager(string path)
            => ScriptPath = path;

        public IScriptConversation Get(
            string name,
            IConversationContext context,
            ISpeaker self,
            ISpeaker target
        ) => new LuaScriptConversation(Path.Combine(ScriptPath, name + ".lua"), context, self, target);
    }
}