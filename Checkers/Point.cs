using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    class Point
    {
        public static Point Empty = new Point(-1, -1);

        public bool Equals(Point p)
        {
            return p.X == X && p.Y == Y;
        }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X { get; private set; }
        public int Y { get; private set; }

        public Point SetX(int x)
        {
            return new Point(x, Y);
        }
        public Point SetY(int y)
        {
            return new Point(X, y);
        }

        public Point MoveBy(int x, int y)
        {
            return new Point(X + x, Y + y);
        }
    }
}
