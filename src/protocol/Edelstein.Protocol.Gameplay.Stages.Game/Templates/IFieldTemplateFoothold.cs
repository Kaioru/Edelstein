using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Templates;

public interface IFieldTemplateFoothold : IFieldSpaceObject
{
    int NextID { get; }
    int PrevID { get; }

    ISegment2D Line { get; }
}
