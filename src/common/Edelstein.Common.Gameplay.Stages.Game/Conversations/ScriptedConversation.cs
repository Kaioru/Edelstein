using Edelstein.Protocol.Gameplay.Stages.Game.Conversations;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations.Speakers;
using Edelstein.Protocol.Scripting;

namespace Edelstein.Common.Gameplay.Stages.Game.Conversations;

public class ScriptedConversation : IConversation
{
    private readonly IScript _script;

    public ScriptedConversation(IScript script) => _script = script;

    public Task Start(IConversationContext ctx, IConversationSpeaker self, IConversationSpeaker target) =>
        _script.Run(new Dictionary<string, object>
        {
            ["self"] = self,
            ["target"] = target
        });
}
