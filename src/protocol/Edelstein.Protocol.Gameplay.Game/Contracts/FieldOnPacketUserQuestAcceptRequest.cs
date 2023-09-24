using Edelstein.Protocol.Gameplay.Game.Objects.NPC;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Game.Quests.Templates;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Protocol.Gameplay.Game.Contracts;

public record FieldOnPacketUserQuestAcceptRequest(
    IFieldUser User,
    IQuestTemplate Template,
    int? NPCTemplateID,
    IPoint2D? Position
);
