using System;
using System.Drawing;

namespace AsteroidGame.VisualObjects
{
    internal class Asteroid : ImageObject, ICollision
    {
        public int Power { get; set; } = 3;

        public Asteroid(Point Position, Point Direction, int ImageSize) 
            : base(Position, Direction, new Size(ImageSize, ImageSize), Properties.Resources.asteroid3)
        {
        }

        public Rectangle Rect => new Rectangle(_Position, _Size);
        public bool CheckCollision(ICollision obj) => Rect.IntersectsWith(obj.Rect);
    }
}
