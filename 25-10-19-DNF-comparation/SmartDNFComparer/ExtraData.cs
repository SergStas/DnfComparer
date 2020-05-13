using System;
using System.Collections.Generic;
using System.Text;

namespace SmartDNFComparer
{
    static class ExtraData
    {
        public enum Coordinates
        {
            FALSE,
            NOT,
            IS
        }

        public static bool EdgesListsAreEqual(List<Edge> list1, List<Edge> list2)
        {
            if (list1.Count != list2.Count)
                return false;
            foreach (Edge currentEdge in list1)
                if (GetEdgeWithSameCoords(currentEdge, list2) == null)
                    return false;
            return true;
        }

        public static Edge GetEdgeWithSameCoords(Edge edge, List<Edge> list)
        {
            foreach (Edge currentEdge in list)
                if (CoordinatesAreEqual(edge.CommonCoordinates, currentEdge.CommonCoordinates))
                    return currentEdge;
            return null;
        }

        public static bool EdgesAreEqual(Edge edge1, Edge edge2)
        {
            List<Dot> dots1 = edge1.DotList;
            List<Dot> dots2 = edge2.DotList;
            if (dots1.Count != dots2.Count)
                return false;
            foreach (Dot currentDot in dots1)
            {
                Coordinates[] coords = currentDot.Coordinates;
                Dot sameCoordsDot = GetDotWithSameCoordinates(coords, dots2);
                if (sameCoordsDot == null || currentDot.Value != sameCoordsDot.Value)
                    return false;
            }
            return true;
        }

        public static Dot GetDotWithSameCoordinates(Coordinates[] coordinates, List<Dot> dotList)
        {
            foreach (Dot currentDot in dotList)
            {
                bool coordinatesAreEqual = true;
                for (int i = 0; i < coordinates.Length; i++)
                    if (currentDot.Coordinates[i] != coordinates[i])
                        coordinatesAreEqual = false;
                if (coordinatesAreEqual)
                    return currentDot;
            }
            return null;
        }

        public static bool DotIsNuclear(Function function, Dot dot)
        {
            if (!dot.Value)
            {
                Console.WriteLine("Вы реально балбес-_-");
                return false;
            }
            List<Edge> edges = new Sheaf(dot, function).Edges;
            return edges.Count == 1;
        }

        public static Exception MustHave = new Exception("\n\nНекорректный ввод\n");

        public static bool DotsAreEqual(Dot dot1, Dot dot2)
        {
            if (dot1.VarsCount != dot2.VarsCount)
                throw new Exception("\n\nЧто-то явно идет не так...\n");
            if ((dot1.Coordinates.Length != dot2.Coordinates.Length) || (dot1.Value != dot2.Value))
                return false;
            return CoordinatesAreEqual(dot1.Coordinates, dot2.Coordinates);
        }

        public static bool CoordinatesAreEqual(Coordinates[] coords1, Coordinates[] coords2)
        {
            if (coords1.Length != coords2.Length)
                return false;
            for (int i = 0; i < coords1.Length; i++)
                if (coords1[i] != coords2[i])
                    return false;
            return true;
        }

        public static bool ListContainsDot(Dot dot, List<Dot> list)
        {
            foreach (Dot currentDot in list)
                if (DotsAreEqual(dot, currentDot))
                    return true;
            return false;
        }

        public static bool ListContainsEdge(Edge edge, List<Edge> list)
        {
            foreach (Edge currentEdge in list)
                if (EdgesAreEqual(edge, currentEdge))
                    return true;
            return false;
        }

        public static List<Edge> RemoveRepeatings(List<Edge> original)
        {
            List<Edge> result = CopyEdgeList(original);
            if (original.Count > 1)
                for (int i = 1; i < original.Count; i++)
                {
                    if (ListContainsEdge(original[i], GetEdgeSublist(original, i)))
                        result = RemoveEdgeFromList(result, original[i]);
                }
            return result;
        }

        public static List<Edge> GetEdgeSublist(List<Edge> original, int length)
        {
            List<Edge> result = new List<Edge>();
            for (int i = 0; i < length; i++)
                result.Add(original[i]);
            return result;
        }

        public static Dot CopyDot(Dot original)
        {
            Coordinates[] coords = new Coordinates[original.Coordinates.Length];
            for (int i = 0; i < original.Coordinates.Length; i++)
                coords[i] = original.Coordinates[i];
            return new Dot(original.Value, coords);
        }

        public static Edge CopyEdge(Edge original)
        {
            List<Dot> resultList = new List<Dot>();
            foreach (Dot currentDot in original.DotList)
                resultList.Add(currentDot);
            return new Edge(resultList);
        }

        public static List<Edge> CopyEdgeList(List<Edge> original)
        {
            List<Edge> result = new List<Edge>();
            foreach (Edge edge in original)
                result.Add(CopyEdge(edge));
            return result;
        }

        public static List<Edge> RemoveEdgeFromList(List<Edge> list, Edge edge)
        {
            List<Edge> result = new List<Edge>();
            bool deleted = false;
            foreach (Edge currentEdge in list)
            {
                if (!EdgesAreEqual(currentEdge, edge) || deleted)
                    result.Add(currentEdge);
                else
                    deleted = true;
            }
            return result;
        }

        public static bool SheafIsSubsheaf(Sheaf motherSheaf, Sheaf subsheaf)
        {
            if (subsheaf.CoveredDotsList.Count > motherSheaf.CoveredDotsList.Count)
                return false;
            foreach (Dot currentDot in subsheaf.CoveredDotsList)
            {
                bool covered = false;
                foreach (Edge currentEdge in motherSheaf.Edges)
                {
                    if (ListContainsDot(currentDot, currentEdge.DotList))
                    {
                        covered = true;
                        break;
                    }
                }
                if (!covered)
                    return false;
            }
            return true;
        }

        public static List<Dot> CopyDotList(List<Dot> original)
        {
            List<Dot> result = new List<Dot>();
            foreach (Dot currentDot in original)
                result.Add(CopyDot(currentDot));
            return result;
        }

        public static Coordinates[] CopyCoordsArray(Coordinates[] original)
        {
            Coordinates[] result = new Coordinates[original.Length];
            for (int i = 0; i < original.Length; i++)
                result[i] = original[i];
            return result;
        }

        public static char[] variables = new char[] { 'X', 'Y', 'Z', 'T', 'U', 'V', 'W', 'A', 'B', 'C', 'D' };
    }
}
