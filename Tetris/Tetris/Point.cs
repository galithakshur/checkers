using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
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
//bool CanDraw(List<Point> layout, Point pos)
//{
//    if (IsAtEdges(layout, pos) || IsOnLeftEdge(layout, pos))
//        return false;
//    return true;
//}
//bool IsAtEdges(List<Point> shapeLayout, Point location) //SizeX,SizeY
//{
//    foreach (var p in shapeLayout)
//    {
//        var p2 = p.MoveBy(location.X, location.Y);
//        if (p2.X >= SizeX || p2.Y >= SizeY)
//            return false;
//    }
//    return true;
//}
//bool IsOnFloor(List<Point> shapeLayout, Point location)
//{
//    foreach (var p in shapeLayout)
//    {
//        var p2 = p.MoveBy(location.X, location.Y);
//        // add condition to hitting another shape
//        if (p2.Y >= SizeY)
//            return false;
//    }
//    return true;
//}
//bool IsOnLeftEdge(List<Point> shapeLayout, Point location)
//{
//    foreach (var p in shapeLayout)
//    {
//        var p2 = p.MoveBy(location.X, location.Y);
//        if (p2.X < 0)
//            return false;
//    }
//    return true;
//}

//if (CurrentShapePos.X > SizeX)
//    CurrentShapePos = new Point(SizeX, CurrentShapePos.Y);
//if (CurrentShapePos.Y > SizeY)
//    CurrentShapePos = new Point(CurrentShapePos.X, SizeY);

//bool IsOnRightEdge(List<Point> shapeLayout, Point location)
//{
//    foreach (var p in shapeLayout)
//    {
//        var p2 = p.MoveBy(location.X, location.Y);
//        if (p2.X >= SizeX)
//            return false;
//    }
//    return true;
//}   



//}
//DrawBoard();

//var index = -2;
//var shape = new Shape();
//foreach (var sh in Shapes.AllShapes)
//{
//    if (CurrentShape.Color == sh.Color)
//        shape = sh;
//}
//if (shape != null)
//    foreach (var s in shape.Layouts)
//    {
//        // not correct 
//        if (s.Equals(CurrentShape))
//        {
//            //create this shape (currentShape) according to shapes[shape] 
//            //and then search for the current and return the next 1
//            index = shape.Layouts.IndexOf(s);
//            var newShape = shape.Layouts[index + 1];
//            break;
//        }
//    }
////delete from console current shape draw new shape
//public void MoveShape(Shape shape, int x, int y)
//{
//    var newLayout = new List<Point>();
//    foreach (var p in shape.Layout)
//    {
//        newLayout.Add(p.MoveBy(x, y));
//    }
//    shape.Layout = newLayout;
//    Console.Clear();
//    DrawShape(shape);
//}

//complete moveshape
// make a pointer from cell -> shape
//foreach (var p in shape.Layout)
//{
//    Board[p.X + "_" + p.Y].Color = ConsoleColor.Black;
//}
//var newLayout = new List<Point>();
//foreach (var p in shape.Layout)
//{
//    var newPos = p.MoveBy(x, y);
//    newLayout.Add(newPos);
//    var cell = new Cell(shape.Color, newPos);
//    Board[newPos.X + "_" + newPos.Y] = cell;
//}
//shape.Layout = newLayout;
//Console.Clear();
//DrawBoard();
