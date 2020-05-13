using System;
using System.Collections.Generic;
using System.Text;

namespace SmartDNFComparer
{
    static class SigmaTDNFCreator
    {
        public static bool DotIsRegular(Dot dot, Edge edge, Function function)
        {
            Sheaf sheaf = new Sheaf(dot, function);
            List<Dot> coveredBySheafDots = new List<Dot>();
            foreach (Edge currentEdge in sheaf.Edges)
                if (!ExtraData.EdgesAreEqual(edge, currentEdge))
                    foreach (Dot currentDot in currentEdge.DotList)
                        if (!ExtraData.ListContainsDot(currentDot, edge.DotList))
                            coveredBySheafDots.Add(currentDot);
            foreach (Dot currentDot in coveredBySheafDots)
            {
                Sheaf currentSheaf = new Sheaf(currentDot, function);
                if (ExtraData.SheafIsSubsheaf(sheaf, currentSheaf))
                    return true;
            }
            return false;
        }

        public static bool EdgeIsRegular(Edge edge, Function function)
        {
            if (ExtraData.ListContainsEdge(edge, function.Nuke))
                return false;
            if (function.ToString() == ("(0001111111111011)"))
                edge = edge;
            foreach (Dot dot in edge.DotList)
                if (!DotIsRegular(dot, edge, function))
                    return false;
            return true;
        }

        public static List<Edge> GetSTDNFEdgeList(Function function)
        {
            List<Edge> result = ExtraData.CopyEdgeList(function.MaxEdgesList);
            List<Edge> originalList = ExtraData.CopyEdgeList(result);
            foreach (Edge edge in originalList)
                if (EdgeIsRegular(edge, function))
                    result = ExtraData.RemoveEdgeFromList(result, edge);
            return result;
        }

        public static string GetSTDNFConsoleOutputString(Function function)
        {
            List<Edge> STDNFEdges = GetSTDNFEdgeList(function);
            StringBuilder str = new StringBuilder();
            str.Append(string.Format("ДНФ Сигма Тэ функции {0} = ", function.ToString()));
            foreach (Edge edge in STDNFEdges)
                str.Append(edge.ToString() + " V ");
            return str.ToString().Substring(0, str.ToString().Length - 3);
        }
    }
}