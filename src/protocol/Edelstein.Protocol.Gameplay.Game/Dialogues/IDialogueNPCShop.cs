using Edelstein.Protocol.Gameplay.Game.Objects.NPC;

namespace Edelstein.Protocol.Gameplay.Game.Dialogues;

public interface IDialogueNPCShop : IDialogue
{
    INPCShop Shop { get; }
}
