using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightBlock
{
    public class ChessSpace
    {
        public Guid Id = Guid.NewGuid();

        public List<ChessSpace> LinkedSpaces { get; set; }

        public bool HasObstacle { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public ChessSpace()
        {
            LinkedSpaces = new List<ChessSpace>();
        }
    }
}
