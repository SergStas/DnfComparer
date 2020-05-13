using System;
using System.Collections.Generic;
using System.Text;

namespace SmartDNFComparer
{
    static class KarnaughMapCreator
    {
        public static string GetKarnaughMapConsoleOutputString(Function function)
        {
            StringBuilder result = new StringBuilder();
            int[][] varsArrs = SplitVars(function);
            string[] vertGrayCode = GetGrayCode(varsArrs[0].Length, false);
            string[] horGrayCode = GetGrayCode(varsArrs[1].Length, function.VarsCount == 1);
            Dot[,] functionMatrix = GetFunctionMatrix(function);
            int fMX = functionMatrix.GetLength(1);
            int fMY = functionMatrix.GetLength(0);
            for (int i = 0; i < horGrayCode[0].Length; i++)
            {
                for (int j = 0; j < vertGrayCode[0].Length; j++)
                    result.Append(" ");
                result.Append(horGrayCode.Length == 1 ? '1' : ExtraData.variables[i + vertGrayCode[0].Length]);
                result.Append('┃');
                for (int j = 0; j < horGrayCode.Length; j++)
                    result.Append((j == 0 ? "" : " ") + horGrayCode[j][i]);
                result.Append('\n');
            }
            for (int i = 0; i < vertGrayCode[0].Length; i++)
                result.Append(ExtraData.variables[i]);
            result.Append(" ┃");
            for (int i = 0; i < horGrayCode.Length; i++)
                result.Append((i == 0 ? "" : " ") + " ");
            result.Append('\n');
            for (int i = 0; i <= vertGrayCode[0].Length; i++)
                result.Append('━');
            result.Append('╋');
            for (int i = 0; i < horGrayCode.Length; i++)
                result.Append((i == 0 ? "" : "━") + "━");
            result.Append('\n');
            for (int i = 0; i < vertGrayCode.Length * 2 - 1; i++)
            {
                if (i%2==1)
                    for (int j = 0; j <= vertGrayCode[0].Length; j++)
                        result.Append(' ');
                else
                    result.Append(vertGrayCode[(int)Math.Floor(i / 2.0)] + ' ');
                result.Append('┃');
                if (i % 2 == 1)
                    for (int j = 0; j < horGrayCode.Length; j++)
                        result.Append((j == 0 ? "" : " ") + " ");
                else
                    for (int j = 0; j < fMX; j++)
                        result.Append(functionMatrix[(int)Math.Floor(i / 2.0), j].CharValue() + " ");
                result.Append('\n');
            }
            return result.ToString();
        }
        
        public static string GetFunctionMatrixConsoleOutputString(Function function)
        {
            StringBuilder builder = new StringBuilder();
            Dot[,] functionMatrix = GetFunctionMatrix(function);
            int x = functionMatrix.GetLength(1);
            int y = functionMatrix.GetLength(0);
            for (int i = 0; i < y; i++) 
            {
                for (int j = 0; j < x; j++)
                    builder.Append(functionMatrix[i, j].CharValue() + " ");
                builder.Append("\n");
            }
            return builder.ToString();
        }

        public static Dot[,] GetFunctionMatrix(Function function)
        {
            ExtraData.Coordinates[,][] coordsMatrix = GetCoordsMatrix(function);
            int x = coordsMatrix.GetLength(1);
            int y = coordsMatrix.GetLength(0);
            Dot[,] result = new Dot[y, x];
            List<Dot> values = new List<Dot>();
            foreach (Dot currentDot in function.Values)
                values.Add(currentDot);
            for (int i = 0; i < y; i++)
                for (int j = 0; j < x; j++)
                    result[i, j] = ExtraData.GetDotWithSameCoordinates(coordsMatrix[i, j], values);
            return result;
        }

        public static ExtraData.Coordinates[,][] GetCoordsMatrix(Function function)
        {
            if (function.VarsCount == 1)
                return new ExtraData.Coordinates[2, 1][]
                {
                    { new[] { ExtraData.Coordinates.NOT } },
                    { new[] { ExtraData.Coordinates.IS } }
                };
            int[][] varsArrs = SplitVars(function);
            string[] vertGrayCode = GetGrayCode(varsArrs[0].Length, false);
            string[] horGrayCode = GetGrayCode(varsArrs[1].Length, false);
            ExtraData.Coordinates[,][] result = new ExtraData.Coordinates[vertGrayCode.Length,horGrayCode.Length][];
            for (int i = 0; i < vertGrayCode.Length; i++)
                for (int j = 0; j < horGrayCode.Length; j++)
                {
                    result[i, j] = new ExtraData.Coordinates[function.VarsCount];
                    for (int k = 0; k < vertGrayCode[i].Length; k++)
                        result[i, j][k] = vertGrayCode[i][k] == '0' ? ExtraData.Coordinates.NOT : ExtraData.Coordinates.IS;
                    for (int k = vertGrayCode[i].Length; k < function.VarsCount; k++)
                        result[i, j][k] = horGrayCode[j][k - vertGrayCode[i].Length] == '0' ? ExtraData.Coordinates.NOT : ExtraData.Coordinates.IS;
                }
            return result;
        }

        public static string[] GetGrayCode(int varsCount, bool isConst)
        {
            if (isConst)
                return new string[] { "1" };
            if (varsCount == 1)
                return new string[] { "0", "1" };
            string[] subArr = GetGrayCode(varsCount - 1, false);
            int length = subArr.Length;
            string[] result = new string[length * 2];
            Array.Copy(subArr, result, length);
            for (int i = length; i < length * 2; i++)
                result[i] = subArr[2 * length - i - 1];
            for (int i = 0; i < length * 2; i++)
                result[i] = i < length ? "0" + result[i] : "1" + result[i];
            return result;
        }

        public static int[][] SplitVars(Function function)
        {
            int varsCount = function.VarsCount;
            int vert = (int)Math.Ceiling(varsCount / 2.0);
            int hor = varsCount - vert;
            int[] vertArr = new int[vert];
            for (int i = 0; i < vert; i++)
                vertArr[i] = i;
            int[] horArr = new int[hor];
            if (hor == 0)
                horArr = new int[] { 0 };
            else
                for (int i = vert; i < varsCount; i++)
                    horArr[i - vert] = i;
            return new int[][] { vertArr, horArr };
        }
    }
}
