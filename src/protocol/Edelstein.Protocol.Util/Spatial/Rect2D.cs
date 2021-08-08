namespace Edelstein.Protocol.Util.Spatial
{
    public struct Rect2D
    {
        public Point2D LeftTop { get; }
        public Point2D RightBottom { get; }

        public int Left => LeftTop.X;
        public int Top => LeftTop.Y;
        public int Right => RightBottom.X;
        public int Bottom => RightBottom.Y;

        public Size2D Size => new(RightBottom.X - LeftTop.X, RightBottom.Y - LeftTop.Y);

        public Rect2D(Point2D leftTop, Point2D rightBottom)
        {
            LeftTop = leftTop;
            RightBottom = rightBottom;
        }
    }
}
