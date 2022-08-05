using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC.Contracts;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC.Movements;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.NPC.Contracts;

public record NPCMove(
    IFieldUser User,
    IFieldNPC NPC,
    INPCMovePath Path
) : INPCMove;
