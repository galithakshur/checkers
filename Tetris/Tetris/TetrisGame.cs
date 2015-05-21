using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
            IsGameOn = true;
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
        public bool IsGameOn { get; set; }


        public void Start()
        {
            CreateBoard();
            DrawBoard();
            DrawBorders();
            AddRandomShapeAndLayout();
            var lastApplyGravity = DateTime.Now;
            while (IsGameOn)
            {
                Thread.Sleep(10);
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.UpArrow)
                        RotateCurrentShape();
                    else if (keyInfo.Key == ConsoleKey.DownArrow)
                        ApplyGravity();
                    else if (keyInfo.Key == ConsoleKey.RightArrow)
                        MoveShapeBy(1, 0);
                    else if (keyInfo.Key == ConsoleKey.LeftArrow)
                        MoveShapeBy(-1, 0);
                }
                if (DateTime.Now - lastApplyGravity >= TimeSpan.FromSeconds(1))
                {
                    ApplyGravity();
                    lastApplyGravity = DateTime.Now;
                }
            }
        }
        void ApplyGravity()
        {
            if (IsCurrentShapeOnFloor())
            {
                if (CurrentShapePos.Y == 0)
                    GameOver();
                //
                DeleteFullRows();
                //clear current shape from cells
                var cells = CurrentShapeLayout.Select(p => GetCell(p.MoveBy(CurrentShapePos))).ToList();
                cells.ForEach(cell => cell.Shape = null);
                AddRandomShapeAndLayout();
            }
            else
            {
                MoveShapeBy(0, 1);
            }
        }

        List<int> GetFullRows()
        {
            var fullRows = new List<int>();
            var IsRowFull = true;
            for (var j = SizeY - 1; j > 0; j--)
            {
                for (var i = 1; i < SizeX; i++)
                {
                    //if (Board[i, j].Shape == null && !Board[i, j].IsOccupied)
                    if (Board[i, j].Color == ConsoleColor.Black || Board[i, j].Color == ConsoleColor.Gray)
                    {
                        IsRowFull = false;
                        break;
                    }
                }
                if (IsRowFull)
                    fullRows.Add(j);
            }
            return fullRows;
        }
        List<Cell> GetRowCells(int rowY)
        {
            var cells = new List<Cell>();
            //i is not 0 cause 0 is border
            for (var i = 1; i < SizeX; i++)
            {
                cells.Add(Board[i, rowY]);
            }
            return cells;
        }
        private void DeleteFullRows()
        {
            var rows = GetFullRows();
            if (rows.Count <= 0)
                return;
            //foreach (var row in rows)
            //{
            //    var cells = GetRowCells(row);
            //    cells.ForEach(cell => cell.Color = ConsoleColor.Black);
            //    // cells.ForEach(DrawCell());
            //
            //}
            var untilLine = rows[0] - 1;
            for (var i = 0; i < rows.Count; i++)
            {
                if (rows.Count > 1 && untilLine >= rows[rows.Count-1])
                    untilLine = rows[i + 1];
                for (var j = rows[i]; j > untilLine; j--)
                {   
                    ApplyEmptyRowGravity(j);
                }
            }
            DrawBoard();
            DrawBorders();
        }

        void ApplyEmptyRowGravity(int rowIndex)
        {
            //TODO: move all rows *** above ***  this row one line lower and render them.
            var row = GetRowCells(rowIndex);
            var rowAbove = GetRowCells(rowIndex - 1);
            var i = 0;
            foreach (var cellAbove in rowAbove)
            {
                var cell = row[i];
                cell.Color = cellAbove.Color;
                i++;
            }

        }

        void GameOver()
        {
            Console.SetCursorPosition(10, 10);
            Console.WriteLine("Game Over");
            Thread.Sleep(10);
            IsGameOn = false;
        }

        void MyKeyHandlerFunction(char key)
        {

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
            for (var i = 0; i <= SizeX + 1; i++)
            {
                for (var j = 0; j <= SizeY; j++)
                {
                    var color = ConsoleColor.Black;
                    if (i == SizeX + 1)
                        color = ConsoleColor.Gray;
                    if (j == SizeY)
                        color = ConsoleColor.Gray;
                    if (i == 0)
                        color = ConsoleColor.Gray;
                    Draw(' ', new Point(i, j), color);
                }
            }
        }
        public void DrawBoard()
        {
            //var s = ' ';
            foreach (var c in Board)
            {
                Draw(' ', c.Location, c.Color);
            }
        }
        void DrawOnBoard(char s, Point pos, ConsoleColor bgColor)
        {
            Draw(s, pos.MoveBy(1, 0), bgColor);
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
        public void Draw(char ch, Point pos, ConsoleColor bgColor)
        {
            Console.SetCursorPosition(pos.X, pos.Y);
            var prevColor = Console.BackgroundColor;
            if (prevColor != bgColor)
                Console.BackgroundColor = bgColor;
            Console.Write(ch);
            if (prevColor != bgColor)
                Console.BackgroundColor = prevColor;
        }
    }
}


//for (var j = line; j >= 0; j--)
//{
//    for (var i = 0; i < SizeX; i++)
//    {
//        //Board[i, j].Shape = null;
//        //Board[line, j].Color = ConsoleColor.Black;
//        Board[i, j - 1].Location.MoveBy(0, 1);
//        Board[i, j] = Board[i, j-1];
//        //Board[i, j].Location = Board[i, j - 1].Location.MoveBy(0, 1);
//    }
//}

                    //var cells = GetRowCells(j);

