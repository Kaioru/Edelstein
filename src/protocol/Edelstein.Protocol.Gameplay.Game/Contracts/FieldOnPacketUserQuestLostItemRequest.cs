using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Game.Quests.Templates;

namespace Edelstein.Protocol.Gameplay.Game.Contracts;

public record FieldOnPacketUserQuestLostItemRequest(
    IFieldUser User,
    IQuestTemplate Template,
    ICollection<int> LostItem
);
