using System;
using System.IO;

namespace MazeSolverByPaulius
{
    public static class Logger
    {
        private static readonly string FileName = "Log.txt";
        private static readonly string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private static readonly string FullName = Path.Combine(DesktopPath, FileName);

        public static void CreateLogFile()
        {
            if (File.Exists(FullName)) File.Delete(FullName);

            using (var streamWriter = new StreamWriter(FullName, false))
            {
                streamWriter.Write(string.Empty);
            }
        }

        public static void WriteToLogFile(string outputLine)
        {
            if (File.Exists(FullName))
                using (var streamWriter = new StreamWriter(FullName, true))
                {
                    streamWriter.WriteLine(outputLine);
                }
        }
    }
}