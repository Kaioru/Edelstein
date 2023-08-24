using Edelstein.Protocol.Gameplay.Game.Objects.NPC.Templates;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Protocol.Gameplay.Game.Objects.NPC;

public interface IFieldNPC : IFieldLife<IFieldNPCMovePath, IFieldNPCMoveAction>, IFieldControllable
{
    INPCTemplate Template { get; }
    IRectangle2D Bounds { get; }

    bool IsEnabled { get; }
}
