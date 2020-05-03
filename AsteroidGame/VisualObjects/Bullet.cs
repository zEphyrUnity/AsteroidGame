using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidGame.VisualObjects
{
    internal class Bullet : CollisionObject
    {
        private const int __BulletSizeX = 20;
        private const int __BulletSizeY = 12;
        private const int __BulletSpeed = 5;

        public Bullet(int Posisiton) 
            : base(new Point(0, Posisiton), Point.Empty, new Size(__BulletSizeX, __BulletSizeY))
        {
        }

        public override void Draw(Graphics g)
        {
            var rect = new Rectangle(_Position, _Size);
            g.FillEllipse(Brushes.IndianRed, rect);
            g.DrawEllipse(Pens.LightGray, rect);
        }

        public override void Update()
        {
            _Position = new Point(_Position.X + __BulletSpeed, _Position.Y); 
        }
    }
}
