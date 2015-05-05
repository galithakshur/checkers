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
            PlayReverseEating();
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
                    if (CurrentPlayer.IsEating)
                    {
                        CurrentPlayer.IsEating = false;
                        ChangeTurn();
                    }
                    else
                    {
                        SelectedPiece = null;
                        DrawCell(SelectorPosition);
                    }
                }
            }
            //Console.ReadLine();
        }

        private void PlayReverseEating()
        {
            MovePiece(Board.Pieces[8], new Point(1, 3));
            CurrentPlayer = Board.PlayerO;
            MovePiece(Board.Pieces[15], new Point(6, 4));
            CurrentPlayer = Board.PlayerX;
            MovePiece(Board.Pieces[8], new Point(2, 4));
            CurrentPlayer = Board.PlayerO;
            MovePiece(Board.Pieces[9], new Point(3, 3));
            CurrentPlayer = Board.PlayerX;
            MovePiece(Board.Pieces[15], new Point(7, 3));
            CurrentPlayer = Board.PlayerO;
            MovePiece(Board.Pieces[9], new Point(4, 4));
            CurrentPlayer = Board.PlayerX;
            MovePiece(Board.Pieces[4], new Point(0, 2));
            CurrentPlayer = Board.PlayerO;
            MovePiece(Board.Pieces[14], new Point(6, 4));
            CurrentPlayer = Board.PlayerX;
            MovePiece(Board.Pieces[4], new Point(1, 3));
            CurrentPlayer = Board.PlayerO;
        }
        private void MakeAKing()
        {
            MovePiece(Board.Pieces[8], new Point(1, 3));
            MovePiece(Board.Pieces[12], new Point(2, 4));
            MovePiece(Board.Pieces[9], new Point(3, 3));
            MovePiece(Board.Pieces[12], new Point(0, 2));
            MovePiece(Board.Pieces[10], new Point(5, 3));
            MovePiece(Board.Pieces[13], new Point(4, 4));
            MovePiece(Board.Pieces[5], new Point(4, 2));
            MovePiece(Board.Pieces[13], new Point(2, 2));
            MovePiece(Board.Pieces[1], new Point(3, 1));
            MovePiece(Board.Pieces[12], new Point(2, 0));
        }
        public void RegisterPlayers()
        {
            Console.SetCursorPosition(0, 18);
            Console.WriteLine("WELCOME TO CHEKERS");
            Console.WriteLine("PLAYER X - please enter your name");
            Board.PlayerX.Name = Console.ReadLine();
            Console.WriteLine("PLAYER O - please enter your name");
            Board.PlayerO.Name = Console.ReadLine();
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
                SelectedPiece = GetPiece(SelectorPosition);
                DrawCell(SelectorPosition);
            }
            else
            {
                var piece = SelectedPiece;
                if (piece.Position.Equals(SelectorPosition))
                {
                    SelectedPiece = null;
                    DrawCell(SelectorPosition);
                }
                else if (CheckLegalMoveAndErrorIfNeeded(piece, SelectorPosition))
                {
                    SelectedPiece = null;
                    MovePiece(piece, SelectorPosition);
                    // change turn to other player (on Board/here.CurrentPlayer)?
                    ChangeTurn();
                }
                else
                {
                    //TODO: handle illegal move - make illegal cell red
                    Error();
                }
            }
        }
        /// 
        private void ChangeTurn()
        {
            //if (GetAvailableMoves())
            //    return;
            // Point selectorPos;
            if (!CurrentPlayer.IsEating)
            {
                if (CurrentPlayer == Board.PlayerX)// && !CurrentPlayer.IsEating)
                {
                    CurrentPlayer = Board.PlayerO;
                    // selectorPos = new Point(0, 5);
                }
                else
                {
                    CurrentPlayer = Board.PlayerX;
                    //selectorPos = new Point(0, 0);
                }

            }

            DeleteLines(1);
            Console.WriteLine("{0} - your turn", CurrentPlayer.Name);
            //MoveSelector(selectorPos);

        }
        private void Error()
        {
            var display = string.Empty;
            var point = new Point(ConvertCursorToX(Console.CursorLeft), ConvertCursorToY(Console.CursorTop));
            var piece = GetPiece(point);
            if (piece == null)
                display = " ";
            else
                display = piece.Display;
            Draw(display, Convert(SelectorPosition), ConsoleColor.Red);
            Thread.Sleep(500);
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
        public void Select()
        {


        }
        public void MovePiece(Piece piece, Point newPos)
        {
            if (!CheckLegalMoveAndErrorIfNeeded(piece, newPos))
                throw new Exception("Illegal Move");
            //ISKING
            if ((piece.Display == "X" && newPos.Y == 7) || (piece.Display == "O" && newPos.Y == 0))
            {
                piece.IsKing = true;
                // change display
                //piece.Display = "K";
                piece.Display += piece.Display;
            }
            //Eating..
            if (IsEatingPossible(piece, newPos))
            {
                var midPiece = GetMidPiece(piece, newPos);
                var midCell = midPiece.Position;
                //mayb beter to do : piece.IsDisabled = true;  --> 
                midPiece.Position = Point.Empty;
                //
                var display = midPiece.IsKing ? "  " : " ";
                Draw(display, Convert(midCell), ConsoleColor.Black);
            }
            var oldPos = piece.Position;
            piece.Position = newPos;
            // DrawCell(oldPos);
            DrawCell2(oldPos, piece.IsKing);
            DrawCell(piece.Position);
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
        public bool CheckLegalMoveAndErrorIfNeeded(Piece piece, Point newPos)
        {
            var legal = IsMoveLegal(piece, newPos);
            if (!legal)
                Error();
            return legal;
        }

        bool IsMoveLegal(Piece piece, Point newPos)
        {
            var px = piece.Position.X;
            var py = piece.Position.Y;
            if (IsCellOccupied(newPos))
                return false;
            if (IsEatingPossible(piece, newPos))
                return true;
            var legalY = piece.Display == "X" ? py + 1 : py - 1;
            // go forward left or forward right, or eat
            //
            if ((px - 1 == newPos.X || px + 1 == newPos.X) && (legalY == newPos.Y || piece.IsKing))
                return true;


            return false;

            //var availableMoves = GetAvailableMoves();
            //if (availableMoves.Contains(newPos))
            //    return true;
            //return false;
        }

        //public List<Point> GetAvailableMoves()
        //{
        //    var piece = GetPiece(SelectorPosition);
        //    int x = piece.Position.X;
        //    int y = piece.Position.Y;
        //    var legalY = piece.Display == "X" ? 1 : -1;

        //    var possiblePositions = new List<Point>{
        //         new Point(x+1,y+legalY),
        //        new Point(x-1,y+legalY),

        //        //new Point(x-2,y-2),
        //        //new Point(x-1,y-2),
        //        //new Point(x-2,y-2),
        //        //new Point(x-2,y-2),
        //    };
        //    var AvailablePositions = new List<Point>();
        //    foreach (var p in possiblePositions)
        //    {
        //        if (!IsCellOccupied(p))
        //            AvailablePositions.Add(p);
        //    }
        //    return AvailablePositions;
        //}

        /// maybe this method should be on board

        public bool IsEatingPossible(Piece piece, Point newPos)  // get pieces to global
        {
            var px = piece.Position.X;
            var py = piece.Position.Y;
            var offset = piece.Display == "X" ? 2 : -2;

            if ((px - 2 == newPos.X || px + 2 == newPos.X) && (py + offset == newPos.Y || piece.IsKing))
            {
                var midPiece = GetMidPiece(piece, newPos);
                if (midPiece != null && midPiece.Display != piece.Display)
                    CurrentPlayer.IsEating = true;
                return true;
            }
            //eating backwards
            if (CurrentPlayer.IsEating && (px - 2 == newPos.X || px + 2 == newPos.X) && (py - 2 == newPos.Y || py + 2 == newPos.X))
                return true;
            return false;
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
                    piece.Display = "X";
                else
                    piece.Display = "O";
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
                Draw(piece.Display, Convert(piece.Position), bgColor);
            else
                Draw(" ", Convert(pos), bgColor);

        }
        public void DrawCell2(Point pos, bool isKing)
        {
            var bgColor = ConsoleColor.Black;
            if (pos.Equals(SelectorPosition))
            {
                bgColor = ConsoleColor.DarkBlue;
                if (SelectedPiece != null)
                    bgColor = ConsoleColor.DarkGreen;
                ///
            }
            var piece = GetPiece(pos);
            //
            var display = isKing ? "  " : " ";
            ///
            //if(piece==SelectedPiece)
            //    bgColor = ConsoleColor.Yellow;
            if (piece != null)
                Draw(piece.Display, Convert(piece.Position), bgColor);
            else
                //
                Draw(display, Convert(pos), bgColor);

        }

    }
}
