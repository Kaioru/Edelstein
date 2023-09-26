using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers.Facades;

namespace Edelstein.Common.Gameplay.Game.Conversations.Speakers.Facades;

public class SpeakerField : ISpeakerField
{
    private readonly IField _field;
    
    public SpeakerField(IField field) => _field = field;

    public int ID => _field.Template.ID;
}
