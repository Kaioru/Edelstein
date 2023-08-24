using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Scripting;

namespace Edelstein.Common.Gameplay.Game.Conversations;

public class ScriptedConversation : INamedConversation
{
    private readonly IScript _script;

    public ScriptedConversation(string id, IScript script)
    {
        _script = script;
        ID = id;
    }
    
    public string ID { get; }

    public Task Start(IConversationContext ctx, IConversationSpeaker self, IConversationSpeaker target) =>
        _script.Run(new Dictionary<string, object>
        {
            ["Self"] = self,
            ["Target"] = target
        });
}
