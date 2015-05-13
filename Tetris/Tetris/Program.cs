using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Tetris
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            var tetris = new TetrisGame();
            tetris.CreateBoard();
            tetris.DrawBoard();
            TestShapes(tetris);

            var shapes = new Shapes();
            var shape = shapes.AllShapes[0];
            shape.Color = ConsoleColor.Magenta;
            tetris.AddShape(shape);
            tetris.DrawBoard();
            tetris.CurrentShape = shape;
            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.UpArrow)
                    tetris.RotateCurrentShape();
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                    tetris.MoveShape(0, 1);
                else if (keyInfo.Key == ConsoleKey.RightArrow)
                    tetris.MoveShape(1, 0);
                else if (keyInfo.Key == ConsoleKey.LeftArrow)
                    tetris.MoveShape(-1, 0);
            }
        }

        private static void TestShapes(TetrisGame tetris)
        {
            foreach (var sh in tetris.Shapes.AllShapes)
            {
                tetris.DrawShape(sh);
                Thread.Sleep(500);
                Console.Clear();

            }
        }


    }
}
