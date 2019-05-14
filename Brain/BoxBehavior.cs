using Microsoft.Xna.Framework;

namespace Brain
{
    class BoxBehavior : Entity
    {
        public int Width;
        public int Height;
        public Point Size => new Point(Width, Height);

        public bool Contains(Point currentMousePoint)
        {
            var entityPosition = Parent.Position;
            var rectangle = new Rectangle((int)entityPosition.X - Size.X / 2, (int)entityPosition.Y - Size.Y / 2, Size.X, Size.Y);
            return rectangle.Contains(currentMousePoint);
        }
    }
}
