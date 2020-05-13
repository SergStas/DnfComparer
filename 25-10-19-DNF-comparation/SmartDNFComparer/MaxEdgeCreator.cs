using System;
using System.Collections.Generic;
using System.Text;

namespace SmartDNFComparer
{
    static class MaxEdgeCreator
    {
        public static List<Edge> GetMaxEdges(Function function)
        {
            List<Edge> result = new List<Edge>();
            foreach (Dot dot in function.Values)
                if (dot.Value)
                    AddEdges(new Sheaf(dot, function).Edges, result);
            return result;
        }

        public static void AddEdges(List<Edge> edges, List<Edge> list)
        {
            foreach (Edge currentEdge in edges)
            {
                bool isNew = true;
                foreach (Edge listEdge in list)
                    if (ExtraData.EdgesAreEqual(listEdge, currentEdge))
                        isNew = false;
                if (isNew)
                    list.Add(currentEdge);
            }
        }
    }
}
