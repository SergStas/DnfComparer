using System;
using System.Collections.Generic;
using System.Text;

namespace SmartDNFComparer
{
    static class InputData
    {
        public static void GetCoords(string str, int i, ExtraData.Coordinates[] coords, int varsCount)
        {
            if (str.Length == 1) return;
            string sub;
            if (i < str.Length / 2)
            {
                coords[varsCount - (int)Math.Log(str.Length, 2)] = ExtraData.Coordinates.NOT;
                sub = str.Substring(0, str.Length / 2);
            }
            else
            {
                coords[varsCount - (int)Math.Log(str.Length, 2)] = ExtraData.Coordinates.IS;
                sub = str.Substring(str.Length / 2);
                i -= str.Length / 2;
            }
            GetCoords(sub, i, coords, varsCount);
        }

        public static Function KeyboardInput()
        {
            Console.Write("Введите функцию в векторном виде: ");
            string f = Console.ReadLine();
            f = f.Replace(" ", "");
            f = f.Replace(".", "");
            f = f.Replace("\n", "");
            f = f.Replace("\t", "");
            double log = Math.Log2(f.Length);
            if (log - Math.Floor(log) != 0)
                throw ExtraData.MustHave;
            return GenerateFunctionFromString(f);
        }

        public static Function GenerateFunctionFromString(string str)
        {
            int varsCount = (int)Math.Log2(str.Length);
            ExtraData.Coordinates[] coords = new ExtraData.Coordinates[varsCount];
            Dot[] dots = new Dot[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                bool value = str[i] == '1' ? true : false;
                GetCoords(str, i, coords, varsCount);
                Dot dot = new Dot(value, ExtraData.CopyCoordsArray(coords));
                dots[i] = dot;
            }
            return new Function(dots);
        }
    }
}
