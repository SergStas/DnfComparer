using System;
using System.Collections.Generic;
using System.Text;
using DNFComparer;

namespace DNFComparer
{
    class Edge
    {
        public Dot[,] Dots;
        public ExtraData.Coordinates[] CommonCoordinates = new ExtraData.Coordinates[4];
        public List<Dot> DotList;

        public Edge(Dot[,] dots)
        {
            Dots = dots;
            CommonCoordinates = GetCommonCoordinates(dots);
            DotList = GetDotList();
        }

        public int SizeX
        {
            get
            {
                return Dots.GetLength(1);
            }
        }

        public int SizeY
        {
            get
            {
                return Dots.GetLength(0);
            }
        }

        public int DotsCount
        {
            get
            {
                return Dots.Length;
            }
        }

        public void UpdateFields()
        {
            CommonCoordinates = GetCommonCoordinates(Dots);
            DotList = GetDotList();
        }

        private static ExtraData.Coordinates[] GetCommonCoordinates(Dot[,] dots)
        {
            foreach (Dot dot in dots)
                if (dot == null)
                    return new ExtraData.Coordinates[] { ExtraData.Coordinates.FALSE, ExtraData.Coordinates.FALSE, ExtraData.Coordinates.FALSE, ExtraData.Coordinates.FALSE };
            ExtraData.Coordinates[] result = new ExtraData.Coordinates[4];
            for (int i = 0; i < 4; i++)
                result[i] = dots[0, 0].Coordinates[i];
            foreach (Dot dot in dots)
                for (int i = 0; i < 4; i++)
                    if (result[i] != dot.Coordinates[i])
                        result[i] = ExtraData.Coordinates.FALSE;
            return result;
        }

        public List<Dot> GetDotList()
        {
            List<Dot> result = new List<Dot>();
            foreach (Dot dot in Dots)
                result.Add(dot);
            return result;
        }

        public override string ToString()
        {
            return GetCommonDotsString();
        }

        public string ToMatrix()
        {
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < SizeY; i++)
            {
                for (int j = 0; j < SizeX; j++)
                    str.Append((Dots[i, j].Value ? "1" : "0") + "\t");
                str.Append("\n");
            }
            return str.ToString();
        }

        public string GetCommonDotsString()
        {
            StringBuilder result = new StringBuilder();
            List<string> vars = new List<string>();
            for (int i = 0; i < 4; i++)
                if (CommonCoordinates[i] != ExtraData.Coordinates.FALSE)
                    vars.Add("(" + (CommonCoordinates[i] == ExtraData.Coordinates.NOT ? "-" : "") + ExtraData.variables[i] + ")");
            result.Append(string.Join('*', vars));
            return result.ToString();
        }
    }
}
