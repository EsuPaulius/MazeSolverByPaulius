using System;
using System.IO;
using System.Linq;

namespace MazeSolverByPaulius
{
    public class FileReader
    {
        private readonly string FileName = "RPAMaze.txt";

        public bool DataInitialized;
        private string FileContent;

        private bool FileReadSuccessful;
        public int[,] Maze;

        public int MazeHeight;
        private bool MazeSizeReadSuccessful;
        public int MazeWidth;

        public FileReader()
        {
            ReadFromDesktop();

            if (FileReadSuccessful)
            {
                ReadMazeSize();
                Maze = new int[MazeHeight, MazeWidth];

                if (MazeSizeReadSuccessful)
                    ReadMaze();
            }

            DataInitialized = FileReadSuccessful && MazeSizeReadSuccessful;
        }

        private void ReadMazeSize()
        {
            var textRows = FileContent.Split(new[] {"\r\n", "\r", "\n"}, StringSplitOptions.None);

            if (textRows.Any())
            {
                var firstLineElements = textRows[0].Split(null).ToList();

                if (firstLineElements.Count.Equals(2))
                    MazeSizeReadSuccessful = int.TryParse(firstLineElements[0], out MazeHeight)
                                             && int.TryParse(firstLineElements[1], out MazeWidth)
                                             && MazeHeight > 0
                                             && MazeWidth > 0;
            }
        }

        private void ReadMaze()
        {
            var textRows = FileContent.Split(new[] {"\r\n", "\r", "\n"}, StringSplitOptions.None);

            if (textRows.Length.Equals(MazeHeight + 1))
                for (var i = 1; i < textRows.Length; i++)
                {
                    var rowElements = textRows[i].Split(null).ToList();

                    if (rowElements.Count.Equals(MazeWidth))
                        for (var j = 0; j < rowElements.Count; j++)
                            if (int.TryParse(rowElements[j], out var number))
                                Maze[i - 1, j] = number;
                }
        }

        private void ReadFromDesktop()
        {
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var fullName = Path.Combine(desktopPath, FileName);

            if (File.Exists(fullName))
                using (var steamReader = new StreamReader(fullName))
                {
                    FileContent = steamReader.ReadToEnd();
                }

            FileReadSuccessful = !string.IsNullOrEmpty(FileContent);
        }
    }
}