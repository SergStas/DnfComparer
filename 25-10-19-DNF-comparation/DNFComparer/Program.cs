using System;
using System.Collections.Generic;

namespace DNFComparer
{
    class Program
    {
        static void Main(string[] args)
        {
            Function function = InputData.KeyboardInput();
            Console.WriteLine((new Edge(function.Values)).ToMatrix());
            Console.WriteLine(function.ToString());
            WriteBorder();
            Console.WriteLine(function.GetMaxEdgesListConsoleOutputString());
            Console.WriteLine(function.GetNukeConsoleOutputString());
            Console.WriteLine(QuineDNFCreator.GetQDNFConsoleOutputString(function));
            Console.WriteLine(SigmaTDNFCreator.GetSTDNFConsoleOutputString(function));
        }

        public static void WriteBorder()
        {
            Console.WriteLine("\n============================\n");
        }

        public static Function GenerateRandomFunction(int key)
        {
            int sizeX = 4;
            int sizeY = 4;
            Random random = new Random(key);
            Dot[,] dots = new Dot[4, 4];
            int rnd;
            for (int i = 0; i < sizeY; i++)
                for (int j = 0; j < sizeX; j++)
                {
                    ExtraData.Coordinates[] coordinates = new ExtraData.Coordinates[4];
                    switch (i)
                    {
                        case 0:
                            coordinates[0] = ExtraData.Coordinates.NOT;
                            coordinates[1] = ExtraData.Coordinates.NOT;
                            break;
                        case 1:
                            coordinates[0] = ExtraData.Coordinates.NOT;
                            coordinates[1] = ExtraData.Coordinates.IS;
                            break;
                        case 2:
                            coordinates[0] = ExtraData.Coordinates.IS;
                            coordinates[1] = ExtraData.Coordinates.IS;
                            break;
                        case 3:
                            coordinates[0] = ExtraData.Coordinates.IS;
                            coordinates[1] = ExtraData.Coordinates.NOT;
                            break;
                    }
                    switch (j)
                    {
                        case 0:
                            coordinates[2] = ExtraData.Coordinates.NOT;
                            coordinates[3] = ExtraData.Coordinates.NOT;
                            break;
                        case 1:
                            coordinates[2] = ExtraData.Coordinates.NOT;
                            coordinates[3] = ExtraData.Coordinates.IS;
                            break;
                        case 2:
                            coordinates[2] = ExtraData.Coordinates.IS;
                            coordinates[3] = ExtraData.Coordinates.IS;
                            break;
                        case 3:
                            coordinates[2] = ExtraData.Coordinates.IS;
                            coordinates[3] = ExtraData.Coordinates.NOT;
                            break;
                    }
                    rnd = random.Next(2);
                    dots[i, j] = new Dot((rnd == 1 ? true : false), coordinates);
                }
            return new Function(dots);
        }
    }
}
