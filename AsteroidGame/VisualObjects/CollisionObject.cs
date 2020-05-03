using System;
using System.Drawing;

namespace AsteroidGame.VisualObjects
{
    internal abstract class CollisionObject : VisualObject, ICollision
    {
        protected CollisionObject(Point Position, Point Direction, Size Size) 
            : base(Position, Direction, Size) { }

        public Rectangle Rect
        {
            get => new Rectangle(_Position, _Size);
        }

        public bool CheckCollision(ICollision obj) => Rect.IntersectsWith(obj.Rect);
    }
}
