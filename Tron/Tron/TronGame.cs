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
        public TronGame()
        {
            SizeX = 30;
            SizeY = 20;
            Left = new Point(-1, 0);
            Right = new Point(1, 0);
            Left.OppositePoint = Right;
            Right.OppositePoint = Left;
            Up = new Point(0, -1);
            Down = new Point(0, 1);
            Up.OppositePoint = Down;
            Down.OppositePoint = Up;
            Board = new Cell[SizeX, SizeY];
            Random = new Random();
            Directions = new List<Point> { Left, Right, Up, Down };
            Player1 = new Player(StartingPointPlayer1, Left, ConsoleColor.Magenta);
            Player2 = new Player(StartingPointPlayer2, Right, ConsoleColor.Green);
            StartingPointPlayer1 = new Point((SizeX / 3) * 2, SizeY / 2);
            StartingPointPlayer2 = new Point(SizeX / 3, SizeY / 2);

        }
        Player Player1 { get; set; }
        Player Player2 { get; set; }
        Player OposingPlayer { get; set; }
        public Cell[,] Board { get; set; }
        public int SizeX { get; set; }
        public int SizeY { get; set; }
        public bool IsGameOn { get; set; }
        public Point StartingPointPlayer1 { get; set; }
        public Point StartingPointPlayer2 { get; set; }
        public Point Left { get; set; }
        public Point Right { get; set; }
        public Point Up { get; set; }
        public Point Down { get; set; }
        public List<Point> Directions { get; set; }
        public Random Random { get; set; }
        // public int Strikes { get; set; }

        public void Start()
        {
            CreateBoard();
            DrawBorders();
            InitializePlayers();
            DrawBoard();
            DrawScoreBoard();

            var lastMove = DateTime.Now;
            //WriteTextAt(new Point(0, SizeY + 2), "1 player or 2 players Enter 1 or 2 ", ConsoleColor.DarkRed);
            //var players = int.Parse(Console.ReadLine());
            //if (players == 2)
            //    Player2.IsComputer = true;
            //DeleteTextAt(new Point(0, SizeY + 2), ConsoleColor.Black);
            WriteTextAt(new Point(0, SizeY + 2), "Press Space Bar To Start", ConsoleColor.DarkRed);
            var begine = Console.ReadKey();
            if (begine.Key == ConsoleKey.Spacebar)  // while?
                IsGameOn = true;
            DeleteTextAt(new Point(0, SizeY + 2), ConsoleColor.Black);
            while (IsGameOn)
            {
                Thread.Sleep(10);
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.UpArrow)
                        //HandelDirection(Player1,Up);
                        Player1.Direction = Up;
                    else if (keyInfo.Key == ConsoleKey.DownArrow)
                        //HandelDirection(Player1,Down);
                        Player1.Direction = Down;
                    else if (keyInfo.Key == ConsoleKey.RightArrow)
                        //HandelDirection(Player1,Right);
                        Player1.Direction = Right;
                    else if (keyInfo.Key == ConsoleKey.LeftArrow)
                        //HandelDirection(Player1,Left);
                        Player1.Direction = Left;
                    //else if (keyInfo.Key == ConsoleKey.Enter)
                    //   player1 Turbo();

                    if (!Player2.IsComputer)
                    {
                        if (keyInfo.Key == ConsoleKey.W)
                            //HandelDirection(Player2,Up);
                            Player2.Direction = Up;
                        else if (keyInfo.Key == ConsoleKey.S)
                            //HandelDirection(Player2,Down);
                            Player2.Direction = Down;
                        else if (keyInfo.Key == ConsoleKey.D)
                            //HandelDirection(Player2, Right);
                            Player2.Direction = Right;
                        else if (keyInfo.Key == ConsoleKey.A)
                            //HandelDirection(Player2,Left);
                            Player2.Direction = Left;
                        //else if (keyInfo.Key == ConsoleKey.Enter)
                        //  Player2  Turbo();
                    }
                    else
                    {
                        ComputerMove(Player2);
                    }
                    Move(Player1);
                    //Move(Player2);
                    //ComputerMove(Player1); 
                }
                if (!IsGameOn)
                    break;
                if (DateTime.Now - lastMove >= TimeSpan.FromSeconds(1))
                {
                    Move(Player1);
                    //ComputerMove(Player1);
                    //Move(Player2);
                    ComputerMove(Player2);
                    lastMove = DateTime.Now;
                }
            }
        }


        void HandelDirection(Player player, Point newDirection)
        {
            player.PreviuosDirection = player.Direction;
            if (!OppositeDirection(player, newDirection))
                player.Direction = newDirection;
            //else
            // make it so that it will "read another direction
        }
        bool OppositeDirection(Player player, Point direction)
        {
            if (direction == player.PreviuosDirection.OppositePoint)
                return true;
            return false;
        }

        void DrawScoreBoard()
        {
            var scorePlayer1 = new string[]
                    {
                       "---------------",
                       "   Player 1    ",
                        Player1.Wins.ToString()+" Wins",
                       "---------------",
                    };
            DrawStringArray(scorePlayer1, SizeX + 15, 0);
            var scorePlayer2 = new string[]
                    {
                        "---------------",
                        "  Player 2     ",
                        Player2.Wins.ToString()+" Wins",
                        "---------------",
                    };
            DrawStringArray(scorePlayer2, SizeX + 15, 10);
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
        void ComputerMove(Player player)
        {
            if (!CanComputerMove(player))
            {
                if (player.Color == Player1.Color)
                {
                    Player2.Wins++;
                    OposingPlayer = Player2;
                }
                else
                {
                    Player1.Wins++;
                    OposingPlayer = Player1;
                }
                if (OposingPlayer.Wins == 3)
                    GameOver();   // make the game stop completely
                else
                    Start();
            }
            else
            {
                player.Head = player.Head.MoveBy(player.Direction);
                GetCell(player.Head).Color = player.Color;
                DrawBoard();
            }
        }
        private bool CanComputerMove(Player player)
        {
            //var canComputerMove = false;
            foreach (var direction in Directions)
            {
                player.Direction = direction;
                var newPos = player.Head.MoveBy(player.Direction);
                // had a problem with if (!HasReachedABorder(newPos) || !GetCell(newPos).IsOccupied)
                // because if  !HasReachedABorder(newPos) then i'm at (-1,0) and   GetCell() will result in nullrefferanceexception
                if (HasReachedABorder(newPos) || GetCell(newPos).IsOccupied)
                    continue;
                else
                    return true;
            }
            return false;
        }
        private bool CanMove(Player player)
        {
            var newPos = player.Head.MoveBy(player.Direction);
            if (HasReachedABorder(newPos) || GetCell(newPos).IsOccupied)
                return false;
            return true;
        }
        private Point PickRandomDirection()
        {
            var index = Random.Next(0, Directions.Count);
            return Directions[index];
        }
        void DeleteTextAt(Point p, ConsoleColor color)
        {
            var p2 = p;
            {
                for (var i = 0; i < SizeX * 2; i++)
                {
                    Draw(' ', p2, color);
                    p2 = p2.MoveBy(1, 0);
                }
            }
        }
        void WriteTextAt(Point p, string s, ConsoleColor color)
        {
            var p2 = p;
            foreach (var c in s)
            {
                Draw(c, p2, color);
                p2 = p2.MoveBy(1, 0);
            }
        }
        private void InitializePlayers()
        {
            Player1.Direction = Left;
            Player2.Direction = Right;
            Player1.Head = StartingPointPlayer1;
            Player2.Head = StartingPointPlayer2;
            //  StartingPointPlayer1 = new Point((SizeX / 3) * 2, SizeY / 2);
            //  StartingPointPlayer2 = new Point(SizeX / 3, SizeY / 2);
            //  Player1 = new Player(StartingPointPlayer1, Left, ConsoleColor.Magenta);
            //  Player2 = new Player(StartingPointPlayer2, Right, ConsoleColor.Green);
            GetCell(StartingPointPlayer1).Color = Player1.Color;
            GetCell(StartingPointPlayer2).Color = Player2.Color;
        }
        private void Turbo()
        {
            // for the  next x seconds make "speed" faster
        }
        private void Move(Player player)
        {

            if (!CanMove(player))
            {
                if (player.Color == Player1.Color)
                {
                    Player2.Wins++;
                    OposingPlayer = Player2;
                }
                else
                {
                    Player1.Wins++;
                    OposingPlayer = Player1;
                }
                if (OposingPlayer.Wins == 3)
                    GameOver(); // make the game stop completely
                else
                    Start();
            }
            else
            {
                player.Head = player.Head.MoveBy(player.Direction);
                Board[player.Head.X, player.Head.Y].Color = player.Color;
                DrawBoard();
            }
        }
        void GameOver()
        {
            DrawScoreBoard();
            var p = new Point(SizeX / 2, SizeY / 2);
            var s = "Game Over";
            WriteTextAt(p, s, ConsoleColor.Red);
            if (Player1.Wins > Player2.Wins)
                s = "Player2 is the winner";
            else
                s = "Player1 is the winner";
            p = p.MoveBy(Down);
            WriteTextAt(p, s, ConsoleColor.Blue);
            Thread.Sleep(20);
            IsGameOn = false;
        }
        public bool HasReachedABorder(Point p)
        {
            if (p.X == SizeX || p.Y == SizeY || p.X == -1 || p.Y == -1)
                return true;
            else
                return false;
        }
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

