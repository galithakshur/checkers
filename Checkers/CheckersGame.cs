using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Checkers
{
    class CheckersGame
    {
        public Board Board { get; set; }
        public List<Piece> Pieces
        {
            get
            {
                return Board.Pieces;
            }
        }
        public Player CurrentPlayer { get; set; }
        public Move LastMove { get; set; }
        public void Start()
        {
            Console.CursorVisible = false;
            Board = new Board();
            //CurrentPlayer = new Player();
            CurrentPlayer = Board.PlayerX;
            DrawGrid();
            InitPieces();
            SelectorPosition = new Point(0, 0);
            //draw
            Pieces.ForEach(piece => DrawCell(piece.Position));
            RegisterPlayers();
            MoveSelector(new Point(0, 0));
            //MakeAKing();
            //PlayReverseEating();
            while (true)  // there are pieces on board
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.UpArrow)
                    MoveSelector(SelectorPosition.MoveBy(0, -1));
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                    MoveSelector(SelectorPosition.MoveBy(0, 1));
                else if (keyInfo.Key == ConsoleKey.RightArrow)
                    MoveSelector(SelectorPosition.MoveBy(1, 0));
                else if (keyInfo.Key == ConsoleKey.LeftArrow)
                    MoveSelector(SelectorPosition.MoveBy(-1, 0));
                else if (keyInfo.Key == ConsoleKey.Enter)
                    ToggleSelection();
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    if (LastMove != null && LastMove.IsEating)
                    {
                        LastMove = null;
                        ChangeTurn();
                    }
                    else
                    {
                        SelectedPiece = null;
                        DrawCell(SelectorPosition);
                    }
                }
            }
        }
        private void MakeAKing()
        {
            MakeMove(CreateMove(Board.Pieces[8], new Point(1, 3)));
            MakeMove(CreateMove(Board.Pieces[12], new Point(2, 4)));
            MakeMove(CreateMove(Board.Pieces[9], new Point(3, 3)));
            MakeMove(CreateMove(Board.Pieces[12], new Point(0, 2)));
            MakeMove(CreateMove(Board.Pieces[10], new Point(5, 3)));
            MakeMove(CreateMove(Board.Pieces[13], new Point(4, 4)));
            MakeMove(CreateMove(Board.Pieces[5], new Point(4, 2)));
            MakeMove(CreateMove(Board.Pieces[13], new Point(2, 2)));
            MakeMove(CreateMove(Board.Pieces[1], new Point(3, 1)));
            MakeMove(CreateMove(Board.Pieces[12], new Point(2, 0)));
        }
        public void RegisterPlayers()
        {
            Console.SetCursorPosition(0, 18);
            Console.WriteLine("WELCOME TO CHEKERS");
            Console.WriteLine("PLAYER X - please enter your name");
            Board.PlayerX.Name = "XXX"; // Console.ReadLine();
            Console.WriteLine("PLAYER O - please enter your name");
            Board.PlayerO.Name = "OOO";// Console.ReadLine();
            ///
            DeleteLines(5);
            Console.WriteLine("{0} - GO AHEAD!", Board.PlayerX.Name);
            CurrentPlayer = Board.PlayerX;
        }
        private void DeleteLines(int num)
        {
            Console.SetCursorPosition(0, 18);
            for (var i = 0; i < num; i++)
            {
                Console.Write(new string(' ', Console.WindowWidth));
            }
            Console.SetCursorPosition(0, 18);
        }
        private void ToggleSelection()
        {
            if (SelectedPiece == null)
            {
                // make sure a sign is left on selected piece
                //var Color = ConsoleColor.Yellow;
                //Draw(CurrentPlayer.Display , SelectorPosition, Color);
                var piece = GetPiece(SelectorPosition);
                if (piece == null)
                    return;
                if (piece.Player != CurrentPlayer)
                {
                    BlinkUserError();
                    return;
                }
                SelectedPiece = piece;
                DrawCell(SelectorPosition);
            }
            else
            {
                var piece = SelectedPiece;
                var move = CreateMove(piece, SelectorPosition);
                if (piece.Position.Equals(SelectorPosition))
                {
                    SelectedPiece = null;
                    DrawCell(SelectorPosition);
                }
                else if (!move.IsLegal)
                {
                    BlinkUserError();
                }
                else
                {
                    SelectedPiece = null;
                    MakeMove(move);
                    ChangeTurn();
                }
            }
        }

        private Move CreateMove(Piece piece, Point newPos)
        {
            var move = new Move { Piece = piece, From = piece.Position, To = newPos };
            move.IsEating = IsEating(move);
            move.IsEatingForward = IsEatingForward(move);
            move.IsEatingBackward = IsEatingBackward(move);
            move.IsTurningIntoKing = IsTurningIntoKing(move);
            move.IsLegal = IsLegal(move);
            return move;
        }

        bool CanMoveAgain()
        {
            if (LastMove == null)
                return false; //you still haven't moved even once!

            if (!LastMove.IsEatingForward)
                return false; //you can't move again without eating first
            var moves = GetAvailableMoves(LastMove.Piece);
            var canMoveAgain = moves.Where(t => t.IsEating).FirstOrDefault() != null;
            return canMoveAgain;
        }
        private void ChangeTurn()
        {
            if (CanMoveAgain())
                return;

            if (CurrentPlayer == Board.PlayerX)
                CurrentPlayer = Board.PlayerO;
            else
                CurrentPlayer = Board.PlayerX;

            DeleteLines(1);
            Console.WriteLine("{0} - your turn", CurrentPlayer.Name);
            LastMove = null;
        }
        private void BlinkUserError()
        {
            var display = string.Empty;
            var point = new Point(ConvertCursorToX(Console.CursorLeft), ConvertCursorToY(Console.CursorTop));
            var piece = GetPiece(point);
            if (piece == null)
                display = " ";
            else
                display = piece.Display;

            Draw(display, Convert(SelectorPosition), ConsoleColor.Red);
            Thread.Sleep(300);
            Draw(display, Convert(SelectorPosition), ConsoleColor.Green);
        }
        Point SelectorPosition;
        Piece SelectedPiece;
        public void MoveSelector(Point p)
        {
            if (p.X < 0 || p.X > 7)
                return;
            if (p.Y < 0 || p.Y > 7)
                return;
            var prevPos = SelectorPosition;
            var x = Convert(p);
            //Console.SetCursorPosition(x.X, x.Y);
            SelectorPosition = p;
            if (prevPos != null)
                DrawCell(prevPos);
            DrawCell(SelectorPosition);
        }


        public bool IsTurningIntoKing(Move move)
        {
            var piece = move.Piece;
            var newPos = move.To;
            if ((piece.Player.IsPlayerX && newPos.Y == 7) || (!piece.Player.IsPlayerX && newPos.Y == 0))
                return true;
            return false;

        }
        public void MakeMove(Move move)
        {
            var piece = move.Piece;
            var newPos = move.To;
            // again is it necessary? move.islegal = true                    
            if (!move.IsLegal)
                throw new Exception("Illegal Move");
            if (move.IsTurningIntoKing)
                piece.IsKing = true;
            if (move.IsEatingForward || move.IsEatingBackward) // or: if(move.IsEating) 
            {
                var midPiece = GetMidPiece(piece, newPos);
                var midCell = midPiece.Position;
                midPiece.Position = Point.Empty;
                Draw(" ", Convert(midCell), ConsoleColor.Black);
            }
            var oldPos = piece.Position;
            piece.Position = newPos;
            DrawCell2(oldPos, piece.IsKing);
            DrawCell(piece.Position);

            LastMove = move;
        }
        public Piece GetMidPiece(Piece piece, Point newPos)
        {
            int x;
            int y;
            //if(piece.Display=="O") - not working
            if (piece.Position.X > newPos.X)
            {
                if (piece.Position.Y > newPos.Y)
                {
                    y = piece.Position.Y - 1;
                }
                else
                {
                    y = piece.Position.Y + 1;
                }
                x = piece.Position.X - 1;

            }
            else
            {
                if (piece.Position.Y > newPos.Y)
                {
                    y = piece.Position.Y - 1;
                }
                else
                {
                    y = piece.Position.Y + 1;
                }
                x = piece.Position.X + 1;

            }
            var point = new Point(x, y);
            return GetPiece(point);

        }

        bool IsCellOccupied(Point pos)
        {
            return GetPiece(pos) != null;
        }
        bool IsLegal(Move move)
        {
            var piece = move.Piece;
            var newPos = move.To;
            var px = piece.Position.X;
            var py = piece.Position.Y;
            if (newPos.X < 0 || newPos.Y < 0)
                return false;
            if (newPos.Y > 7 || newPos.X > 7)
                if (IsCellOccupied(newPos))
                    return false;
            if (move.IsEatingForward)
                return true;
            if (move.IsEatingBackward)
            {
                if (move.Piece.IsKing)
                    return true;
                if (LastMove != null && LastMove.IsEating)
                    return true;
                return false;
            }
            var legalY = piece.Player.YDirection;//.Display == "x" ? py + 1 : py - 1;
            // go forward left or forward right, or eat
            //
            if ((px - 1 == newPos.X || px + 1 == newPos.X) && (legalY == newPos.Y || piece.IsKing))
                return true;
            return false;
        }

        public List<Move> GetAvailableMoves(Piece piece)
        {
            int x = piece.Position.X;
            int y = piece.Position.Y;
            //var yDir = piece.Player.YDirection;


            var possiblePositions = new List<Point>
            {
                new Point(x+1, y+1),
                new Point(x+1, y-1),
                
                new Point(x-1, y+1),
                new Point(x-1, y-1),
                
                new Point(x+2, y+2),
                new Point(x+2, y-2),
                
                new Point(x-2, y+2),
                new Point(x-2, y-2),
            };
            var moves = new List<Move>();
            foreach (var p in possiblePositions)
            {
                var move = CreateMove(piece, p);
                if (move.IsLegal)
                    moves.Add(move);
            }
            return moves;
        }

        public bool IsEating(Move move)
        {
            var piece = move.Piece;
            var newPos = move.To;
            var px = piece.Position.X;
            var py = piece.Position.Y;
            var offsetY = piece.Player.YDirection * 2;

            if ((px - 2 == newPos.X || px + 2 == newPos.X) && (py + 2 == newPos.Y || py - 2 == newPos.Y))
            {
                var midPiece = GetMidPiece(piece, newPos);
                if (midPiece != null && midPiece.Player != piece.Player)
                    return true;
            }
            return false;
        }

        public bool IsEatingForward(Move move)
        {
            if (!move.IsEating)
                return false;
            if (move.Piece.Player.YDirection == 1)
                return move.From.Y < move.To.Y;
            return move.From.Y > move.To.Y;
        }

        bool IsEatingBackward(Move move)
        {
            return move.IsEating && !move.IsEatingForward;
        }
        public Piece GetPiece(Point pos)
        {
            foreach (var piece in Pieces)
            {
                if (piece.Position.Equals(pos))
                    return piece;
            }
            return null;
        }
        public void InitPieces()
        {
            for (var i = 0; i < 24; i++)
            {
                var piece = new Piece();
                if (i < 12)
                    piece.Player = Board.PlayerX;
                else
                    piece.Player = Board.PlayerO;
                Pieces.Add(piece);
            }
            SetPieces();
        }
        void SetPieces()
        {
            var pieceIndex = 0;
            var piece = Pieces[pieceIndex];

            var i = 0;
            for (var j = 0; j < 8; j++)
            {
                if (j == 3 || j == 4)
                    continue;
                if (j == 1 || j == 5 || j == 7)
                {
                    i = 1;
                }
                while (i < 8)
                {
                    piece.Position = new Point(i, j);
                    pieceIndex++;
                    if (pieceIndex < Pieces.Count)
                        piece = Pieces[pieceIndex];
                    i += 2;
                }
                i = 0;
            }
        }
        public void DrawGrid()
        {
            Console.WriteLine("    0   1   2   3   4   5   6   7  ");
            Console.WriteLine("  ┌───┬───┬───┬───┬───┬───┬───┬───┐");
            Console.WriteLine("0 │   │   │   │   │   │   │   │   │");
            Console.WriteLine("  ├───┼───┼───┼───┼───┼───┼───┼───┤");
            Console.WriteLine("1 │   │   │   │   │   │   │   │   │");
            Console.WriteLine("  ├───┼───┼───┼───┼───┼───┼───┼───┤");
            Console.WriteLine("2 │   │   │   │   │   │   │   │   │");
            Console.WriteLine("  ├───┼───┼───┼───┼───┼───┼───┼───┤");
            Console.WriteLine("3 │   │   │   │   │   │   │   │   │");
            Console.WriteLine("  ├───┼───┼───┼───┼───┼───┼───┼───┤");
            Console.WriteLine("4 │   │   │   │   │   │   │   │   │");
            Console.WriteLine("  ├───┼───┼───┼───┼───┼───┼───┼───┤");
            Console.WriteLine("5 │   │   │   │   │   │   │   │   │");
            Console.WriteLine("  ├───┼───┼───┼───┼───┼───┼───┼───┤");
            Console.WriteLine("6 │   │   │   │   │   │   │   │   │");
            Console.WriteLine("  ├───┼───┼───┼───┼───┼───┼───┼───┤");
            Console.WriteLine("7 │   │   │   │   │   │   │   │   │");
            Console.WriteLine("  └───┴───┴───┴───┴───┴───┴───┴───┘");
        }
        void Draw(string s, Point pos, ConsoleColor bgColor)
        {
            var prevColor = Console.BackgroundColor;
            Console.BackgroundColor = bgColor;
            Console.SetCursorPosition(pos.X, pos.Y);
            Console.Write(s);
            Console.BackgroundColor = prevColor;
        }
        Point Convert(Point p)
        {
            return new Point(ConvertX(p.X), ConvertY(p.Y));
        }
        int ConvertX(int x)
        {
            return ((x + 1) * 4);
        }
        int ConvertY(int y)
        {
            return ((y + 1) * 2);
        }
        int ConvertCursorToX(int x)
        {
            return (x - 2) / 4;
        }
        int ConvertCursorToY(int y)
        {
            return (y - 1) / 2;
        }
        public void DrawCell(Point pos)
        {
            var bgColor = ConsoleColor.Black;
            if (pos.Equals(SelectorPosition))
            {
                bgColor = ConsoleColor.DarkBlue;
                if (SelectedPiece != null)
                    bgColor = ConsoleColor.DarkGreen;
            }
            var piece = GetPiece(pos);
            if (piece != null)
            {
                var display = piece.IsKing ? piece.Player.KingDisplay : piece.Player.Display;
                Draw(display, Convert(piece.Position), bgColor);
            }
            else
            {
                Draw(" ", Convert(pos), bgColor);
            }


        }
        public void DrawCell2(Point pos, bool isKing)
        {
            var bgColor = ConsoleColor.Black;
            if (pos.Equals(SelectorPosition))
            {
                bgColor = ConsoleColor.DarkBlue;
                if (SelectedPiece != null)
                    bgColor = ConsoleColor.DarkGreen;
            }
            var piece = GetPiece(pos);
            if (piece != null)
                Draw(piece.Display, Convert(piece.Position), bgColor);
            else
                Draw(" ", Convert(pos), bgColor);
        }
    }

    class Move
    {
        public Piece Piece { get; set; }
        public Point From { get; set; }
        public Point To { get; set; }
        public bool IsEatingForward { get; set; }
        public bool IsEatingBackward { get; set; }
        public bool IsTurningIntoKing { get; set; }
        public bool IsLegal { get; set; }
        //public bool CanMoveAgain { get; set; }


        public bool IsEating { get; set; }
    }
}
