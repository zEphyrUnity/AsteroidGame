using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using AsteroidGame.VisualObjects;

namespace AsteroidGame
{
    internal static class Game
    {
        private static BufferedGraphicsContext __Context;
        private static BufferedGraphics __Buffer;
        private static VisualObject[] __GameObjects;
        private static Bullet __Bullet;
        private static SpaceShip __SpaceShip;

        public static Form f;

        const int maxWindowSize = 1000;
        const int minWindowSize = 0;

        public static int Width;
        public static int Height;

        public static int WindowWidth
        {
            get => Width;
            set 
            {
                if (value > maxWindowSize || value < minWindowSize) 
                {
                    throw new ArgumentOutOfRangeException("Ошибка размера по ширине");
                }
                else
                {
                    Width = value;
                }
            }
        }

        public static int WindowHeight
        {
            get => Height;
            set
            {
                if (value > maxWindowSize || value < minWindowSize)
                {
                    throw new ArgumentOutOfRangeException("Ошибка размера по высоте");
                }
                else
                {
                    Height = value;
                }
            }
        }

        public static void Initialize(Form form)
        {
            f = form;

            try
            {
                WindowWidth = form.Width;
                WindowHeight = form.Height;
            }
            catch(ArgumentOutOfRangeException exception)
            {
                MessageBox.Show(exception.Message);
            }

            __Context = BufferedGraphicsManager.Current;
            Graphics g = form.CreateGraphics();
            __Buffer = __Context.Allocate(g, new Rectangle(0, 0, WindowWidth, WindowHeight));

            Timer timer = new Timer { Interval = 50 };
            timer.Tick += OnVimerTick;
            timer.Start();
        }

        private static void OnVimerTick(object sender, EventArgs e)
        {
            Update();
            Draw();
        }

        public static void Draw()
        {
            Graphics g = __Buffer.Graphics;

            g.DrawImage(Properties.Resources.sky, 0, 0);

            foreach (var game_object in __GameObjects)
                game_object?.Draw(g);

            __Bullet?.Draw(g); 
            __Buffer.Render();
        }

        public static void Load()
        {
            List<VisualObject> game_objects = new List<VisualObject>();

            //for(var i = 0; i < 30; i++)
            //{
            //    game_objects.Add(new VisualObject(
            //        new Point(600, i * 1),
            //        new Point(15 - i, 20 - i),
            //        new Size(20, 20)));
            //}

            for (var i = 0; i < 10; i++)
            {
                game_objects.Add(new Star(
                    new Point(600, (int)(i / 2.0 *20)),
                    new Point( -i, 0), 20));
            }

            var rnd = new Random();
            const int asteroid_count = 10;
            const int asteroid_size = 25;
            const int asteroid_max_speed = 20;
            for (var i = 0; i < asteroid_count; i++)
                game_objects.Add(new Asteroid
                    (new Point(rnd.Next(0, Width), rnd.Next(0, Height)),
                     new Point(rnd.Next(0, asteroid_max_speed), 0),
                     asteroid_size));

            __Bullet = new Bullet(200);
            __GameObjects = game_objects.ToArray();
        }

        public static void Update()
        {
            try
            {
                WindowWidth = f.Width;
                WindowHeight = f.Height;
            }
            catch (ArgumentOutOfRangeException exception)
            {
                MessageBox.Show(exception.Message);
                f.Close();
            }

            foreach (var game_object in __GameObjects)
                game_object?.Update();

            __Bullet?.Update();
            if(__Bullet is null || __Bullet.Rect.Left > Width)
            {
                Random rnd = new Random();
                __Bullet = new Bullet(rnd.Next(0, Height));
            }

            for(var i = 0; i < __GameObjects.Length; i++)
            {
                var obj = __GameObjects[i];
                if(obj is ICollision)
                {
                    var collision_object = (ICollision)obj;
                    if(__Bullet != null)
                        if (__Bullet.CheckCollision(collision_object))
                        {
                            __Bullet = null;
                            __GameObjects[i] = null;
                            System.Media.SystemSounds.Asterisk.Play();
                        }
                }
            }
        }
    }
}
