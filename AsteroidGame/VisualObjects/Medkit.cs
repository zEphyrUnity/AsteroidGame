using System;
using System.Drawing;

namespace AsteroidGame.VisualObjects
{
    internal class Medkit : ImageObject, ICollision
    {
        public int Power { get; set; } = 5;

        public Medkit(Point Position, Point Direction, int ImageSize)
            : base(Position, Direction, new Size(ImageSize, ImageSize), Properties.Resources.medkit)
        {
        }

        public Rectangle Rect => new Rectangle(_Position, _Size);
        public bool CheckCollision(ICollision obj) => Rect.IntersectsWith(obj.Rect);
    }

}
