using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace KnightBlock
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This program will determine if you can block a knight in chess " +
                              "from reaching a certain point on a board by placing only one " +
                              "additional obstacle.");

            // Build the board.
            Console.WriteLine("\nPlease enter a board width...");
            var horizontalSize = int.Parse(Console.ReadLine());

            Console.WriteLine("\nPlease enter a board height...");
            var verticalSize = int.Parse(Console.ReadLine());

            Console.WriteLine("\nPlease enter an obstacle count for the board..");
            var obstacleCount = int.Parse(Console.ReadLine());

            var board = new ChessBoardFactory()
                .BuildKnightsMoveBoard(horizontalSize, verticalSize, obstacleCount);

            // Set the beginning and end for the paths.
            Console.WriteLine("\nPlease enter a starting space X coordinate...");
            var startingX = int.Parse(Console.ReadLine());

            Console.WriteLine("\nPlease enter a starting space Y coordinate...");
            var startingY = int.Parse(Console.ReadLine());

            var startingSpace = board.GetSpace(startingX - 1, startingY - 1);

            Console.WriteLine("\nPlease enter an ending space X coordinate...");
            var endingX = int.Parse(Console.ReadLine());

            Console.WriteLine("\nPlease enter an ending space Y coordinate...");
            var endingY = int.Parse(Console.ReadLine());

            var endingSpace = board.GetSpace(endingX - 1, endingY - 1);

            // Get all the unique paths.
            var visited = new List<ChessSpace>();
            var paths = new List<List<ChessSpace>>();
            while (!paths.Any() || paths.Last() != null)
                paths.Add(FindPathToEnd(startingSpace, endingSpace, visited));

            // If there was only one path returned, then either there was only one
            // viable path, or the other paths crossed the same point.  Either way,
            // one path returned means we can block it with one move.
            Console.WriteLine(paths.Count == 1
                ? "\nYES - You can block this knight with one obstacle."
                : "\nNO - You cannot block this knight with only one obstacle.");
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// A recursive backtracking algorithm to find a viable path from point A to point B, 
        /// without using spaces that have been visited before.
        /// </summary>
        /// <param name="startingSpace">A</param>
        /// <param name="endingSpace">B</param>
        /// <param name="visited"></param>
        /// <returns>A fresh path from A to B</returns>
        private static List<ChessSpace> FindPathToEnd(ChessSpace startingSpace, ChessSpace endingSpace, List<ChessSpace> visited)
        {
            var path = new List<ChessSpace>();
            var currentSpace = startingSpace;
            while (currentSpace != endingSpace)
            {
                visited.Add(currentSpace);

                var curBelowEnd = currentSpace.X < endingSpace.X;
                var curLeftOfEnd = currentSpace.Y < endingSpace.Y;

                var unvisitedSpaces = currentSpace.LinkedSpaces
                    .Where(space => !space.HasObstacle && !visited.Contains(space))
                    .ToList();

                var nextSpace = unvisitedSpaces.FirstOrDefault(space => space == endingSpace);
                if (nextSpace == null)
                {
                    var orderedSpaces = curBelowEnd ? 
                        unvisitedSpaces.OrderByDescending(space => space.X) : 
                        unvisitedSpaces.OrderBy(space => space.X);

                    orderedSpaces = curLeftOfEnd ?
                        orderedSpaces.OrderByDescending(space => space.Y) :
                        orderedSpaces.OrderBy(space => space.Y);

                    nextSpace = orderedSpaces.FirstOrDefault();
                }

                if (nextSpace == null)
                {
                    // Remove currentSpace, just in case this is a double backtrack.
                    if (path.Contains(currentSpace))
                        path.Remove(currentSpace);

                    // No viable path found.
                    if (!path.Any())
                        return null;

                    // Backtrack because we run out of options.
                    currentSpace = path.Last();
                    continue;
                }

                path.Add(currentSpace);
                currentSpace = nextSpace;
            }
            return path;
        }
    }
}
