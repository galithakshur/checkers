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
            tetris.Start();
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
