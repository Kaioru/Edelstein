namespace Edelstein.Protocol.Util.Spatial
{
    public struct Rect2D
    {
        public Point2D Position { get; }
        public Size2D Size { get; }

        public Rect2D(Point2D position, Size2D size)
        {
            Position = position;
            Size = size;
        }
    }
}
