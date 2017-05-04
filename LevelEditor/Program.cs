using System;
using System.IO;

namespace LevelEditor
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new Game1())
            {
                try
                {

                    game.Run();
                }
                finally
                {
                    int i = 0;
                    while (File.Exists("Backup" + i + ".dat"))
                        i++;
                    game.Level.Save("Backup" + i + ".dat");
                }
            }
        }
    }
#endif
}
