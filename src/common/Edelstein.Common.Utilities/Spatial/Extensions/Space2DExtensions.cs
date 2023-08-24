using Edelstein.Protocol.Utilities.Spatial;
using Edelstein.Protocol.Utilities.Spatial.Collections;

namespace Edelstein.Common.Utilities.Spatial.Extensions;

public static class Space2DExtensions
{
    public static IEnumerable<TSegment> FindIntersecting<TSegment>(this ISpace2D<TSegment> space, IPoint2D obj)
        where TSegment : ISegment2D =>
        space
            .Find(obj)
            .Where(s => s.Intersects(obj));
}
