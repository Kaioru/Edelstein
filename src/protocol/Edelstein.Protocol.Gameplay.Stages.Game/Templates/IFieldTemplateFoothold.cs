using Edelstein.Protocol.Util.Repositories;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Templates;

public interface IFieldTemplateFoothold : IIdentifiable<int>, IObject2D
{
    int NextID { get; }
    int PrevID { get; }

    ISegment2D Line { get; }
}
