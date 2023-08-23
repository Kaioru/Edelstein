using Edelstein.Protocol.Gameplay.Game.Objects.NPC;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Game.Contracts;

public record FieldOnPacketNPCMove(
    IFieldUser User,
    IFieldNPC NPC,
    IFieldNPCMovePath Path
);
