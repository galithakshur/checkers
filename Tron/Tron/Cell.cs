using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tron
{
    class Cell
    {
        public Cell(ConsoleColor color, Point point)
        {
            Color = color;
            Location = point;
        }
        public Point Location { get; set; }
        public ConsoleColor Color { get; set; }
        public Player Player { get; set; }
        public bool Equals(Cell c)
        {
            return c.Color == Color && c.Location == Location;
        }
        public bool IsOccupied
        {
            get { return Color != ConsoleColor.Black; }
        }
    }

}
