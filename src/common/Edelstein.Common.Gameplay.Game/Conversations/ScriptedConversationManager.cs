using Edelstein.Common.Utilities.Repositories;
using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Scripting;

namespace Edelstein.Common.Gameplay.Game.Conversations;

public class ScriptedConversationManager : 
    Repository<string, INamedConversation>, 
    INamedConversationManager
{
    private readonly IScriptEngine _engine;

    public ScriptedConversationManager(IScriptEngine engine) => _engine = engine;

    public override async Task<INamedConversation?> Retrieve(string key)
    {
        var conversation = await base.Retrieve(key);
        if (conversation != null) return conversation;
        
        var script = await _engine.CreateByName(key);
        return script == null ? null : new ScriptedConversation(key, script);
    }
}
