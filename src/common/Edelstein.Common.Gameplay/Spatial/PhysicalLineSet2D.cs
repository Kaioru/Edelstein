using System;
using System.Collections.Generic;
using System.Linq;
using Edelstein.Protocol.Gameplay.Spatial;
using Edelstein.Protocol.Util.Spatial;
using MoreLinq;

namespace Edelstein.Common.Gameplay.Spatial
{
    public class PhysicalLineSet2D<TObject> : IPhysicalCollection2D<TObject> where TObject : class, IPhysicalLine2D
    {
        private IDictionary<int, TObject> _objects;

        public PhysicalLineSet2D()
            => _objects = new Dictionary<int, TObject>();

        public void Insert(TObject obj)
            => _objects[obj.ID] = obj;

        public void Insert(IEnumerable<TObject> objs)
            => objs.ForEach(obj => Insert(obj));

        public TObject Find(int id)
            => _objects.ContainsKey(id) ? _objects[id] : null;

        public TObject FindRandom()
            => _objects.Values.Shuffle().FirstOrDefault();

        public TObject FindNearest(Point2D point)
            => _objects.Values
                .OrderBy(obj => obj.Line.Start.Distance(point) + obj.Line.End.Distance(point))
                .FirstOrDefault();

        public TObject FindNearestBelow(Point2D point)
            => _objects.Values
                .Where(obj => point.X >= obj.Line.Start.X && point.X <= obj.Line.End.X)
                .Where(obj => point.Y <= Math.Max(obj.Line.Start.Y, obj.Line.End.Y))
                .OrderBy(obj => Math.Abs(point.Y - Math.Max(obj.Line.Start.Y, obj.Line.End.Y)))
                .FirstOrDefault();
    }
}
