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

    // inherit from Point - less changes
    class Direction : Point
    {
        public Direction(int x, int y)
            : base(x, y)
        {

        }
        public Direction()
            : base(0, 0)
        {

        }

        Point _OppositeDirection;
        public Point OppositeDirection
        {
            get
            {
                if (_OppositeDirection == null)
                    _OppositeDirection = new Direction(X * -1, Y * -1);
                return _OppositeDirection;
            }
        }
    }
}



