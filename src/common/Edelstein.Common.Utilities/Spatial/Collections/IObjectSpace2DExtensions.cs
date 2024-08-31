using System.Collections.Generic;
using System.Linq;
using Edelstein.Protocol.Utilities.Spatial;
using Edelstein.Protocol.Utilities.Spatial.Collections;

namespace Edelstein.Common.Utilities.Spatial.Collections;

public static class IObjectSpace2DExtensions
{
    public static IEnumerable<TLine> FindIntersecting<TLine>(this IObjectSpace2D<TLine> space, IPoint2D obj) where TLine : ILine2D 
        => space
            .Find(obj)
            .Where(s => s.Intersects(obj));
}
