using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    class Piece
    {
        public Piece()
        {
            Position = Point.Empty;
        }
        public Point Position { get; set; }
        public bool IsKing { get; set; }
        public string Display
        {
            get
            {
                return IsKing ? Player.KingDisplay : Display;
            }
        }
        public Player Player { get; set; }


        //public string KingDisplay { get; set; }
    }
}
