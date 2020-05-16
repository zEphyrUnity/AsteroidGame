using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using AsteroidGame.VisualObjects;
using AsteroidGame.Logger;

namespace AsteroidGame
{
    delegate void GameLog(string Message);

    internal static class Game
    {
        private static Label lbl = new Label();
        private static int score = 0;
        private static int asteroid_count = 1;

        private static BufferedGraphicsContext __Context;
        private static BufferedGraphics __Buffer;
        private static VisualObject[] __GameObjects;
        private static VisualObject[] __AsteroidObjects;
        private static Bullet __Bullet;
        private static SpaceShip __SpaceShip;
        private static Timer __Timer;

        public static Form MainForm;


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
            MainForm = form;

            lbl.Text = "0";
            lbl.BackColor = Color.Transparent;

            MainForm.Controls.Add(lbl); 

            ConsoleLogger Logger = new ConsoleLogger();
            TextFileLogger TextLogger = new TextFileLogger("Logger.log");

            GameLog GameLogger = Logger.Log;
            GameLogger += TextLogger.Log;

            GameLogger("Игра Запущена");
            TextLogger.Flush();

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

            __Timer = new Timer { Interval = 50 };
            __Timer.Tick += OnVimerTick;
            __Timer.Start();

            form.KeyDown += OnFormKeyDown;

            //var test_button = new Button();
            //test_button.Width = 70;
            //test_button.Height = 30;
            //test_button.Text = "KKTHX";
            //test_button.Left = 20;
            //test_button.Top = 30;
            //test_button.Click += OnTestButtonClick;

            //form.Controls.Add(test_button);
        }

        //private static void OnTestButtonClick(object sender, EventArgs e)
        //{
        //    MessageBox.Show("Кнопка нажата");
        //}

        private static void OnFormKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.ControlKey:
                    __Bullet = new Bullet(__SpaceShip.Rect.Y);
                    break;
                case Keys.Up:
                    __SpaceShip.MoveUp();
                    break;
                case Keys.Down:
                    __SpaceShip.MoveDown();
                    break;
            }
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

            foreach (var asteriod_object in __AsteroidObjects)
                asteriod_object?.Draw(g);

            __SpaceShip.Draw(g);
            __Bullet?.Draw(g);

            if (!__Timer.Enabled) return;
            __Buffer.Render();
        }

        internal static void AsteroidBuilder()
        {
            List<VisualObject> asteroid_objects = new List<VisualObject>();
            Random rnd = new Random();
            
            const int asteroid_size = 25;
            const int asteroid_max_speed = 20;

            for (var i = 0; i < asteroid_count; i++)
                asteroid_objects.Add(new Asteroid
                    (new Point(rnd.Next(0, Width), rnd.Next(0, Height)),
                     new Point(rnd.Next(0, asteroid_max_speed), 0),
                     asteroid_size));

            __AsteroidObjects = asteroid_objects.ToArray();
        }

        public static void Load()
        {
            List<VisualObject> game_objects = new List<VisualObject>();

            for (var i = 0; i < 10; i++)
            {
                game_objects.Add(new Star(
                    new Point(600, (int)(i / 2.0 *20)),
                    new Point( -i, 0), 20));
            }

            var rnd = new Random();
            const int medkit_count = 5;
            const int medkit_size = 20;
            const int medkit_max_speed = 10;
            for (var i = 0; i < medkit_count; i++)
                game_objects.Add(new Medkit
                    (new Point(rnd.Next(0, Width), rnd.Next(0, Height)),
                     new Point(rnd.Next(0, medkit_max_speed), 0),
                     medkit_size));

            AsteroidBuilder();

            __Bullet = new Bullet(200);
            __GameObjects = game_objects.ToArray();
            __SpaceShip = new SpaceShip(new Point(10, 400), new Point(5, 5), new Size(10, 10));
            __SpaceShip.Destroyed += OnShipDestroyed;
        }

        private static void OnShipDestroyed(object sender, EventArgs e)
        {
            __Timer.Stop();
            var g = __Buffer.Graphics;
            g.Clear(Color.MistyRose);
            g.DrawString("Game over!!!", new Font(FontFamily.GenericSerif, 60, FontStyle.Bold), Brushes.MediumVioletRed, 200, 100);
            __Buffer.Render();
        }

        public static void Update()
        {
            try
            {
                WindowWidth = MainForm.Width;
                WindowHeight = MainForm.Height;
            }
            catch (ArgumentOutOfRangeException exception)
            {
                MessageBox.Show(exception.Message);
                MainForm.Close();
            }

            foreach (var game_object in __GameObjects)
                game_object?.Update();

            bool asteroidExists = false;
            foreach (var asteroid_object in __AsteroidObjects)
            {
                asteroid_object?.Update();

                if (asteroid_object != null) 
                    asteroidExists = true;
            }

            if(asteroidExists == false)
            {
                asteroid_count++;
                AsteroidBuilder();
            } 

            __Bullet?.Update();

            for(var i = 0; i < __AsteroidObjects.Length; i++)
            {
                var obj = __AsteroidObjects[i];
                var g = __Buffer.Graphics;

                if (obj is ICollision)
                {
                    var collision_object = (ICollision)obj;

                    __SpaceShip.CheckCollision(collision_object);

                    if(__Bullet != null)
                        if (__Bullet.CheckCollision(collision_object))
                        {
                            __Bullet = null;
                            lbl.Text = (++score).ToString();

                            __AsteroidObjects[i] = null;
                            System.Media.SystemSounds.Asterisk.Play();
                        }
                }
            }
        }
    }
}
