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
            Board = new Dictionary<string, Cell>();
            Shapes = new Shapes();
        }
        //      public List<Cell> Board;
        public Dictionary<string, Cell> Board;
        public Shapes Shapes;
        public void CreateBoard()
        {
            SizeX = 20;
            SizeY = 20;
            ConsoleColor color = ConsoleColor.Black;
            for (var i = 0; i < SizeX; i++)
            {
                for (var j = 0; j < SizeY; j++)
                {
                    var point = new Point(i, j);
                    var cell = new Cell(color, point);
                    Board.Add(i + "_" + j, cell);

                }
            }

        }
        public void DrawBoard()
        {
            foreach (var pair in Board)
            {
                //var s = cell.Key.Split('_');
                //var x = int.Parse(s[0].ToString());
                //var y = int.Parse(s[1].ToString());
                //var p = new Point(x, y);
                Draw(" ", pair.Value.Location, pair.Value.Color);
            }
        }
        public void AddShape(Shape shape)
        {
            CurrentShape = shape;
            CurrentShapeLayout = shape.Layouts[0];
            CurrentShapePos = new Point(3,3);
            DrawCurrentShape();
        }

        void DrawShape(Shape shape, List<Point> layout, Point pos, ConsoleColor color)
        {
            CurrentShape = shape;
            CurrentShapeLayout = layout;
            CurrentShapePos = pos;
            foreach (var p in layout)
            {
                var p2 = p.MoveBy(pos.X, pos.Y);
                Board[p2.X + "_" + p2.Y].Color = color;
                Draw(" ", p2, color);
            }
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


        }
        public Shape CurrentShape;
        public Point CurrentShapePosition;
        public int SizeX;
        public int SizeY;

        void MoveCurrentShapeDown()
        {

        }
        public void DrawShape(Shape shape)
        {
            foreach (var p in shape.Layout)
            {
                Draw(" ", p, shape.Color);
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
            if(prevColor!=bgColor)
                Console.BackgroundColor = bgColor;
            Console.Write(s);
            if (prevColor != bgColor)
                Console.BackgroundColor = prevColor;
        }
        ///
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
        public void MoveShape(int x, int y)
        {
            UndrawCurrentShape();
            CurrentShapePos = CurrentShapePos.MoveBy(x, y);
            DrawCurrentShape();

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
        }


        public Point CurrentShapePos { get; set; }

        public List<Point> CurrentShapeLayout { get; set; }
    }
}
