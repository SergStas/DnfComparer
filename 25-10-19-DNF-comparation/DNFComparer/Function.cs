using System;
using System.Collections.Generic;
using System.Text;

namespace DNFComparer
{
    class Function
    {
        public Dot[,] Values;
        public List<Edge> MaxEdgesList;
        public List<Dot> NuclearDotsList;
        public List<Edge> Nuke;

        public Function(Dot[,] dots)
        {
            Values = dots;
            MaxEdgesList = MaxEdgeCreator.GetMaxEdges(Values);
            NuclearDotsList = GetNuclearDots();
            Nuke = GetNuke();
        }

        public int ValuesCount
        {
            get
            {
                return Values.Length;
            }
        }


        public int SizeX
        {
            get
            {
                return Values.GetLength(1);
            }
        }

        public int SizeY
        {
            get
            {
                return Values.GetLength(0);
            }
        }

        public static Dot[,] SwitchCoordinateMatrix(Dot[,] original)
        {
            int sizeX = original.GetLength(1);
            int sizeY = original.GetLength(1);
            Dot[,] switched = new Dot[sizeX, sizeY];
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    int indexX = i;
                    int indexY = j;
                    indexX = SwitchCoordinates(i, indexX);
                    indexY = SwitchCoordinates(j, indexY);
                    switched[indexX, indexY] = original[i, j];
                }
            }
            return switched;
        }

        public override string ToString()
        {
            Dot[,] switched = SwitchCoordinateMatrix(Values);
            StringBuilder str = new StringBuilder();
            foreach (Dot dot in switched)
                str.Append(dot.CharValue());
            return "(" + str.ToString() + ")";
        }

        private static int SwitchCoordinates(int i, int index)
        {
            switch (i)
            {
                case 2:
                    index = 3;
                    break;
                case 3:
                    index = 2;
                    break;
            }
            return index;
        }

        public List<Dot> GetNuclearDots()
        {
            List<Dot> result = new List<Dot>();
            foreach (Dot currentDot in Values)
                if (currentDot.Value && ExtraData.DotIsNuclear(Values, currentDot))
                    result.Add(currentDot);
            return result;
        }

        public List<Edge> GetNuke()
        {
            List<Edge> result = new List<Edge>();
            foreach (Dot dot in NuclearDotsList)
                if (!ExtraData.ListContainsEdge(Sheaf.GetSheaf(Values, dot)[0], result))
                    result.Add(Sheaf.GetSheaf(Values, dot)[0]);
            return result;
        }

        public string GetNukeConsoleOutputString()
        {
            StringBuilder str = new StringBuilder();
            str.Append(string.Format("Ядро функции {0}:\n", ToString()));
            foreach (Dot dot in NuclearDotsList)
                str.Append(string.Format("\tГрань {0}, ядровая точка - {1}\n", Sheaf.GetSheaf(Values, dot)[0].GetCommonDotsString(), dot));
            return str.ToString();
        }

        public string GetMaxEdgesListConsoleOutputString()
        {
            StringBuilder str = new StringBuilder();
            str.Append(string.Format("Максимальные грани функции {0}:\n", ToString()));
            foreach (Edge edge in MaxEdgesList)
                str.Append(string.Format("\t{0}\n", edge.GetCommonDotsString()));
            return str.ToString();
        }
    }
}
