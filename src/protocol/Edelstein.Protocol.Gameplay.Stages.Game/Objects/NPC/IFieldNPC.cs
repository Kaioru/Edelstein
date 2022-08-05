using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC.Movements;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC.Templates;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC;

public interface IFieldNPC : IFieldLife<INPCMovePath>, IFieldControllable
{
    INPCTemplate Template { get; }

    bool IsEnabled { get; }

    int RX0 { get; }
    int RX1 { get; }
}
