using System;
using System.Collections.Generic;
using System.Text;

namespace DNFComparer
{
    class Dot
    {
        public ExtraData.Coordinates[] Coordinates;
        public bool Value;

        public Dot(bool value, ExtraData.Coordinates[] coordinates)
        {
            Value = value;
            Coordinates = coordinates;
        }

        public override string ToString()
        {
            return string.Format("Function" + GetCoordinatesString() + " = {0}", Value.ToString());
        }

        public string GetCoordinatesString()
        {
            string arg1 = Coordinates[0] == ExtraData.Coordinates.NOT ? "-" : "";
            string arg2 = Coordinates[1] == ExtraData.Coordinates.NOT ? "-" : "";
            string arg3 = Coordinates[2] == ExtraData.Coordinates.NOT ? "-" : "";
            string arg4 = Coordinates[3] == ExtraData.Coordinates.NOT ? "-" : "";
            return string.Format("({0}X, {1}Y, {2}Z, {3}T)",
                arg1, arg2, arg3, arg4);
        }

        public char CharValue()
        {
            return Value ? '1' : '0';
        }
    }
}
