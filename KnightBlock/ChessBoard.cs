using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightBlock
{
    public class ChessBoard
    {
        public List<List<ChessSpace>> Spaces { get; set; }

        public ChessBoard()
        {
            Spaces = new List<List<ChessSpace>>();
        }

        public ChessSpace GetSpace(int x, int y)
        {
            // In range check...
            if (x < 0 ||
                y < 0 ||
                x >= Spaces.Count ||
                y >= Spaces[x].Count)
                return null;

            return Spaces[x][y];
        }
    }
}
