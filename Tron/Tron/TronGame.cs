using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tron
{
    class TronGame
    {
        Player Player { get; set; }
        public Cell[,] Board { get; set; }
        public int SizeX { get; set; }
        public int SizeY { get; set; }
        public bool IsGameOn { get; set; }
        public Point StartingPoint { get; set; }
        public Point Left { get; set; }
        public Point Right { get; set; }
        public Point Up { get; set; }
        public Point Down { get; set; }
        public int Strikes { get; set; }
        public void Start()
        {
            SizeX = 10;
            SizeY = 10;
            Left = new Point(-1, 0);
            Right = new Point(1, 0);
            Up = new Point(0, -1);
            Down = new Point(0, 1);
            Board = new Cell[SizeX, SizeY];
            CreateBoard();
            DrawBorders();
            StartingPoint = new Point((SizeX / 3) * 2, (SizeY / 3) * 2);
            Player = new Player(StartingPoint, Left, ConsoleColor.Magenta);
            Board[StartingPoint.X, StartingPoint.Y].Color = Player.Color;      //getcell..
            DrawBoard();
            var lastMove = DateTime.Now;
            Console.SetCursorPosition(SizeX + 2, SizeY + 2);   // func ariteextat(point)/" " - to delete
            // To Do: delete the line after game starts
            Console.WriteLine("Press Space Bar To Start");  
            var begine = Console.ReadKey();
            if (begine.Key == ConsoleKey.Spacebar)  // while?
                IsGameOn = true;

            while (IsGameOn)
            {
                Thread.Sleep(10);
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.UpArrow)
                        Player.Direction = Up;
                    else if (keyInfo.Key == ConsoleKey.DownArrow)
                        Player.Direction = Down;
                    else if (keyInfo.Key == ConsoleKey.RightArrow)
                        Player.Direction = Right;
                    else if (keyInfo.Key == ConsoleKey.LeftArrow)
                        Player.Direction = Left;
                    //else if (keyInfo.Key == ConsoleKey.Enter)
                    //    Turbo();
                    Move();
                }
                if (DateTime.Now - lastMove >= TimeSpan.FromSeconds(1))
                {
                    Move();
                    lastMove = DateTime.Now;
                }
            }
        }

        private void Turbo()
        {
            // for the  next x seconds make "speed" faster
        }
        private void Move()
        {
            if (!CanMove())
            {
                Strikes++;
                if (Strikes == 3)
                    GameOver();
                else
                Start();
            }
            else
            {
                Player.Head = Player.Head.MoveBy(Player.Direction);
                Board[Player.Head.X, Player.Head.Y].Color = Player.Color;
                DrawBoard();
            }
        }
        void GameOver()
        {
            Console.SetCursorPosition(SizeX / 2, SizeY / 2);
            Console.WriteLine("Game Over");
            Thread.Sleep(10);
            IsGameOn = false;
        }
        private bool CanMove()
        {
            var newPos = Player.Head.MoveBy(Player.Direction);
            if (!HasReachedABorder(newPos) || Board[newPos.X, newPos.Y].IsOccupied)
                return false;
            return true;
        }
        public bool HasReachedABorder(Point p)
        {
            if (p.X == SizeX || p.Y == SizeY || p.X == -1 || p.Y == -1)
                return false;
            else
                return true;
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
                for (var j = 0; j <= SizeY + 1; j++)
                {
                    var color = ConsoleColor.Black;
                    if (i == SizeX + 1)
                        color = ConsoleColor.Gray;
                    if (j == SizeY + 1)
                        color = ConsoleColor.Gray;
                    if (i == 0)
                        color = ConsoleColor.Gray;
                    if (j == 0)
                        color = ConsoleColor.Gray;
                    Draw(' ', new Point(i, j), color);
                }
            }
        }
        public void DrawBoard()
        {
            foreach (var c in Board)
            {
                DrawOnBoard(' ', c.Location, c.Color);
            }
        }
        void DrawOnBoard(char s, Point pos, ConsoleColor bgColor)
        {
            Draw(s, pos.MoveBy(1, 1), bgColor);
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

//public bool IsPointOutOfBoard(Point p)
//{
//    if (p.X > SizeX || p.Y > SizeY || p.X< 0 || p.Y <0)
//        return false;
//    else
//        return true;
//}

