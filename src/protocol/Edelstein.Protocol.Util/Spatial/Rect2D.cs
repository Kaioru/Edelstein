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

        public Rect2D(Point2D middle, Size2D size)
        {
            LeftTop = new Point2D(middle.X - size.Width / 2, middle.Y - size.Height / 2);
            RightBottom = new Point2D(middle.X + size.Width / 2, middle.Y + size.Height / 2);
        }

        public bool Contains(Point2D point)
            =>
                point.X <= RightBottom.X && point.X >= LeftTop.X &&
                point.Y <= RightBottom.Y && point.Y >= LeftTop.Y;
    }
}
