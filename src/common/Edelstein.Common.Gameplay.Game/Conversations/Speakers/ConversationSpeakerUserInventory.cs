using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Game.Objects.User.Effects;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Game.Conversations.Speakers;

public class ConversationSpeakerUserInventory : IConversationSpeakerUserInventory
{
    private readonly IFieldUser _user;
    
    public ConversationSpeakerUserInventory(IFieldUser user) => _user = user;

    public void Add(int templateID, short count = 1)
    {
        _user.ModifyInventory(i => i.Add(templateID, count)).Wait();
        _user.Effect(new QuestEffect(new List<Tuple<int, int>> { Tuple.Create(templateID, (int)count) })).Wait();
    }

    public void Remove(int templateID, short count = 1)
    {
        _user.ModifyInventory(i => i.Remove(templateID, count)).Wait();
        _user.Effect(new QuestEffect(new List<Tuple<int, int>> { Tuple.Create(templateID, -count) })).Wait();
    }
}
