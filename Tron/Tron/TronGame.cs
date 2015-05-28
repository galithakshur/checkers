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
            Left = new Direction(-1, 0);
            Right = new Direction(1, 0);
            //Left.OppositeDirection = Right;
            //Right.OppositeDirection = Left;
            Up = new Direction(0, -1);
            Down = new Direction(0, 1);
            //Up.OppositeDirection = Down;
            //Down.OppositeDirection = Up;
            Board = new Cell[SizeX, SizeY];
            Random = new Random();
            Directions = new List<Direction> { Left, Right, Up, Down };
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
        public Direction Left { get; set; }
        public Direction Right { get; set; }
        public Direction Up { get; set; }
        public Direction Down { get; set; }
        public List<Direction> Directions { get; set; }
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
            //For test
            //Player2.IsComputer = true;
            while (IsGameOn)
            {
                Thread.Sleep(10);
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.UpArrow)
                        HandleDirection(Player1, Up);
                    else if (keyInfo.Key == ConsoleKey.DownArrow)
                        HandleDirection(Player1, Down);
                    else if (keyInfo.Key == ConsoleKey.RightArrow)
                        HandleDirection(Player1, Right);
                    else if (keyInfo.Key == ConsoleKey.LeftArrow)
                        HandleDirection(Player1, Left);
                    //else if (keyInfo.Key == ConsoleKey.Enter)
                    //   player1 Turbo();

                    if (!Player2.IsComputer)
                    {
                        if (keyInfo.Key == ConsoleKey.W)
                            HandleDirection(Player2,Up);
                        else if (keyInfo.Key == ConsoleKey.S)
                            HandleDirection(Player2,Down);
                        else if (keyInfo.Key == ConsoleKey.D)
                            HandleDirection(Player2, Right);
                        else if (keyInfo.Key == ConsoleKey.A)
                            HandleDirection(Player2,Left);
                        //else if (keyInfo.Key == ConsoleKey.Enter)
                        //  Player2  Turbo();
                    }
                }
                if (!IsGameOn)
                    break;
                if (DateTime.Now - lastMove >= TimeSpan.FromMilliseconds(100))
                {
                    Move(Player1);
                    Move(Player2);
                    lastMove = DateTime.Now;
                }
            }
        }


        void HandleDirection(Player player, Direction newDirection)
        {
            if (IsOppositeDirection(player, newDirection))
                return;

            player.PreviousDirection = player.Direction;
            player.Direction = newDirection;
        }
        bool IsOppositeDirection(Player player, Point newDirection)
        {
            if (newDirection.Equals(player.Direction.OppositeDirection))
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



        Player GetOpposite(Player player)
        {
            if (player.Color == Player1.Color)
                return Player2;
            return Player1;
        }


        private void Move(Player player)
        {
            Direction dir;
            if (player.IsComputer)
            {
                dir = GetNextRandomDirection(player);
            }
            else
            {
                dir = player.Direction;
                if (!CanMove(player, dir))
                    dir = null;
            }

            if (dir == null)
            {
                LoseGame(player);
                return;
            }

            player.Direction = dir;
            player.Head = player.Head.MoveBy(player.Direction);
            var cell = Board[player.Head.X, player.Head.Y];
            cell.Color = player.Color;
            DrawCell(cell);
        }


        Direction GetNextRandomDirection(Player player)
        {
            var possibleDirs = Directions.Where(dir => CanMove(player, dir)).ToList();
            if (possibleDirs.Count == 0)
                return null;
            if (possibleDirs.Contains(player.Direction))
                possibleDirs.Add(player.Direction);
            var index = Random.Next(0, possibleDirs.Count);
            return possibleDirs[index];
        }

        private bool CanMove(Player player, Direction dir)
        {
            var newPos = player.Head.MoveBy(dir);
            if (HasReachedABorder(newPos) || GetCell(newPos).IsOccupied)
                return false;
            return true;
        }



        void LoseGame(Player player)
        {
            var opponent = GetOpposite(player);
            opponent.Wins++;
            if (opponent.Wins == 3)
                GameOver(); // make the game stop completely
            else
                Start();
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
                DrawCell(c);
            }
        }

        private void DrawCell(Cell c)
        {
            DrawOnBoard(' ', c.Location, c.Color);
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

