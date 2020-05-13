using System;
using System.Collections.Generic;
using System.Text;

namespace DNFComparer
{
    class QuineDNFCreator
    {
        public static List<Dot> GetCoveredByNukeDots(Function function)
        {
            List<Dot> result = new List<Dot>();
            List<Dot> nuclearDots = function.NuclearDotsList;
            List<Edge> nuke = function.Nuke;
            foreach (Edge edge in nuke)
                foreach (Dot dot in edge.DotList)
                    if (!ExtraData.ListContainsDot(dot, nuclearDots) && !ExtraData.ListContainsDot(dot, result))
                        result.Add(dot);
            return result;
        }

        public static bool EdgeIsCoveredByNuke(List<Dot> coveredDots, Edge edge)
        {
            foreach (Dot dot in edge.DotList)
                if (!ExtraData.ListContainsDot(dot, coveredDots))
                    return false;
            return true;
        }

        public static List<Edge> GetQDNFEdgeList(Function function)
        {
            List<Dot> coveredDots = GetCoveredByNukeDots(function);
            List<Edge> result = ExtraData.CopyEdgeList(function.MaxEdgesList);
            List<Edge> originalList = ExtraData.CopyEdgeList(result);
            foreach (Edge edge in originalList)
                if (EdgeIsCoveredByNuke(coveredDots, edge))
                    result = ExtraData.RemoveEdgeFromList(result, edge);
            return result;
        }

        public static string GetQDNFConsoleOutputString(Function function)
        {
            List<Edge> QDNFEdges = GetQDNFEdgeList(function);
            StringBuilder str = new StringBuilder();
            str.Append(string.Format("ДНФ Квайна функции {0} = ", function.ToString()));
            foreach (Edge edge in QDNFEdges)
                str.Append(edge.ToString()+" V ");
            return str.ToString().Substring(0, str.ToString().Length - 3);
        }
    }
}
