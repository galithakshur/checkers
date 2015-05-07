using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    class Board
    {
        public Board()
        {
            Pieces = new List<Piece>();
            PlayerX = new Player { Display = "X", IsPlayerX = true, YDirection = +1 };
            PlayerO = new Player { Display = "O", IsPlayerX = false, YDirection = -1 };
        }
        public List<Piece> Pieces { get; set; }

        public Player PlayerX { get; set; }
        public Player PlayerO { get; set; }
    }
}
