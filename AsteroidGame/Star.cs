using System;
using System.Drawing;

namespace AsteroidGame
{
    class Star : VisualObject
    {
        public Star(Point Position, Point Direction, int Size) 
            : base(Position, Direction, new Size(Size, Size))
        {

        }

        public override void Draw(Graphics g)
        {
            g.DrawImage(Properties.Resources.star2, _Position.X, _Position.Y, _Size.Width, _Size.Height);
        }

        public override void Update()
        {
            _Position.X += _Direction.X;
            if (_Position.X < 0)
                _Position.X = Game.Width + _Size.Width;
        }
    }
}
