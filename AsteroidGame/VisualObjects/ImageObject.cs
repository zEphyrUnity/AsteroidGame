using System.Drawing;

namespace AsteroidGame.VisualObjects
{
    internal abstract class ImageObject : VisualObject
    {
        private readonly Image _Image;

        protected ImageObject(Point Position, Point Direction, Size Size, Image Image)
            : base(Position, Direction, Size)
        {
            _Image = Image;
        }

        public override void Draw(Graphics g)
        {
            g.DrawImage(Properties.Resources.asteroid3, _Position.X, _Position.Y, _Size.Width, _Size.Height);
        }
    }
}
