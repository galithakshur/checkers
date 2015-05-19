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
            SizeX = 20;
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
            DrawBoard2();
            DrawBorders();
            AddRandomShapeAndLayout();
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
                    AddRandomShapeAndLayout();
                }
            }
        }

        private void AddRandomShapeAndLayout()
        {
            var randomNumber = Random1.Next(0, Shapes.AllShapes.Count);
            var shape = Shapes.AllShapes[randomNumber];
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
        void DrawBorders()
        {
            for (var i = 0; i <= SizeX+1; i++)
            {
                for (var j = 0; j <= SizeY; j++)
                {
                    var color = ConsoleColor.Black;
                    //char s = ' ';
                    if (i == SizeX + 1)
                        color = ConsoleColor.Gray;
                        //s = '|';
                    if (j == SizeY)
                        color = ConsoleColor.Gray;
                    // s = '-';
                    if (i == 0)
                        color = ConsoleColor.Gray;
                    //s = '|';
                    //if (j == SizeY  && i== 0)
                    //    s = "|_";
                    //if (j == SizeY && i == SizeX)
                    //    s = '|';
                    Draw(' ', new Point(i, j), color);
                }
            }
            //Draw(" |", sizeX, 0 - sizeY);
            //Draw("_", sizeY, 0 - sizeX);
            //Draw("| ", 0, 0 - sizeY);

        }
        public void DrawBoard2()
        {
            var s = ' ';
            foreach (var c in Board)
            {
                Draw(s, c.Location, c.Color);
            }
        }
        void DrawOnBoard(char s, Point pos, ConsoleColor bgColor)
        {
            Draw(s, pos.MoveBy(1, 0), bgColor);
        }
        public void DrawBoard()
        {
            foreach (var c in Board)
            {
                Draw(' ', c.Location, c.Color); // c.value.Location -> c.location
            }
        }
        public void AddShape(Shape shape, List<Point> layout)
        {
            CurrentShape = shape;
            CurrentShapeLayout = layout;
            ///?
            CurrentShapeLayout.ForEach(p => Board[p.X, p.Y].Shape = shape);
            CurrentShapePos = new Point(SizeX / 2, 0);
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
                DrawOnBoard(' ', p2, color);
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
            // incase shape at border and can't rotate
            // if (CurrentShapePos.X == SizeX)
            //     CurrentShapePos.MoveBy(-2, 0);
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
                DrawOnBoard(' ', p, shape.Color);
            }
        }
        Point LastCursorPos;
        public void Draw(char ch, Point pos, ConsoleColor bgColor)
        {
            if (LastCursorPos == null || !LastCursorPos.Equals(pos))
            {
                Console.SetCursorPosition(pos.X, pos.Y);
                LastCursorPos = pos.MoveBy(1, 0);
            }
            var prevColor = Console.BackgroundColor;
            if (prevColor != bgColor)
                Console.BackgroundColor = bgColor;
            Console.Write(ch);
            if (prevColor != bgColor)
                Console.BackgroundColor = prevColor;
        }
    }
}



