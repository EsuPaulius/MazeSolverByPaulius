using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeSolverByPaulius
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var fileReader = new FileReader();

            if (fileReader.DataInitialized)
            {
                var userSelectedStartingPoint = AskForStartingPosition(fileReader.MazeHeight, fileReader.MazeWidth);

                var mazeController = new MazeController(fileReader.MazeHeight, fileReader.MazeWidth, fileReader.Maze);

                if (mazeController.MazeInitialized)
                {
                    CorrectStartingPosition(mazeController, userSelectedStartingPoint);

                    PrintMaze(mazeController.MazeHeight, mazeController.MazeWidth, mazeController.MazeLayout);

                    mazeController.SolveMaze();

                    if (mazeController.NoExit)
                    {
                        Console.WriteLine("Maze has no exit.");
                    }
                    else
                    {
                        PrintMoves(mazeController.SolvedMazeHistory);

                        PrintMaze(mazeController.MazeHeight, mazeController.MazeWidth, mazeController.MazeLayout,
                            mazeController.SolvedMazeHistory);
                    }
                }
                else
                {
                    Console.WriteLine("Error creating maze");
                }
            }
            else
            {
                Console.WriteLine("Error reading \"RPAMaze.txt\" file from Desktop");
            }

            Console.ReadLine();
        }

        private static Tuple<bool, int, int> AskForStartingPosition(int height, int width)
        {
            var selectionExists = false;

            Console.WriteLine("Press ENTER to proceed with text file maze");
            Console.WriteLine("Type anything to select starting position");
            var input = Console.ReadLine();

            var selectedX = 0;
            var selectedY = 0;

            if (!string.IsNullOrEmpty(input))
            {
                var xCorrect = false;
                var yCorrect = false;

                Console.WriteLine("\nEnter X and Y coordinates for new starting position");
                Console.WriteLine($"Maximum X is {height}");
                Console.WriteLine($"Maximum Y is {width}\n");

                Console.Write("X: ");
                var xCoordinate = Console.ReadLine();
                if (int.TryParse(xCoordinate, out selectedX) && selectedX >= 1 && selectedX <= height)
                {
                    Console.WriteLine($"Selected X: {xCoordinate}\n");
                    xCorrect = true;
                }
                else
                {
                    Console.WriteLine("Wrong height position.\n");
                }

                Console.Write("Y: ");
                var yCoordinate = Console.ReadLine();
                if (int.TryParse(yCoordinate, out selectedY) && selectedY >= 1 && selectedY <= width)
                {
                    Console.WriteLine($"Selected Y: {yCoordinate}\n");
                    yCorrect = true;
                }
                else
                {
                    Console.WriteLine("Wrong width position.\n");
                }

                selectionExists = xCorrect && yCorrect;
            }

            if (!selectionExists)
                Console.WriteLine("Continuing with text file maze\n");

            return new Tuple<bool, int, int>(selectionExists, selectedX, selectedY);
        }

        private static void CorrectStartingPosition(MazeController mazeController,
            Tuple<bool, int, int> userSelectedStartingPoint)
        {
            if (userSelectedStartingPoint.Item1)
            {
                if (!mazeController.IsWall(userSelectedStartingPoint.Item2, userSelectedStartingPoint.Item3))
                {
                    mazeController.ChangeMazeStartingPoint(userSelectedStartingPoint.Item2,
                        userSelectedStartingPoint.Item3);
                }
                else
                {
                    Console.WriteLine("Selected starting position is WALL");
                    Console.WriteLine("Continuing with text file maze\n");
                }
            }
        }

        private static void PrintMaze(int height, int width, int[,] maze, List<int[]> history = null)
        {
            if (history != null && history.Any())
                Console.Write("Solved maze:\n");
            else
                Console.Write("Maze:\n");

            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    if (history?.FirstOrDefault(o => o[0].Equals(i) && o[1].Equals(j)) != null)
                        PrintMazeElement((int) Elements.Player);
                    else
                        PrintMazeElement(maze[i, j]);

                    if (j < width - 1) Console.Write(" ");
                }

                Console.Write("\n");
            }

            Console.WriteLine();
        }

        private static void PrintMazeElement(int output)
        {
            var element = (Elements) output;

            if (element.Equals(Elements.Path))
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.White;
            }
            else if (element.Equals(Elements.Wall))
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else if (element.Equals(Elements.Player))
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.DarkGreen;
            }

            Console.Write(output);

            Console.ResetColor();
        }

        private static void PrintMoves(List<int[]> history)
        {
            Console.Write("Moves:\n");

            Logger.CreateLogFile();

            var stepCount = 0;
            for (var i = 0; i < history.Count - 1; i++)
            {
                var xFrom = history[i][0] + 1;
                var yFrom = history[i][1] + 1;

                var xTo = history[i + 1][0] + 1;
                var yTo = history[i + 1][1] + 1;

                var outputString =
                    $"{++stepCount}) X={xFrom} Y={yFrom} --> {GetDirection(history[i], history[i + 1])} --> X={xTo} Y={yTo}";

                Console.WriteLine(outputString);
                Logger.WriteToLogFile(outputString);
            }

            Console.WriteLine();
        }

        private static string GetDirection(int[] fromPoint, int[] toPoint)
        {
            if (fromPoint[0].Equals(toPoint[0]) && fromPoint[1] > toPoint[1])
                return "to LEFT";

            if (fromPoint[0].Equals(toPoint[0]) && fromPoint[1] < toPoint[1])
                return "to RIGHT";

            if (fromPoint[1].Equals(toPoint[1]) && fromPoint[0] > toPoint[0])
                return "to TOP";

            if (fromPoint[1].Equals(toPoint[1]) && fromPoint[0] < toPoint[0])
                return "to BOTTOM";

            return string.Empty;
        }
    }
}