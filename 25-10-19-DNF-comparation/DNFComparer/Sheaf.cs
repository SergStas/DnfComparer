using System;
using System.Collections.Generic;
using System.Text;

namespace DNFComparer
{
    class Sheaf
    {
        public Dot RootDot;
        public List<Edge> Edges;
        public Function MotherFunction;
        public List<Dot> CoveredDotsList;

        public Sheaf(Dot rootDot, Dot[,] functionValues)
        {
            RootDot = rootDot;
            Edges = GetSheaf(functionValues, rootDot);
            MotherFunction = new Function(functionValues);
            CoveredDotsList = GetCoveredDots();
        }

        public static List<Edge> GetSheaf(Dot[,] functionValues, Dot dot)
        {
            if (dot.Value == false)
            {
                return null;
            }
            List<Edge> result = new List<Edge>();
            int c = result.Count;
            Edge startingEdge = new Edge(new Dot[,] { { dot } });
            for (int i = 0; i < 4; i++)
            {
                ExpandCurrentEdge(startingEdge, (ExtraData.Direction)i, functionValues, result);
            }
            if (c == result.Count)
            {
                startingEdge.UpdateFields();
                result.Add(startingEdge);
            }
            result = ExtraData.RemoveRepeatings(result);
            return result;
        }

        public List<Dot> GetCoveredDots()
        {
            List<Dot> result = new List<Dot>();
            foreach (Edge edge in Edges)
                foreach (Dot dot in edge.DotList)
                    if (!ExtraData.ListContainsDot(dot, result))
                        result.Add(dot);
            return result;
        }

        public static void ExpandCurrentEdge(Edge currentEdge, ExtraData.Direction dir, Dot[,] functionValues, List<Edge> result)
        {
            bool OX = (dir == ExtraData.Direction.Right) || (dir == ExtraData.Direction.Left);
            int SizeX = functionValues.GetLength(1);
            int SizeY = functionValues.GetLength(0);
            bool endCondition = OX ? (currentEdge.SizeX == SizeX) : (currentEdge.SizeY == SizeY);
            if (endCondition)
                return;
            int newX = OX ? 2 * currentEdge.SizeX : currentEdge.SizeX;
            int newY = OX ? currentEdge.SizeY : 2 * currentEdge.SizeY;
            Edge newEdge = new Edge(new Dot[newY, newX]);
            for (int i = 0; i < currentEdge.SizeY; i++)
                for (int j = 0; j < currentEdge.SizeX; j++)
                {
                    newEdge.Dots[i, j] = currentEdge.Dots[i, j];
                    int x = ConvertCoordinateToIntIndex(currentEdge.Dots[i, j].Coordinates)[0];
                    int y = ConvertCoordinateToIntIndex(currentEdge.Dots[i, j].Coordinates)[1];
                    switch (dir)
                    {
                        case ExtraData.Direction.Right:
                            newEdge.Dots[i, j + currentEdge.SizeX] = functionValues[y, (x + currentEdge.SizeX) % SizeX];
                            break;
                        case ExtraData.Direction.Down:
                            newEdge.Dots[i + currentEdge.SizeY, j] = functionValues[(y + currentEdge.SizeY) % SizeY, x];
                            break;
                        case ExtraData.Direction.Left:
                            newEdge.Dots[i, (j + currentEdge.SizeX)] = functionValues[y, (x - currentEdge.SizeX + SizeX) % SizeX];
                            break;
                        default:
                            newEdge.Dots[i + currentEdge.SizeY, j] = functionValues[(y - currentEdge.SizeY + SizeY) % SizeY, x];
                            break;
                    }
                }
            foreach (Dot dot in newEdge.Dots)
                if (!dot.Value)
                    return;
            int c = result.Count;
            for (int i = 0; i < 4; i++)
                ExpandCurrentEdge(newEdge, (ExtraData.Direction)i, functionValues, result);
            if (c == result.Count)
            {
                newEdge.UpdateFields();
                result.Add(newEdge);
            }
        } 

        private static int[] ConvertCoordinateToIntIndex(ExtraData.Coordinates[] coordinates)
        {
            int i; int j;
            switch (coordinates[0])
            {
                case ExtraData.Coordinates.NOT:
                    if (coordinates[1] == ExtraData.Coordinates.NOT)
                        i = 0;
                    else
                        i = 1;
                    break;
                default:
                    if (coordinates[1] == ExtraData.Coordinates.NOT)
                        i = 3;
                    else
                        i = 2;
                    break;
            }
            switch (coordinates[2])
            {
                case ExtraData.Coordinates.NOT:
                    if (coordinates[3] == ExtraData.Coordinates.NOT)
                        j = 0;
                    else
                        j = 1;
                    break;
                default:
                    if (coordinates[3] == ExtraData.Coordinates.NOT)
                        j = 3;
                    else
                        j = 2;
                    break;
            }
            return new int[] { j, i };
        }

        public string GetConsoleOutputString()
        {
            StringBuilder str = new StringBuilder();
            str.Append("Пучок максимальных ребер для точки с координатами " + RootDot.GetCoordinatesString() + ":\n");
            for (int i = 0; i < Edges.Count; i++)
            {
                str.Append(string.Format("\t Ребро #{0} ({1}):\n", i, Edges[i].GetCommonDotsString()));
                for (int j = 0; j < Edges[i].DotList.Count; j++)
                    str.Append("\t\t" + Edges[i].DotList[j].GetCoordinatesString() + "\n");
                str.Append("\n");
            }
            return str.ToString();
        }
    }
}