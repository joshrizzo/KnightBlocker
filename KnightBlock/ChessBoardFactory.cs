
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KnightBlock
{
    public class ChessBoardFactory
    {
        private ChessBoard bord;

        public ChessBoardFactory()
        {
            bord = new ChessBoard();
        }
        public ChessBoard BuildKnightsMoveBoard(int horizontalSize, int verticalSize, int obstacleCount)
        {
            SetupBoard(horizontalSize, verticalSize);
            AddRandomObstacles(obstacleCount);
            LinkSpacesWithKnightsMoves();

            return bord;
        }

        private void SetupBoard(int horizontalSize, int verticalSize)
        {
            for (int x = 0; x < horizontalSize; x++)
            {
                var chessSpaces = new List<ChessSpace>();
                for (int y = 0; y < verticalSize; y++)
                {
                    chessSpaces.Add(new ChessSpace());
                }
                bord.Spaces.Add(chessSpaces);
            }
        }

        private void AddRandomObstacles(int obstacleCount)
        {
            var rng = new Random();
            for (int i = 0; i < obstacleCount; i++)
            {
                var x = rng.Next(bord.Spaces.Count - 1);
                var y = rng.Next(bord.Spaces[x].Count - 1);
                bord.Spaces[x][y].HasObstacle = true;
            }
        }

        private void LinkSpacesWithKnightsMoves()
        {
            for (int x = 0; x < bord.Spaces.Count; x++)
            {
                var column = bord.Spaces[x];

                for (int y = 0; y < column.Count; y++)
                {
                    var space = column[y];

                    AddLinkedSpace(space, x, y, 2, 1);
                    AddLinkedSpace(space, x, y, 1, 2);
                    AddLinkedSpace(space, x, y, -2, -1);
                    AddLinkedSpace(space, x, y, -1, -2);
                    AddLinkedSpace(space, x, y, -2, 1);
                    AddLinkedSpace(space, x, y, -1, 2);
                    AddLinkedSpace(space, x, y, 2, -1);
                    AddLinkedSpace(space, x, y, 1, -2);
                }
            }
        }

        private void AddLinkedSpace(ChessSpace space, int x, int y, int xMove, int yMove)
        {
            var newX = x + xMove;
            var newY = y + yMove;
            var linkedSpace = bord.GetSpace(newX, newY);

            if (linkedSpace != null && !linkedSpace.HasObstacle)
            {
                space.LinkedSpaces.Add(linkedSpace);
            }
        }
    }
}
