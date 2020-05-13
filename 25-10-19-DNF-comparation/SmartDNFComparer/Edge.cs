using System;
using System.Collections.Generic;
using System.Text;

namespace SmartDNFComparer
{
    class Edge
    {
        public int VarsCount;
        public ExtraData.Coordinates[] CommonCoordinates;
        public List<Dot> DotList;
        public int DotsCount;

        public Edge(List<Dot> dotList)
        {
            DotList = ExtraData.CopyDotList(dotList);
            CommonCoordinates = GetCommonCoordinates(DotList);
            VarsCount = DotList[0].Coordinates.Length;
            DotsCount = DotList.Count;
        }

        public void UpdateFields()
        {
            CommonCoordinates = GetCommonCoordinates(DotList);
        }

        private static ExtraData.Coordinates[] GetCommonCoordinates(List<Dot> dots)
        {
            int varsCount = dots[0].VarsCount;
            foreach (Dot dot in dots)
                if (dot == null)
                    return new ExtraData.Coordinates[] { ExtraData.Coordinates.FALSE, ExtraData.Coordinates.FALSE, ExtraData.Coordinates.FALSE, ExtraData.Coordinates.FALSE };
            ExtraData.Coordinates[] result = new ExtraData.Coordinates[varsCount];
            for (int i = 0; i < varsCount; i++)
                result[i] = dots[0].Coordinates[i];
            foreach (Dot dot in dots)
                for (int i = 0; i < varsCount; i++)
                    if (result[i] != dot.Coordinates[i])
                        result[i] = ExtraData.Coordinates.FALSE;
            return result;
        }

        public override string ToString()
        {
            return GetCommonDotsString();
        }

        public string GetCommonDotsString()
        {
            StringBuilder result = new StringBuilder();
            List<string> vars = new List<string>();
            for (int i = 0; i < VarsCount; i++)
                if (CommonCoordinates[i] != ExtraData.Coordinates.FALSE)
                    vars.Add("(" + (CommonCoordinates[i] == ExtraData.Coordinates.NOT ? "-" : "") + ExtraData.variables[i] + ")");
            if (vars.Count == 0)
                return "1";
            result.Append(string.Join('*', vars));
            return result.ToString();
        }

        public bool ContainsDot(Dot dot)
        {
            return ExtraData.ListContainsDot(dot, DotList);
        }
    }
}
