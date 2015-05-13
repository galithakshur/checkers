//var ch = (char)i;
//foreach(var piece in pieces)
//{
//
//Console.WriteLine("{0}:{1}",i,ch);
//Console.OutputEncoding = Encoding.UTF8;
//Console.WriteLine("┬");
//  SetCell(0, 0, "0");
//  SetCell(1, 1, "1");
//  SetCell(2, 2, "2");
//  SetCell(3, 3, "3");
//static void foo()
//{
//    //var s =Console.WriteLine(ch+ " "+(char)ch);
//
//    //var s = Encoding.ASCII.GetString(new byte[] { 192 });
//    //Console.OutputEncoding = Encoding.Unicode;
//    //Console.WriteLine(Console.OutputEncoding.EncodingName);
//    //return;
//    Console.Write((char)9472);
//    Console.Write((char)9474);
//    for (var i = 0; i < 100; i++)
//    {
//        var ch = 9400 + i;
//        //Console.WriteLine(ch+ " "+(char)ch);
//        Console.Write((char)ch);
//    }
//    //Console.WriteLine((char)193);
//    //Console.WriteLine((int)"┬"[0]);
//
//}
//public static void PlacePieces()
//{
//    char[] grid = new char[8];
//    for (var i = 0; i < grid.Length;i++)
//    {
//        grid[i] = new char[8];
//    }
//   for(var i =0;i<grid.Length;i++)
//   {
//       for(var j=0;j<grid[i].Length;j++)
//       {

//       }
//   }
//}
//        public static void DrawGrid(int x, int y)
//        {
//            for (var i = 0; i < x * 2; i++)
//            {
//                for (var j = 0; j < y; j++)
//                {
//                    if (i % 2 == 0)
//                        Console.Write("___");
//                    else
//                        if (j == y - 1)
//                            Console.Write("|  |");
//                        else
//                            Console.Write("|  ");
//                }
//                Console.WriteLine();
//                if (i == x - 1)
//                    Console.WriteLine("________________________");
//            }
//        }
//public override bool Equals(object obj)
//{
//    return Equals(obj as Position);
//}

//public override int GetHashCode()
//{
//    return X.GetHashCode() ^ Y.GetHashCode();
//}


//public static bool operator ==(Position p1, Position p2)
//{
//    if (p1 == null && p2 != null)
//        return false;
//    if (p1 != null && p2 == null)
//        return false;
//    if (p1 == null && p2 == null)
//        return true;
//    return p1.Equals(p2);
//}
//public static bool operator !=(Position p1, Position p2)
//{
//    return !p1.Equals(p2);
//}
/// <summary>
/// ─ │ ┌ └ ┐ ┘ ├ ┤ ┬ ┴ ┼
/// 0 1 2 3 4 5 6 7 8 9 10
/// </summary>
//static string Chars = "─│┌└┐┘├┤┬┴┼";
//public List<Piece> Pieces { get; set; }
//

//if (piece.Display == "X")
//     piece.Display = "XX";
// else
//     piece.Display = "OO";

