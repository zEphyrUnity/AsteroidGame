using System;
using System.Windows.Forms;

namespace AsteroidGame
{
    static class Program
    {
        
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form game_form = new Form();
            const ushort game_form_width = 800;
            const ushort game_form_height = 600;


            game_form.Width = game_form_width;
            game_form.Height = game_form_height;
         
            game_form.Show();

            Game.Initialize(game_form);
            Game.Load();
            Game.Draw();

            Application.Run(game_form);
        }
    }
}
