using System;
using System.Collections.Generic;
using System.Text;

namespace DNFComparer
{
    class MaxEdgeCreator
    {
        public static List<Edge> GetMaxEdges(Dot[,] functionValues)
        {
            List<Edge> result = new List<Edge>();
            foreach (Dot dot in functionValues)
                if (dot.Value)
                    AddEdges(Sheaf.GetSheaf(functionValues, dot), result);
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
                //else
                //    Console.WriteLine("REPEATING DETECTED");
                //    Console.WriteLine("BALBES DETECTED");
            }
        }
    }
}
