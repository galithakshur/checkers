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
            SizeX = 10;
            SizeY = 20;
            Board = new Cell[SizeX, SizeY];
            Shapes = new Shapes();
            Random1 = new Random();
            Random2 = new Random();
            IsGameOn = true;
            ScoreForRow = 100;
            NextShapePos = new Point(36, 6);

        }

        public Cell[,] Board { get; set; }
        public Shapes Shapes { get; set; }
        public Shape CurrentShape
        {
            get
            {
                if (CurrentShapeLayout == null)
                    return null;
                return CurrentShapeLayout.Shape;
            }
        }
        public Point CurrentShapePos { get; set; }
        public ShapeLayout CurrentShapeLayout { get; set; }

        public int SizeX { get; set; }
        public int SizeY { get; set; }
        public Random Random1 { get; set; }
        public Random Random2 { get; set; }
        public bool IsGameOn { get; set; }
        public int TotalScore { get; set; }
        public int ScoreForRow { get; set; }
        public Point NextShapePos { get; set; }
        public Shape NextShape
        {
            get
            {
                if (NextLayout == null)
                    return null;
                return NextLayout.Shape;
            }
        }
        public ShapeLayout NextLayout { get; set; }
        public void Start()
        {
            CreateBoard();
            DrawBorders();
            DrawScoreBoard();
            AddRandomShapeAndLayout();
            NextShapeBoard();
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

        private void TestFullRows()
        {
            Board[0, 19].Color = ConsoleColor.Green;
            Board[1, 19].Color = ConsoleColor.DarkRed;
            Board[2, 19].Color = ConsoleColor.DarkRed;
            Board[3, 19].Color = ConsoleColor.DarkRed;
            Board[4, 19].Color = ConsoleColor.DarkRed;
            Board[5, 19].Color = ConsoleColor.DarkRed;
            Board[6, 19].Color = ConsoleColor.DarkRed;
            Board[7, 19].Color = ConsoleColor.DarkRed;
            Board[8, 19].Color = ConsoleColor.DarkRed;
            Board[9, 19].Color = ConsoleColor.DarkGreen;
            Board[0, 18].Color = ConsoleColor.Green;
            Board[1, 18].Color = ConsoleColor.DarkRed;
            Board[2, 18].Color = ConsoleColor.DarkRed;
            Board[3, 18].Color = ConsoleColor.DarkRed;
            //Board[4, 18].Color = ConsoleColor.DarkRed;
            //Board[5, 18].Color = ConsoleColor.DarkRed;
            Board[6, 18].Color = ConsoleColor.DarkRed;
            Board[7, 18].Color = ConsoleColor.DarkRed;
            Board[8, 18].Color = ConsoleColor.DarkRed;
            Board[9, 18].Color = ConsoleColor.DarkGreen;
            DrawBoard();
            DeleteFullRows();
            Console.ReadLine();
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
                var cells = CurrentShapeLayout.Layout.Select(p => GetCell(p.MoveBy(CurrentShapePos))).ToList();
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
            for (var j = 0; j < SizeY; j++)
            {
                var row = GetRowCells(j);
                var isRowFull = row.All(t => t.IsOccupied);
                if (isRowFull)
                    fullRows.Add(j);
            }
            return fullRows;
        }
        List<Cell> GetRowCells(int rowY)
        {
            var cells = new List<Cell>();
            for (var i = 0; i < SizeX; i++)
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
            foreach (var row in rows)
            {
                var cells = GetRowCells(row);
                cells.ForEach(cell => cell.Color = ConsoleColor.Black);
                // cells.ForEach(DrawCell());
            }
            rows.ForEach(ApplyGravityOnRow);
            TotalScore += rows.Count * ScoreForRow;
            DrawBoard();
            DrawScoreBoard();
        }

        private void ApplyGravityOnRow(int row)
        {
            for (var j = row; j > 0; j--)  //j > untilLine
            {
                CopyRowAbove(j);
            }
        }

        void CopyRowAbove(int rowIndex)
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

        private void AddRandomShapeAndLayout()
        {
            if (NextLayout == null)         //this will only happen when the game starts
                NextLayout = GetRandomShapeLayout();
            CurrentShapeLayout = NextLayout;
            NextLayout = GetRandomShapeLayout();
            NextShapeBoard();
            AddCurrentShape();
        }


        private ShapeLayout GetRandomShapeLayout()
        {
            var randomNumber = Random1.Next(0, Shapes.AllShapes.Count);
            var shape = Shapes.AllShapes[randomNumber];
            var randomNumber2 = Random2.Next(0, shape.Layouts.Count);
            var layout = shape.Layouts[randomNumber2];
            return layout;
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
                DrawOnBoard(' ', c.Location, c.Color);
            }
        }
        void DrawScoreBoard()
        {
            var ScoreBoard = new string[]
             {
               "---------------",
               "  Total Score  ",
               TotalScore.ToString(),
               "---------------",
             };

            DrawStringArray(ScoreBoard, 30, 10);
        }

        private void DrawStringArray(string[] array, int x, int y)
        {
            var py = y;
            foreach (var line in array)
            {
                var px = x;
                foreach (var ch in line)
                {
                    var pos = new Point(px, py);
                    Draw(ch, pos, ConsoleColor.DarkMagenta);
                    px++;
                }
                py++;
            }
        }
        void NextShapeBoard()
        {
            var NextShapeBoard = new string[]
             {
               "---------------",
               "  Next Shape   ",
               "               ",
               "               ",
               "               ",
               "               ",
               "---------------",
             };
            DrawStringArray(NextShapeBoard, 30, 3);


            DrawShape(NextLayout, NextShapePos, NextShape.Color);
        }

        void DrawOnBoard(char s, Point pos, ConsoleColor bgColor)
        {
            Draw(s, pos.MoveBy(1, 0), bgColor);
        }
        public void AddCurrentShape()
        {
            CurrentShapeLayout.Layout.ForEach(p => Board[p.X, p.Y].Shape = CurrentShapeLayout.Shape);
            CurrentShapePos = new Point(SizeX / 2, 0);
            DrawCurrentShape();
        }
        // draw shape at a specific Position
        void DrawShapeOnBoard(ShapeLayout layout, Point pos, ConsoleColor color)
        {
            //if (color != ConsoleColor.Black && !CanDrawOnBoard(layout, pos))
            //    return;

            //looping again On all layout - can draw does it too 
            foreach (var p in layout.Layout)
            {
                var p2 = p.MoveBy(pos.X, pos.Y);
                var cell = GetCell(p2);
                cell.Color = color;
                if (color == ConsoleColor.Black)
                    cell.Shape = null;
                else
                    cell.Shape = layout.Shape;
            }
            DrawShape(layout, pos, color);

        }
        void DrawShape(ShapeLayout layout, Point pos, ConsoleColor color)
        {
            //if (color != ConsoleColor.Black && !CanDrawOnBoard(layout, pos))
            //    return;

            //looping again On all layout - can draw does it too 
            foreach (var p in layout.Layout)
            {
                var p2 = p.MoveBy(pos.X, pos.Y);
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
            if (!CanDrawOnBoard(CurrentShapeLayout.Layout, newPos))
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
            DrawShapeOnBoard(CurrentShapeLayout, CurrentShapePos, CurrentShape.Color);
        }
        void UndrawCurrentShape()
        {
            DrawShapeOnBoard(CurrentShapeLayout, CurrentShapePos, ConsoleColor.Black);
        }

        int FindLayoutIndex(ShapeLayout layout)
        {
            return layout.Shape.Layouts.IndexOf(layout);
        }
        public void RotateCurrentShape()
        {
            UndrawCurrentShape();

            var layoutIndex = FindLayoutIndex(CurrentShapeLayout);
            layoutIndex++;
            if (layoutIndex >= CurrentShape.Layouts.Count)
                layoutIndex = 0;

            var layout = CurrentShape.Layouts[layoutIndex];
            if (CanDrawOnBoard(layout.Layout, CurrentShapePos))  // this is used twice: here and inside DrawCurrentShape()- inside DrawShape()
                CurrentShapeLayout = layout;
            DrawCurrentShape();
        }
        public bool IsCurrentShapeOnFloor()
        {
            //can I draw the currrent shape layout one line below where it is now?
            return !CanDrawOnBoard(CurrentShapeLayout.Layout, CurrentShapePos.MoveBy(0, 1));
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
// should be above the for loop: 
//var untilLine = rows[0] - 1;
// should be inside the first for loop
//if (untilLine == rows[rows.Count - 1])
//{
//    untilLine = rows[rows.Count - 1] - 1;
//}
//else
//{
//    if (rows.Count > 1)
//        untilLine = rows[i + 1];
//}


