using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{

    class Cell
    {
        public Cell(ConsoleColor color, Point point)
        {
            Color = color;
            Location = point;
        }
        public bool Equals(Cell c)
        {
            return c.Color == Color && c.Location == Location;
        }
        public ConsoleColor Color;
        public Point Location;
        public Shape Shape;
    }
}



