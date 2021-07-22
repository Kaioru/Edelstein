namespace Edelstein.Protocol.Util.Spatial
{
    public struct Rect2D
    {
        public Point2D LeftTop { get; }
        public Point2D RightBottom { get; }

        public Size2D Size => new(RightBottom.X - LeftTop.X, RightBottom.Y - LeftTop.Y);

        public Rect2D(Point2D leftTop, Point2D rightBottom)
        {
            LeftTop = leftTop;
            RightBottom = rightBottom;
        }
    }
}
