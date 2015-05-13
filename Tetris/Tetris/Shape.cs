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
            Layouts = new List<List<Point>>();
        }
        public bool Equals(Shape shape)
        {
            return shape.Layout == Layout && shape.Color==Color;
        }

        public List<Point> Layout
        {
            get
            {
                return Layouts.FirstOrDefault();
            }
        }
       public List<List<Point>> Layouts { get; set; }
       public ConsoleColor Color;


    }

}
