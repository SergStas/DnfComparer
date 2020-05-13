using System;
using System.Collections.Generic;
using System.Text;

namespace SmartDNFComparer
{
    class Function
    {
        public Dot[] Values;
        public List<Edge> MaxEdgesList;
        public int ValuesCount;
        public int VarsCount;
        public List<Dot> NuclearDotsList;
        public List<Edge> Nuke;
        public List<Edge> QDNF;
        public List<Edge> STDNF;
        public bool QdnfAndStdnfAreEqual;
        public string KarnaughMap;

        public Function(Dot[] dots)
        {
            Values = dots;
            ValuesCount = Values.Length;
            VarsCount = (int)Math.Log2(ValuesCount);
            MaxEdgesList = GetMaxEdges();
            NuclearDotsList = GetNuclearDots();
            Nuke = GetNuke();
            QDNF = QuineDNFCreator.GetQDNFEdgeList(this);
            STDNF = SigmaTDNFCreator.GetSTDNFEdgeList(this);
            QdnfAndStdnfAreEqual = ExtraData.EdgesListsAreEqual(QDNF, STDNF);
            KarnaughMap = KarnaughMapCreator.GetKarnaughMapConsoleOutputString(this);
        }

        public List<Edge> GetMaxEdges()
        {
            return MaxEdgeCreator.GetMaxEdges(this);
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            foreach (Dot dot in Values)
                str.Append(dot.CharValue());
            return "(" + str.ToString() + ")";
        }

        public string GetComparationResultOutputString()
        {
            return string.Format("ДНФ Квайна и ДНФ Сигма Тэ данной функции {0}равны", QdnfAndStdnfAreEqual ? "" : "не ");
        }

        public List<Dot> GetNuclearDots()
        {
            List<Dot> result = new List<Dot>();
            foreach (Dot currentDot in Values)
                if (currentDot.Value && ExtraData.DotIsNuclear(this, currentDot))
                    result.Add(currentDot);
            return result;
        }

        public List<Edge> GetNuke()
        {
            List<Edge> result = new List<Edge>();
            foreach (Dot dot in NuclearDotsList)
                if (!ExtraData.ListContainsEdge(new Sheaf(dot, this).Edges[0], result))
                    result.Add(new Sheaf(dot, this).Edges[0]);
            return result;
        }

        public string GetNukeConsoleOutputString()
        {
            StringBuilder str = new StringBuilder();
            str.Append(string.Format("Ядро функции {0}:\n", ToString()));
            foreach (Edge currentEdge in Nuke)
            {
                str.Append(string.Format("\tГрань {0}, ядров", currentEdge));
                List<Dot> nuclearDots = new List<Dot>();
                foreach (Dot currentDot in currentEdge.DotList)
                    if (currentDot.IsNuclear(this)) 
                        nuclearDots.Add(currentDot);
                if (nuclearDots.Count == 1)
                {
                    str.Append(string.Format("ая точка - {0}\n", nuclearDots[0]));
                    continue;
                }
                str.Append("ые точки:\n");
                foreach (Dot currentDot in nuclearDots)
                    str.Append("\t\t" + currentDot.ToString() + '\n');
            }
            return str.ToString();
        }

        public string GetQDNFConsoleOutputString()
        {
            return QuineDNFCreator.GetQDNFConsoleOutputString(this);
        }

        public string GetSTDNFConsoleOutputString()
        {
            return SigmaTDNFCreator.GetSTDNFConsoleOutputString(this);
        }

        public string GetMaxEdgesListConsoleOutputString()
        {
            StringBuilder str = new StringBuilder();
            str.Append(string.Format("Максимальные грани функции {0}:\n", 'f' + ToString()));
            foreach (Edge edge in MaxEdgesList)
                str.Append(string.Format("\t{0}\n", edge.GetCommonDotsString()));
            return str.ToString();
        }

        public Dot GetDotByCoords(ExtraData.Coordinates[] coords)
        {
            foreach (Dot currentDot in Values)
                if (ExtraData.CoordinatesAreEqual(currentDot.Coordinates, coords))
                    return currentDot;
            throw ExtraData.MustHave;
        }
    }
}
