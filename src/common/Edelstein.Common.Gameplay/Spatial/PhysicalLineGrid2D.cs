using System;
using System.Collections.Generic;
using Edelstein.Protocol.Gameplay.Spatial;
using Edelstein.Protocol.Util.Spatial;
using MoreLinq;

namespace Edelstein.Common.Gameplay.Spatial
{
    public class PhysicalLineGrid2D<TObject> : IPhysicalCollection2D<TObject> where TObject : class, IPhysicalLine2D
    {
        private Rect2D _bounds;
        private Size2D _gridSize;

        private IPhysicalCollection2D<TObject> _objects;
        private IPhysicalCollection2D<TObject>[,] _grid;

        public PhysicalLineGrid2D(Rect2D bounds, Size2D gridSize)
        {
            _bounds = bounds;
            _gridSize = gridSize;

            var rowCount = (_bounds.Size.Height + (_gridSize.Height - 1)) / _gridSize.Height;
            var colCount = (_bounds.Size.Width + (_gridSize.Width - 1)) / _gridSize.Width;

            _objects = new PhysicalLineSet2D<TObject>();
            _grid = new PhysicalLineSet2D<TObject>[rowCount, colCount];

            for (var row = 0; row < rowCount; row++)
                for (var col = 0; col < colCount; col++)
                    _grid[row, col] = new PhysicalLineSet2D<TObject>();
        }

        private IPhysicalCollection2D<TObject> GetCollection(int row, int col)
        {
            if (
                row < 0 || row >= _grid.GetLength(0) ||
                col < 0 || col >= _grid.GetLength(1)
            ) return null;
            return _grid[row, col];
        }

        public IPhysicalCollection2D<TObject> GetCollection(Point2D position)
        {
            var gridPos = GetCollectionPosition(position);
            return GetCollection(gridPos.X, gridPos.Y);
        }

        public Point2D GetCollectionPosition(Point2D position)
        {
            var row = (position.Y - _bounds.Top) / _gridSize.Height;
            var col = (position.X - _bounds.Left) / _gridSize.Width;

            return new Point2D(row, col);
        }

        public void Insert(TObject obj)
        {
            GetCollection(obj.Line.Start)?.Insert(obj);
            GetCollection(obj.Line.End)?.Insert(obj);

            _objects.Insert(obj);
        }

        public void Insert(IEnumerable<TObject> objs)
            => objs.ForEach(obj => Insert(obj));

        public TObject Find(int id)
            => _objects.Find(id);

        public TObject FindRandom()
            => _objects.FindRandom();

        public TObject FindNearest(Point2D point)
            => _objects.FindNearest(point);

        public TObject FindNearestBelow(Point2D point)
        {
            var gridPos = GetCollectionPosition(point);
            var gridRow = gridPos.X;
            var gridRowMax = _grid.GetLength(0);

            for (var i = gridRow; i < gridRowMax; i++)
            {
                var result = GetCollection(i, gridPos.Y).FindNearestBelow(point);

                if (result == null) continue;
                return result;
            }

            return null;
        }
    }
}
