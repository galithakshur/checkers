using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class Shape
    {
        public Shape()
        {
            Layouts = new List<ShapeLayout>();
        }

       public List<ShapeLayout> Layouts { get; set; }
       public ConsoleColor Color;


    }

    class ShapeLayout
    {
        public Shape Shape { get; set; }
        public List<Point> Layout { get; set; }

    }

}
