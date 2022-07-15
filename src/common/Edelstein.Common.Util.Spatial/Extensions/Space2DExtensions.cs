using Edelstein.Protocol.Util.Spatial;
using Edelstein.Protocol.Util.Spatial.Collections;

namespace Edelstein.Common.Util.Spatial.Extensions;

public static class Space2DExtensions
{
    public static IEnumerable<TSegment> FindIntersecting<TSegment>(this ISpace2D<TSegment> space, IPoint2D obj)
        where TSegment : ISegment2D =>
        space
            .Find(obj)
            .Where(s => s.Intersects(obj));
}
