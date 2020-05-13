using System;
using System.Collections.Generic;
using System.Text;

namespace SmartDNFComparer
{
    class Dot
    {
        public ExtraData.Coordinates[] Coordinates;
        public bool Value;
        public int VarsCount;

        public Dot(bool value, ExtraData.Coordinates[] coordinates)
        {
            Value = value;
            Coordinates = coordinates;
            VarsCount = Coordinates.Length;
        }

        public override string ToString()
        {
            return string.Format("f" + GetCoordinatesString() + " = {0}", Value.ToString());
        }

        public bool IsNuclear(Function function)
        {
            return ExtraData.DotIsNuclear(function, this);
        }

        public string GetCoordinatesString()
        {
            StringBuilder str = new StringBuilder();
            str.Append("(");
            for (int i = 0; i < Coordinates.Length; i++)
            {
                str.Append((Coordinates[i] == ExtraData.Coordinates.IS ? "" : "-") + ExtraData.variables[i]);
                if (i != Coordinates.Length - 1)
                    str.Append(", ");
            }
            str.Append(")");
            return str.ToString();
        }

        public char CharValue()
        {
            return Value ? '1' : '0';
        }

        public ExtraData.Coordinates[] GetNeighborsCoords(int varIndex)
        {
            ExtraData.Coordinates[] result = new ExtraData.Coordinates[Coordinates.Length];
            Array.Copy(Coordinates, result, Coordinates.Length);
            result[varIndex] = result[varIndex] == ExtraData.Coordinates.IS ? ExtraData.Coordinates.NOT : ExtraData.Coordinates.IS;
            return result;
        }
    }
}
