using System;
using System.IO;

namespace DiamondInTheWater
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
            if (!Directory.Exists("Save"))
            {
                Directory.CreateDirectory("Save");
            }
            using (var game = new Game1())
                game.Run();
        }
    }
#endif
}
