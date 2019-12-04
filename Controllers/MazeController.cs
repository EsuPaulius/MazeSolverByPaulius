using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeSolverByPaulius
{
    public class MazeController
    {
        private bool AllowSolving;

        public int MazeHeight;
        public bool MazeInitialized;
        public int[,] MazeLayout;
        public int MazeWidth;
        public bool NoExit;

        public List<int[]> SolvedMazeHistory;

        public MazeController(int height, int width, int[,] maze)
        {
            if (height > 0
                && width > 0
                && maze.GetLength(0).Equals(height)
                && maze.GetLength(1).Equals(width))
            {
                MazeHeight = height;
                MazeWidth = width;
                MazeLayout = maze;

                MazeInitialized = true;
            }
        }

        public void SolveMaze()
        {
            var startingPosition = GetStartingPositionCoordinates();

            AllowSolving = true;

            var recursiveAnswer = RecursiveSolving(startingPosition, new List<int[]>());

            SolvedMazeHistory = recursiveAnswer;

            if (AllowSolving)
            {
                NoExit = true;
                SolvedMazeHistory = null;
            }
        }

        private List<int[]> RecursiveSolving(Tuple<int, int> currentPosition, List<int[]> history)
        {
            AllowSolving = AllowSolving && !IsExitReached(currentPosition);

            history.Add(new[] {currentPosition.Item1, currentPosition.Item2});

            var topPossible = CheckMovePosibility(Directions.Top, currentPosition);
            var rightPossible = CheckMovePosibility(Directions.Right, currentPosition);
            var bottomPossible = CheckMovePosibility(Directions.Bottom, currentPosition);
            var leftPossible = CheckMovePosibility(Directions.Left, currentPosition);

            var wentDeeper = false;

            if (topPossible && AllowSolving)
            {
                var newPositionToCheck = new Tuple<int, int>(currentPosition.Item1 - 1, currentPosition.Item2);

                if (!history.Any(o => o[0].Equals(newPositionToCheck.Item1) && o[1].Equals(newPositionToCheck.Item2)))
                {
                    wentDeeper = true;
                    RecursiveSolving(newPositionToCheck, history);
                }
            }

            if (rightPossible && AllowSolving)
            {
                var newPositionToCheck = new Tuple<int, int>(currentPosition.Item1, currentPosition.Item2 + 1);

                if (!history.Any(o => o[0].Equals(newPositionToCheck.Item1) && o[1].Equals(newPositionToCheck.Item2)))
                {
                    wentDeeper = true;
                    RecursiveSolving(newPositionToCheck, history);
                }
            }

            if (bottomPossible && AllowSolving)
            {
                var newPositionToCheck = new Tuple<int, int>(currentPosition.Item1 + 1, currentPosition.Item2);

                if (!history.Any(o => o[0].Equals(newPositionToCheck.Item1) && o[1].Equals(newPositionToCheck.Item2)))
                {
                    wentDeeper = true;
                    RecursiveSolving(newPositionToCheck, history);
                }
            }

            if (leftPossible && AllowSolving)
            {
                var newPositionToCheck = new Tuple<int, int>(currentPosition.Item1, currentPosition.Item2 - 1);

                if (!history.Any(o => o[0].Equals(newPositionToCheck.Item1) && o[1].Equals(newPositionToCheck.Item2)))
                {
                    wentDeeper = true;
                    RecursiveSolving(newPositionToCheck, history);
                }
            }

            if (AllowSolving && !wentDeeper)
            {
                var itemToDelete = history.FirstOrDefault(
                    o => o[0].Equals(currentPosition.Item1) && o[1].Equals(currentPosition.Item2));

                history.Remove(itemToDelete);
            }

            if (history[history.Count - 1][0].Equals(currentPosition.Item1)
                && history[history.Count - 1][1].Equals(currentPosition.Item2)
                && !IsExitReached(currentPosition))
                history.Remove(history[history.Count - 1]);

            return history;
        }

        private bool IsExitReached(Tuple<int, int> currentPosition)
        {
            return currentPosition.Item1.Equals(0)
                   || currentPosition.Item1.Equals(MazeHeight - 1)
                   || currentPosition.Item2.Equals(0)
                   || currentPosition.Item2.Equals(MazeWidth - 1);
        }

        private bool CheckMovePosibility(Directions direction, Tuple<int, int> startingPosition)
        {
            var x = startingPosition.Item1;
            var y = startingPosition.Item2;

            var answer = false;

            if (direction.Equals(Directions.Top))
            {
                if (!(x - 1 < 0))
                    if (!MazeLayout[x - 1, y].Equals((int) Elements.Wall))
                        answer = true;
            }
            else if (direction.Equals(Directions.Right))
            {
                if (!(y + 1 > MazeWidth - 1))
                    if (!MazeLayout[x, y + 1].Equals((int) Elements.Wall))
                        answer = true;
            }
            else if (direction.Equals(Directions.Bottom))
            {
                if (!(x + 1 > MazeHeight - 1))
                    if (!MazeLayout[x + 1, y].Equals((int) Elements.Wall))
                        answer = true;
            }
            else if (direction.Equals(Directions.Left))
            {
                if (!(y - 1 < 0))
                    if (!MazeLayout[x, y - 1].Equals((int) Elements.Wall))
                        answer = true;
            }

            return answer;
        }

        private Tuple<int, int> GetStartingPositionCoordinates()
        {
            for (var x = 0; x < MazeHeight; ++x)
            for (var y = 0; y < MazeWidth; ++y)
                if (MazeLayout[x, y].Equals(2))
                    return Tuple.Create(x, y);

            return null;
        }

        public void ChangeMazeStartingPoint(int newX, int newY)
        {
            for (var x = 0; x < MazeHeight; ++x)
            for (var y = 0; y < MazeWidth; ++y)
                if (MazeLayout[x, y].Equals(2))
                    MazeLayout[x, y] = 0;

            MazeLayout[newX - 1, newY - 1] = 2;
        }

        public bool IsWall(int x, int y)
        {
            return ((Elements) MazeLayout[x - 1, y - 1]).Equals(Elements.Wall);
        }
    }
}