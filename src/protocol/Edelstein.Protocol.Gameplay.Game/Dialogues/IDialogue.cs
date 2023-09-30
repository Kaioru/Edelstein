using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Game.Dialogues;

public interface IDialogue
{
    Task<bool> HandleEnter(IFieldUser user);
    Task<bool> HandleLeave(IFieldUser user);
}
