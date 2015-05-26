using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tron
{
    class Point
    {
        public readonly int X;
        public readonly int Y;
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        public bool Equals(Point p)
        {
            return p.X == X && p.Y == Y;
        }
        public Point MoveBy(int dx, int dy)
        {
            return new Point(X + dx, Y + dy);
        }
        public Point MoveBy(Point offsetPoint)
        {
            return MoveBy(offsetPoint.X, offsetPoint.Y);
        }
    }

}
