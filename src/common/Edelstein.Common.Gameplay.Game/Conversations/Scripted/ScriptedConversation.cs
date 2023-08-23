using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Scripting;

namespace Edelstein.Common.Gameplay.Game.Conversations.Scripted;

public class ScriptedConversation : IConversation
{
    private readonly IScript _script;

    public ScriptedConversation(IScript script) => _script = script;

    public Task Start(IConversationContext ctx, IConversationSpeaker self, IConversationSpeaker target) =>
        _script.Run(new Dictionary<string, object>
        {
            ["Self"] = self,
            ["Target"] = target
        });
}
