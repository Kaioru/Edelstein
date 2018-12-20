using System.Threading.Tasks;
using MoonSharp.Interpreter;

namespace Edelstein.Service.Game.Conversations.Scripts.Lua
{
    public class LuaScriptConversation : AbstractConversation, IScriptConversation
    {
        public string ScriptPath { get; }

        public LuaScriptConversation(
            string path,
            IConversationContext context,
            ISpeaker self,
            ISpeaker target
        ) : base(context, self, target)
            => ScriptPath = path;

        public override Task Start()
        {
            var script = new Script();

            script.Globals["self"] = UserData.Create(Self);
            script.Globals["target"] = UserData.Create(Target);
            return Task.FromResult(script.DoFile(ScriptPath));
        }
    }
}