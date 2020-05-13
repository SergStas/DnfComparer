using System;
using System.Collections.Generic;

namespace SmartDNFComparer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            bool checkFunctionCases = false;
            bool checkGrayCodeCreator = false;
            bool checkKeyboardInput = false;
            bool doMainWork = true;
            bool printing = false;
            bool fullInfo = false;
            bool stops = true;
            if (checkFunctionCases)
                foreach (Function currentFunction in functionCases)
                    PrintFunctionInfo(currentFunction);
            if (checkGrayCodeCreator)
                for (int i = 1; i < 7; i++)
                {
                    string[] grayCode = KarnaughMapCreator.GetGrayCode(i, i == 1 ? true : false);
                    foreach (string currentString in grayCode)
                        Console.WriteLine(currentString);
                    Console.WriteLine();
                }
            if (checkKeyboardInput)
            {
                Function function = InputData.KeyboardInput();
                PrintFunctionInfo(function);
            }
            if (doMainWork)
                DoMainWork(printing, fullInfo, stops);
        }

        public static void DoMainWork(bool printing, bool fullInfo, bool stops)
        {
            int errors = 0;
            List<Function> unequalityies = new List<Function>();
             for (int i = 1; i < 5; i++)
             {
                for (int j = 0; j < Math.Pow(2, Math.Pow(2, i)); j++)
                {
                    Function resultFunction = Generator.GetCurrentFunction(i, j);
                    if (printing)
                    {
                        if (fullInfo)
                            PrintFunctionInfo(resultFunction);
                        bool result = resultFunction.QdnfAndStdnfAreEqual;
                        Console.WriteLine(string.Format("Тест #{0}, функция: {1}, результат: {2}", j, resultFunction.ToString(), result ? "+" : "-"));
                        if (j == 8187)
                            PrintFunctionInfo(resultFunction);
                    }
                    if (!resultFunction.QdnfAndStdnfAreEqual)
                    {
                        unequalityies.Add(resultFunction);
                        errors++;
                    }
                }
                Console.WriteLine("Найдено несоответствий на данном этапе: ");
                Console.WriteLine(errors);
                if (printing)
                    foreach (Function currentFunction in unequalityies)
                        Console.WriteLine(KarnaughMapCreator.GetFunctionMatrixConsoleOutputString(currentFunction) + '\n');
                if (stops)
                {
                    Console.WriteLine(string.Format("Перебраны все функции от {0} переменных. Продолжить?", i));
                    Console.ReadKey();
                }
             }
        }

        public static void PrintFunctionInfo(Function function)
        {
            Console.WriteLine("\nFunction = f" + function.ToString());
            Console.WriteLine(function.GetMaxEdgesListConsoleOutputString());
            Console.WriteLine(function.GetNukeConsoleOutputString());
            Console.WriteLine(function.GetQDNFConsoleOutputString());
            Console.WriteLine(function.GetSTDNFConsoleOutputString() + '\n');
            Console.WriteLine(function.GetComparationResultOutputString());
            Console.WriteLine(function.KarnaughMap);
            Console.WriteLine("------------------------------------------------");
        }

        public static Function[] functionCases = new Function[]
        {
            new Function(new Dot[]
            {
                new Dot(true, new ExtraData.Coordinates[]
                {
                    ExtraData.Coordinates.NOT
                }),
                new Dot(true, new ExtraData.Coordinates[]
                {
                    ExtraData.Coordinates.IS
                })
            }),
            new Function(new Dot[]
            {
                new Dot(true, new ExtraData.Coordinates[]
                {
                    ExtraData.Coordinates.NOT
                }),
                new Dot(false, new ExtraData.Coordinates[]
                {
                    ExtraData.Coordinates.IS
                })
            }),
            new Function(new Dot[]
            {
                new Dot(true, new ExtraData.Coordinates[]
                {
                    ExtraData.Coordinates.NOT,
                    ExtraData.Coordinates.NOT
                }),
                new Dot(true, new ExtraData.Coordinates[]
                {
                    ExtraData.Coordinates.NOT,
                    ExtraData.Coordinates.IS
                }),
                new Dot(true, new ExtraData.Coordinates[]
                {
                    ExtraData.Coordinates.IS,
                    ExtraData.Coordinates.NOT
                }),
                new Dot(false, new ExtraData.Coordinates[]
                {
                    ExtraData.Coordinates.IS,
                    ExtraData.Coordinates.IS
                })
            })
        };
    }
}
