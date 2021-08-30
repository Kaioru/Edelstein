namespace Edelstein.Protocol.Util.Spatial
{
    public struct Line2D
    {
        public Point2D Start { get; }
        public Point2D End { get; }

        public Line2D(Point2D start, Point2D end)
        {
            Start = start;
            End = end;
        }

        public override string ToString()
            => $"({Start}, {End})";
    }
}
