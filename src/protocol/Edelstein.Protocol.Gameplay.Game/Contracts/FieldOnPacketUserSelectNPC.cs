using Edelstein.Protocol.Gameplay.Game.Objects.NPC;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Game.Contracts;

public record FieldOnPacketUserSelectNPC(
    IFieldUser User,
    IFieldNPC NPC
);
