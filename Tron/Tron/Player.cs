using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tron
{
    class Player
    {
        public Player(Point head, Point direction, ConsoleColor color)
        {
            Head = head;
            Direction = direction;
            Color = color;
        }
        public ConsoleColor Color { get; set; }
        public Point Head { get; set; } //cell
        public Point Direction { get; set; } //int/string/PointOffset(0,1/0,-1/1,0/-1,0)/Enum
        public Point PreviuosDirection { get; set; }
        public int Wins { get; set; }
        public bool IsComputer { get; set; }
        //Point/Cell Tail ?{ get; set; }
    }
}
