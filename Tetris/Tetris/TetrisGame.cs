using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class TetrisGame
    {                                   
        public TetrisGame()
        {
            SizeX = 50;
            SizeY = 20;
            Board = new Cell[SizeX, SizeY];
            Shapes = new Shapes();
            Random1 = new Random();
            Random2 = new Random();
        }

        public Cell[,] Board { get; set; }
        public Shapes Shapes { get; set; }
        public Shape CurrentShape { get; set; }
        public Point CurrentShapePos { get; set; }
        public List<Point> CurrentShapeLayout { get; set; }
        //make imutable
        public int SizeX { get; set; }
        public int SizeY { get; set; }
        public Random Random1 { get; set; }
        public Random Random2 { get; set; }


        public void Start()
        {
            CreateBoard();
            var shapes = new Shapes();
            var shape = shapes.AllShapes[0];
            shape.Color = ConsoleColor.Magenta;
            AddShape(shape, shape.Layouts[1]);
            //
            DrawBoard2();
            CurrentShape = shape;
            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.UpArrow)
                    RotateCurrentShape();
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                    MoveShapeBy(0, 1);
                else if (keyInfo.Key == ConsoleKey.RightArrow)
                    MoveShapeBy(1, 0);
                else if (keyInfo.Key == ConsoleKey.LeftArrow)
                    MoveShapeBy(-1, 0);

                if (IsCurrentShapeOnFloor())
                {
                    //clear current shape from cells
                    var cells = CurrentShapeLayout.Select(p => GetCell(p.MoveBy(CurrentShapePos))).ToList();
                    cells.ForEach(cell => cell.Shape = null);
                    AddRandomShapeAndLayout(shapes, shape);
                }
            }
        }

        private void AddRandomShapeAndLayout(Tetris.Shapes shapes, Shape shape)
        {
            var randomNumber = Random1.Next(0, shapes.AllShapes.Count);
            shape = shapes.AllShapes[randomNumber];
            var randomNumber2 = Random2.Next(0, shape.Layouts.Count);
            var layout = (shape.Layouts[randomNumber2]);
            AddShape(shape, layout);
            // return shape;
        }
        public void CreateBoard()
        {
            ConsoleColor color = ConsoleColor.Black;
            for (var i = 0; i < SizeX; i++)
            {
                for (var j = 0; j < SizeY; j++)
                {
                    var point = new Point(i, j);
                    var cell = new Cell(color, point);
                    Board[i, j] = cell;
                }
            }
        }
        //void DrawBorders()
        //{
        //    Draw(" |", sizeX, 0 - sizeY);
        //    Draw("_", sizeY, 0 - sizeX);
        //    Draw("| ", 0, 0 - sizeY);

        //}
        public void DrawBoard2()
        {
            var s = String.Empty ;
            foreach (var c in Board)
            {
                s = " ";
                if (c.Location.X == SizeX-1 )
                    s = " |";
                if (c.Location.Y == SizeY - 1)
                    s = "_";
                if (c.Location.X == 0)
                    s = "| ";
                 if (c.Location.Y == SizeY - 1 && c.Location.X == 0)
                     s = "|_";
                if (c.Location.Y == SizeY - 1 && c.Location.X == SizeX - 1)
                   s = "_|";
                Draw(s, c.Location, c.Color); // c.value.Location -> c.location
            }
        }
        void DrawOnBoard(string s, Point pos, ConsoleColor bgColor)
        {
            Draw(s, pos.MoveBy(1, 1), bgColor);
        }
        public void DrawBoard()
        {
            foreach (var c in Board)
            {
                Draw(" ", c.Location, c.Color); // c.value.Location -> c.location
            }
        }
        public void AddShape(Shape shape, List<Point> layout)
        {
            CurrentShape = shape;
            CurrentShapeLayout = layout;
            ///?
            CurrentShapePos = new Point(25, 0);
            DrawCurrentShape();
        }
        // draw shape at a specific Position
        void DrawShape(Shape shape, List<Point> layout, Point pos, ConsoleColor color)
        {
            CurrentShape = shape;
            CurrentShapeLayout = layout;
            CurrentShapePos = pos;

            if (color != ConsoleColor.Black && !CanDrawOnBoard(layout, pos))
                return;

            //looping again On all layout - can draw does it too 
            foreach (var p in layout)
            {
                var p2 = p.MoveBy(pos.X, pos.Y);
                var cell = GetCell(p2);
                cell.Color = color;
                if (color == ConsoleColor.Black)
                    cell.Shape = null;
                else
                    cell.Shape = shape;
                DrawOnBoard(" ", p2, color);
            }
        }
        bool CanDrawOnBoard(List<Point> layout, Point pos)
        {
            var canDraw = layout.All(p => CanDrawOnBoard(p.MoveBy(pos.X, pos.Y)));
            return canDraw;
        }
        bool CanDrawOnBoard(Point p)
        {
            var cell = GetCell(p);
            if (cell == null)
                return false;
            if (cell.IsOccupied && !cell.IsOccupiedByCurrentShape)
                return false;
            return true;
        }

        /// <summary>
        /// Gets a cell on a specified location, returns null if out of bounds of the board
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        Cell GetCell(Point p)
        {
            // out of borders
            if (p.X >= SizeX || p.Y >= SizeY)
                return null;
            if (p.X < 0 || p.Y < 0)
                return null;

            var cell = Board[p.X, p.Y];
            return cell;
        }
        public void MoveShapeBy(int x, int y)
        {
            var newPos = CurrentShapePos.MoveBy(x, y);
            if (!CanDrawOnBoard(CurrentShapeLayout, newPos))
                return;// throw new Exception("You cannot move there");
            UndrawCurrentShape();
            CurrentShapePos = newPos;
            DrawCurrentShape();
        }
        void DrawCurrentShape()
        {
            DrawShape(CurrentShape, CurrentShapeLayout, CurrentShapePos, CurrentShape.Color);
        }
        void UndrawCurrentShape()
        {
            DrawShape(CurrentShape, CurrentShapeLayout, CurrentShapePos, ConsoleColor.Black);
        }
        public void RotateCurrentShape()
        {
            UndrawCurrentShape();
            var layoutIndex = CurrentShape.Layouts.IndexOf(CurrentShapeLayout);
            layoutIndex++;
            if (layoutIndex >= CurrentShape.Layouts.Count)
                layoutIndex = 0;

            var layout = CurrentShape.Layouts[layoutIndex];
            CurrentShapeLayout = layout;
            DrawCurrentShape();
        }
        public bool IsCurrentShapeOnFloor()
        {
            //can I draw the currrent shape layout one line below where it is now?
            return !CanDrawOnBoard(CurrentShapeLayout, CurrentShapePos.MoveBy(0, 1));
        }


        void MoveCurrentShapeDown()
        {

        }
        public void DrawShape(Shape shape)
        {
            foreach (var p in shape.Layout)
            {
                DrawOnBoard(" ", p, shape.Color);
            }
        }
        Point LastCursorPos;
        public void Draw(string s, Point pos, ConsoleColor bgColor)
        {
            if (LastCursorPos == null || !LastCursorPos.Equals(pos))
            {
                Console.SetCursorPosition(pos.X, pos.Y);
                LastCursorPos = pos.MoveBy(1, 0);
            }
            var prevColor = Console.BackgroundColor;
            if (prevColor != bgColor)
                Console.BackgroundColor = bgColor;
            Console.Write(s);
            if (prevColor != bgColor)
                Console.BackgroundColor = prevColor;
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



