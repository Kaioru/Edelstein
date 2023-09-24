using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Game.Quests.Templates;

namespace Edelstein.Protocol.Gameplay.Game.Contracts;

public record FieldOnPacketUserQuestResignRequest(
    IFieldUser User,
    IQuestTemplate Template
);
