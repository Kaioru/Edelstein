using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Game.Contracts;

public record FieldOnPacketMobMove(
    IFieldUser User,
    IFieldMob Mob,
    IFieldMobMovePath Path
);
