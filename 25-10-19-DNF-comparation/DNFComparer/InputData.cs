using System;
using System.Collections.Generic;
using System.Text;

namespace DNFComparer
{
    class InputData
    {
        public static void GetCoord(string str, int i, ExtraData.Coordinates[] coords)
        {

            if (str.Length == 1) return;
            string sub;
            if (i < str.Length / 2)
            {
                coords[4 - (int)Math.Log(str.Length, 2)] = ExtraData.Coordinates.NOT;
                sub = str.Substring(0, str.Length / 2);
            }
            else
            {
                coords[4 - (int)Math.Log(str.Length, 2)] = ExtraData.Coordinates.IS;
                sub = str.Substring(str.Length / 2);
                i -= str.Length / 2;
            }
            GetCoord(sub, i, coords);
        }

        public static Function KeyboardInput()
        {
            Console.Write("Введите функцию в векторном виде: ");
            string f = Console.ReadLine();
            ExtraData.Coordinates[] coords = new ExtraData.Coordinates[4];
            Dot[,] dots = new Dot[4, 4];
            //if (f.Length != 16)
            //    Console.WriteLine("ты ебанутый?");
            //else
            //{
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    {
                        bool value = f[4 * i + j] == '1' ? true : false;
                        GetCoord(f, 4 * i + j, coords);
                        Dot dot = new Dot(value, ExtraData.CopyCoordsArray(coords));
                        dots[i, j] = dot;
                    }
            //}
            return new Function(Function.SwitchCoordinateMatrix(dots));
        }
    }
}
