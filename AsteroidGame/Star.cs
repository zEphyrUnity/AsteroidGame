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
            g.DrawLine(Pens.PaleVioletRed, 
                _Position.X, _Position.Y, 
                _Position.X + _Size.Width, _Position.Y + _Size.Height);

            g.DrawLine(Pens.MediumVioletRed,
                _Position.X + _Size.Width, _Position.Y,
                _Position.X, _Position.Y + _Size.Height);
        }

        public override void Update()
        {
            _Position.X += _Direction.X;
            if (_Position.X < 0)
                _Position.X = Game.Width + _Size.Width;
        }
    }
}
