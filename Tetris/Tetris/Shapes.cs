using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class Shapes
    {
        public List<Shape> AllShapes;
        public Shapes()
        {
            AllShapes = CreateShapes();
        }
        // need to change param to string [][][]/AllShapes? and add a loop
        List<Point> CreateLayout(string[] def)
        {
            var layout = new List<Point>();
            var y = 0;
            foreach (var line in def)
            {
                var x = 0;
                foreach (var ch in line)
                {
                    if (ch != ' ')
                        layout.Add(new Point(x, y));
                    x++;
                }
                y++;
            }
            return layout;
        }

        List<Shape> CreateShapes()
        {
            var shapes = new string[][][]   //shape->layout->line
            {
                new string[][]
                {
                    new string[]
                    {
                        "xxx ",
                        "x   ",
                        "    ",
                        "    ",
                    },
                    new string[]
                    {
                        "xx  ",
                        " x  ",
                        " x  ",
                        "    ",
                    },
                    new string[]
                    {
                        "  x ",
                        "xxx ",
                        "    ",
                        "    ",
                    },
                    new string[]
                    {
                        "x   ",
                        "x   ",
                        "xx  ",
                        "    ",
                    },
                },
                new string[][]
                {
                    new string[]
                    {
                        "xxx ",
                        "  x ",
                        "    ",
                        "    ",
                    },
                    new string[]
                    {
                        " x  ",
                        " x  ",
                        "xx  ",
                        "    ",
                    },
                    new string[]
                    {
                        "x   ",
                        "xxx ",
                        "    ",
                        "    ",
                    },
                    new string[]
                    {
                        "xx  ",
                        "x   ",
                        "x   ",
                        "    ",
                    },
                },
                new string[][]
                {
                    new string[]
                    {
                        "xxx ",
                        " x  ",
                        "    ",
                        "    ",
                    },
                    new string[]
                    {
                        " x  ",
                        "xx  ",
                        " x  ",
                        "    ",
                    },
                    new string[]
                    {
                        " x  ",
                        "xxx ",
                        "    ",
                        "    ",
                    },
                    new string[]
                    {
                        "x   ",
                        "xx  ",
                        "x   ",
                        "    ",
                    },
                },
                 new string[][]
                {
                    new string[]
                    {
                        "x   ",
                        "xx  ",
                        " x  ",
                        "    ",
                    },
                    new string[]
                    {
                        " xx ",
                        "xx  ",
                        "    ",
                        "    ",
                    },
                },
               new string[][]
                {
                    new string[]
                    {
                        " x  ",
                        "xx  ",
                        "x   ",
                        "    ",
                    },
                    new string[]
                    {
                        "xx  ",
                        " xx ",
                        "    ",
                        "    ",
                    },
                },
                new string[][]
                {
                    new string[]
                    {
                        "xx  ",
                        "xx  ",
                        "    ",
                        "    ",
                    },
                    new string[]
                    {
                        "xx  ",
                        "xx  ",
                        "    ",
                        "    ",
                    },
                },
                new string[][]
                {
                    new string[]
                    {
                        "x   ",
                        "x   ",
                        "x   ",
                        "x   ",
                    },
                    new string[]
                    {
                        "xxxx",
                        "    ",
                        "    ",
                        "    ",
                    },
                },
            };
            return shapes.Select(shapeDef => new Shape { Layouts = shapeDef.Select(CreateLayout).ToList(),Color = ConsoleColor.Blue }).ToList();
        }
    }

}
//    return new List<Shape>
//    {
//        //  x
//        // xxx
//        new Shape
//        {
//            Layout = new List<Point>
//            {
//                new Point(1, 0),
//                new Point(0, 1),
//                new Point(1, 1),
//                new Point(2, 1),
//            },
//            Color = ConsoleColor.Yellow
//        },
//        new Shape
//        {
//            Layout = new List<Point>
//            {
//                new Point(0, 0),
//                new Point(1, 0),
//                new Point(2, 0),
//                new Point(1, 1),
//            },
//            Color = ConsoleColor.Yellow
//        },
//        new Shape
//        {
//            Layout = new List<Point>
//            {
//                new Point(0, 0),
//                new Point(0, 3),
//                new Point(1, 1),
//                new Point(0, 1),
//            },
//            Color = ConsoleColor.Yellow
//        },
//        new Shape
//        {
//            Layout = new List<Point>
//            {
//                new Point(2, 1),
//                new Point(0, 2),
//                new Point(1, 1),
//                new Point(1, 0),
//            },
//            Color = ConsoleColor.Yellow
//        // *  
//        //  * *
//        //
//        },


//    };
//}
//    rotation3.Layout.Add(new Point(x + 1, y + 1));
//    index++;
//    rotation3.Layout.Add(new Point(x - 1, y + 1));
//    index++;
//    rotation3.Layout.Add(new Point(x, y));
//    index++;
//    rotation3.Layout.Add(new Point(x - 1, y - 1));
//    rotation3.Color = shape.Color;
//    shape.Rotations.Add(rotation1); //3
//    shape.Rotations.Add(rotation2);
//    shape.Rotations.Add(rotation3);
//    AllShapes.Add(shape);
//}
// ****
//public void CreateShape2()
//{
//    var shape = new Shape();
//    shape.Layout.Add(new Point(8, 0));
//    shape.Layout.Add(new Point(9, 0));
//    shape.Layout.Add(new Point(10, 0));
//    shape.Layout.Add(new Point(11, 0));
//    shape.Color = ConsoleColor.Red;

//    var rotation1 = new Shape();
//    var index = 0;
//    var x = shape.Layout[index].X;
//    var y = shape.Layout[index].Y;
//    rotation1.Layout.Add(new Point(x, y - 1));
//    index++;
//    rotation1.Layout.Add(new Point(x + 1, y));
//    index++;
//    rotation1.Layout.Add(new Point(x + 2, y - 1));
//    index++;
//    rotation1.Layout.Add(new Point(x + 3, y - 2));
//    rotation1.Color = shape.Color;

//    shape.Layouts.Add(rotation1);
//    AllShapes.Add(shape);
//}
////**
////**
//public void CreateShape3()
//{
//    var shape = new Shape();
//    shape.Layout.Add(new Point(9, 0));
//    shape.Layout.Add(new Point(10, 0));
//    shape.Layout.Add(new Point(9, 1));
//    shape.Layout.Add(new Point(10, 1));
//    shape.Color = ConsoleColor.Magenta;

//    var rotation1 = new Shape();
//    var index = 0;
//    var x = shape.Layout[index].X;
//    var y = shape.Layout[index].Y;
//    rotation1.Layout.Add(new Point(x, y));
//    index++;
//    rotation1.Layout.Add(new Point(x, y));
//    index++;
//    rotation1.Layout.Add(new Point(x, y));
//    index++;
//    rotation1.Layout.Add(new Point(x, y));
//    rotation1.Color = shape.Color;

//    shape.Layouts.Add(rotation1);
//    AllShapes.Add(shape);
//}
//// **
//// *
//// *
//public void CreateShape4()
//{
//    var shape = new Shape();
//    shape.Layout.Add(new Point(10, 0));
//    shape.Layout.Add(new Point(9, 0));
//    shape.Layout.Add(new Point(9, 1));
//    shape.Layout.Add(new Point(9, 2));
//    shape.Color = ConsoleColor.Blue;

//    var rotation1 = new Shape();
//    var index = 0;
//    var x = shape.Layout[index].X;
//    var y = shape.Layout[index].Y;
//    rotation1.Layout.Add(new Point(x + 1, y + 2));
//    index++;
//    rotation1.Layout.Add(new Point(x + 2, y + 1));
//    index++;
//    rotation1.Layout.Add(new Point(x - 1, y));
//    index++;
//    rotation1.Layout.Add(new Point(x, y - 1));
//    rotation1.Color = shape.Color;

//    var rotation2 = new Shape();
//    index = 0;
//    x = shape.Layout[index].X;
//    y = shape.Layout[index].Y;
//    rotation2.Layout.Add(new Point(x - 1, y + 2));
//    index++;
//    rotation2.Layout.Add(new Point(x + 1, y + 2));
//    index++;
//    rotation2.Layout.Add(new Point(x + 1, y));
//    index++;
//    rotation2.Layout.Add(new Point(x + 1, y - 2));
//    rotation2.Color = shape.Color;

//    var rotation3 = new Shape();
//    index = 0;
//    x = shape.Layout[index].X;
//    y = shape.Layout[index].Y;
//    rotation3.Layout.Add(new Point(x - 1, y));
//    index++;
//    rotation3.Layout.Add(new Point(x, y + 1));
//    index++;
//    rotation3.Layout.Add(new Point(x + 1, y));
//    index++;
//    rotation3.Layout.Add(new Point(x + 2, y - 1));
//    rotation3.Color = shape.Color;
//    shape.Layouts.Add(rotation1);
//    shape.Layouts.Add(rotation2);
//    shape.Layouts.Add(rotation3);
//    AllShapes.Add(shape);
//}
//// **
////  *
////  *
//public void CreateShape5()
//{
//    var shape = new Shape();
//    shape.Layout.Add(new Point(10, 0));
//    shape.Layout.Add(new Point(11, 0));
//    shape.Layout.Add(new Point(11, 1));
//    shape.Layout.Add(new Point(11, 2));
//    shape.Color = ConsoleColor.Gray;

//    var rotation1 = new Shape();
//    var index = 0;
//    var x = shape.Layout[index].X;
//    var y = shape.Layout[index].Y;
//    rotation1.Layout.Add(new Point(x + 2, y));
//    index++;
//    rotation1.Layout.Add(new Point(x + 1, y + 1));
//    index++;
//    rotation1.Layout.Add(new Point(x, y));
//    index++;
//    rotation1.Layout.Add(new Point(x - 1, y - 1));
//    rotation1.Color = shape.Color;

//    var rotation2 = new Shape();
//    index = 0;
//    x = shape.Layout[index].X;
//    y = shape.Layout[index].Y;
//    rotation2.Layout.Add(new Point(x + 1, y + 2));
//    index++;
//    rotation2.Layout.Add(new Point(x - 1, y + 2));
//    index++;
//    rotation2.Layout.Add(new Point(x - 1, y));
//    index++;
//    rotation2.Layout.Add(new Point(x - 1, y - 2));
//    rotation2.Color = shape.Color;

//    var rotation3 = new Shape();
//    index = 0;
//    x = shape.Layout[index].X;
//    y = shape.Layout[index].Y;
//    rotation3.Layout.Add(new Point(x, y + 2));
//    index++;
//    rotation3.Layout.Add(new Point(x - 1, y + 1));
//    index++;
//    rotation3.Layout.Add(new Point(x, y));
//    index++;
//    rotation3.Layout.Add(new Point(x + 1, y - 1));
//    rotation3.Color = shape.Color;
//    shape.Layouts.Add(rotation1);
//    shape.Layouts.Add(rotation2);
//    shape.Layouts.Add(rotation3);
//    AllShapes.Add(shape);
//}
////   *
//// * *
//// *
//public void CreateShape6()
//{
//    var shape = new Shape();
//    shape.Layout.Add(new Point(11, 0));
//    shape.Layout.Add(new Point(11, 1));
//    shape.Layout.Add(new Point(10, 1));
//    shape.Layout.Add(new Point(10, 2));
//    shape.Color = ConsoleColor.White;

//    var rotation1 = new Shape();
//    var index = 0;
//    var x = shape.Layout[index].X;
//    var y = shape.Layout[index].Y;
//    rotation1.Layout.Add(new Point(x + 1, y + 2));
//    index++;
//    rotation1.Layout.Add(new Point(x, y + 1));
//    index++;
//    rotation1.Layout.Add(new Point(x + 1, y));
//    index++;
//    rotation1.Layout.Add(new Point(x, y - 1));
//    rotation1.Color = shape.Color;

//    shape.Layouts.Add(rotation1);
//    AllShapes.Add(shape);
//}
//// *
//// * *
////   *
//public void CreateShape7()
//{
//    var shape = new Shape();
//    shape.Layout.Add(new Point(9, 0));
//    shape.Layout.Add(new Point(9, 1));
//    shape.Layout.Add(new Point(10, 1));
//    shape.Layout.Add(new Point(10, 2));
//    shape.Color = ConsoleColor.White;

//    var rotation1 = new Shape();
//    var index = 0;
//    var x = shape.Layout[index].X;
//    var y = shape.Layout[index].Y;
//    rotation1.Layout.Add(new Point(x + 2, y + 1));
//    index++;
//    rotation1.Layout.Add(new Point(x + 1, y));
//    index++;
//    rotation1.Layout.Add(new Point(x, y + 1));
//    index++;
//    rotation1.Layout.Add(new Point(x - 1, y));
//    rotation1.Color = shape.Color;

//    shape.Layouts.Add(rotation1);
//    AllShapes.Add(shape);
//}
