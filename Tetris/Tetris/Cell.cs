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
        public ConsoleColor Color { get; set; }
        public Point Location { get; set; }
        /// <summary>
        /// Returns the current shape if it's occupied by the current shape
        /// </summary>
        public Shape Shape { get; set; }

        public bool IsOccupied
        {
            get { return Color != ConsoleColor.Black; }
        }

        public bool IsOccupiedByCurrentShape
        {
            get { return Shape != null; }
        }
    }
}



