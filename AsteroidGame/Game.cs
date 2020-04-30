using System;

using System.Windows.Forms;
using System.Drawing;

namespace AsteroidGame
{
    internal static class Game
    {
        private static BufferedGraphicsContext __Context;
        private static BufferedGraphics __Buffer;
        public  static Form f;
        public  static int Width;
        
        const int maxWindowSize = 1000;
        const int minWindowSize = 0;

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
        public static int Height;

        public static int WindowHeight
        {
            get => Height;
            set
            {
                if (value > maxWindowSize || value < minWindowSize)
                {
                    throw new ArgumentOutOfRangeException("Ошибка размера по ширине");
                }
                else
                {
                    Height = value;
                }
            }
        }

        private static VisualObject[] __GameObjects;

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
                game_object.Draw(g);

            __Buffer.Render();
        }

        public static void Load()
        {
            __GameObjects = new VisualObject[30];

            for(var i = 0; i < __GameObjects.Length / 2; i++)
            {
                __GameObjects[i] = new VisualObject(
                    new Point(600, i * 1),
                    new Point(15 - i, 20 - i),
                    new Size(20, 20));
            }

            for (var i = __GameObjects.Length / 2; i < __GameObjects.Length; i++)
            {
                __GameObjects[i] = new Star(
                    new Point(600, (int)(i / 2.0 *20)),
                    new Point( -i, 0), 20);
            }
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
                game_object.Update();
        }
    }
}
