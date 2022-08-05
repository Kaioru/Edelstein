using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC.Movements;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC.Templates;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC;

public interface IFieldNPC : IFieldLife<INPCMovePath>, IFieldControllable
{
    INPCTemplate Template { get; }
    IRectangle2D Bounds { get; }

    bool IsEnabled { get; }
}
