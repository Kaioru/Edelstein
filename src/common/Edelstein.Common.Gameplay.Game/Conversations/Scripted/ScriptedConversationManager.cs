using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Scripting;

namespace Edelstein.Common.Gameplay.Game.Conversations.Scripted;

public class ScriptedConversationManager : IConversationManager
{
    private readonly IScriptEngine _engine;

    public ScriptedConversationManager(IScriptEngine engine) => _engine = engine;

    public async Task<IConversation> Create(string name)
    {
        var script = await _engine.CreateByName(name);
        if (script == null)
            return new FallbackConversation(name);
        return new ScriptedConversation(script);
    }
}
