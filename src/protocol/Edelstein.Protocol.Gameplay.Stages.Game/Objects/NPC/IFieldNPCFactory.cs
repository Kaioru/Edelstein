using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC.Templates;
using Edelstein.Protocol.Gameplay.Stages.Game.Spatial;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC;

public interface IFieldNPCFactory
{
    IFieldNPC CreateNPC(
        INPCTemplate template,
        IPoint2D position,
        IFieldFoothold? foothold = null,
        IRectangle2D? bounds = null,
        bool isFacingLeft = true,
        bool isEnabled = true
    );
}
