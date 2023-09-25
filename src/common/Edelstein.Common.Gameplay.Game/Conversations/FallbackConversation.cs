using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Game.Conversations;

public class FallbackConversation : IConversation
{
    private readonly string _name;
    private readonly IFieldUser _user;

    public FallbackConversation(string name, IFieldUser user)
    {
        _name = name;
        _user = user;
    }

    public Task Start(IConversationContext ctx, IConversationSpeaker self, IConversationSpeaker target) 
        => _user.Message($"The scripted conversation '{_name}' is not available");
}
