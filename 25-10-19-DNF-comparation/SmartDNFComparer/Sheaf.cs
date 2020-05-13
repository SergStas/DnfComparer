using System;
using System.Collections.Generic;
using System.Text;

namespace SmartDNFComparer
{
    class Sheaf
    {
        public Dot RootDot;
        public Function MotherFunction;
        public List<Edge> Edges;
        public List<Dot> CoveredDotsList;

        public Sheaf(Dot rootDot, Function motherFunction)
        {
            RootDot = rootDot;
            MotherFunction = motherFunction;
            Edges = GetEdgesList();
            CoveredDotsList = GetCoveredDots();
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

        public List<Edge> GetEdgesList()
        {
            if (!RootDot.Value)
                throw ExtraData.MustHave;
            List<Edge> result = new List<Edge>();
            if (!RootDot.Value)
                throw ExtraData.MustHave;
            int count = result.Count;
            Edge startingEdge = new Edge(new List<Dot>() { RootDot });
            for (int i = 0; i < MotherFunction.VarsCount; i++)
                ExpandCurrentEdge(startingEdge, MotherFunction, i, result);
            if (count == result.Count)
                result.Add(startingEdge);
            result = ExtraData.RemoveRepeatings(result);
            return result;
        }

        public static void ExpandCurrentEdge(Edge edge, Function function, int varIndex, List<Edge> result)
        {
            if (edge.ContainsDot(function.GetDotByCoords(edge.DotList[0].GetNeighborsCoords(varIndex))))
                return;
            List<Dot> expandedDots = new List<Dot>();
            foreach (Dot currentDot in edge.DotList)
            {
                ExtraData.Coordinates[] newCoords = currentDot.GetNeighborsCoords(varIndex);
                expandedDots.Add(currentDot);
                if (!function.GetDotByCoords(newCoords).Value)
                    return;
                expandedDots.Add(function.GetDotByCoords(newCoords));
            }
            int count = result.Count;
            Edge newEdge = new Edge(expandedDots);
            for (int i = 0; i < function.VarsCount; i++)
                ExpandCurrentEdge(newEdge, function, i, result);
            if (count == result.Count)
                result.Add(newEdge);
        }

        public string GetConsoleOutputString()
        {
            StringBuilder str = new StringBuilder();
            str.Append("Пучок максимальных граней для точки с координатами " + RootDot.GetCoordinatesString() + ":\n");
            for (int i = 0; i < Edges.Count; i++)
            {
                str.Append(string.Format("\t Ребро #{0} ({1}):\n", i, Edges[i].GetCommonDotsString()));
                for (int j = 0; j < Edges[i].DotList.Count; j++)
                    str.Append("\t\t" + Edges[i].DotList[j].ToString() + "\n");
            }
            return str.ToString();
        }
    }
}