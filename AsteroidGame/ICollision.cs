using System;
using System.Drawing;
using AsteroidGame.VisualObjects;

namespace AsteroidGame
{
    internal interface ICollision
    {
        Rectangle Rect { get; }
     
        bool CheckCollision(ICollision obj);
    }
}
