using System;
using System.Collections.Generic;
using System.Text;

namespace DNFComparer
{
    class ExtraData
    {
        public enum Coordinates
        {
            FALSE,
            NOT,
            IS
        }

        public enum Direction
        {
            Right,
            Down,
            Left,
            Up
        }

        public static bool EdgesAreEqual(Edge edge1, Edge edge2)
        {
            if (edge1.Dots.Length != edge2.Dots.Length)
                return false;
            List<Dot> dots1 = edge1.GetDotList();
            List<Dot> dots2 = edge2.GetDotList();
            foreach (Dot currentDot in dots1)
            {
                Coordinates[] coords = currentDot.Coordinates;
                Dot sameCoordsDot = GetDotWithSameCoordinates(coords, dots2);
                if (sameCoordsDot == null || currentDot.Value != sameCoordsDot.Value)
                    return false;
            }
            return true;
        }

        public static Dot GetDotWithSameCoordinates(ExtraData.Coordinates[] coordinates, List<Dot> dotList)
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

        public static bool DotIsNuclear(Dot[,] dots, Dot dot)
        {
            if (!dot.Value)
            {
                Console.WriteLine("Вы реально балбес-_-");
                return false;
            }
            List<Edge> edges = Sheaf.GetSheaf(dots, dot);
            return edges.Count == 1;
        }

        public static bool DotsAreEqual(Dot dot1, Dot dot2)
        {
            if ((dot1.Coordinates.Length != dot2.Coordinates.Length) || (dot1.Value != dot2.Value))
                return false;
            for (int i = 0; i < dot1.Coordinates.Length; i++)
                if (dot1.Coordinates[i] != dot2.Coordinates[i])
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
                        result = RemoveEdgeFromList(result,original[i]);
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
            int sizeX = original.Dots.GetLength(1);
            int sizeY = original.Dots.GetLength(0);
            Edge result = new Edge(new Dot[sizeY, sizeX]);
            for (int i = 0; i < sizeY; i++)
                for (int j = 0; j < sizeX; j++)
                    result.Dots[i, j] = CopyDot(original.Dots[i, j]);
            result.UpdateFields();
            return result;
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


        public static Coordinates[] CopyCoordsArray(Coordinates[] original)
        {
            Coordinates[] result = new Coordinates[original.Length];
            for (int i = 0; i < original.Length; i++)
                result[i] = original[i];
            return result;
        }

        public static char[] variables = new char[] { 'X', 'Y', 'Z', 'T', 'W', 'U', 'V', 'O' };
    }
}
