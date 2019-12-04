using System;
using System.IO;

namespace MazeSolverByPaulius
{
    public static class Logger
    {
        public static void Log()
        {
            var fileName = "Log.txt";
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var fullName = Path.Combine(desktopPath, fileName);

            if (!File.Exists(fullName))
            {
            }
        }
    }
}