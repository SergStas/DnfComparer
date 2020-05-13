using System;
using System.Collections.Generic;
using System.Text;

namespace SmartDNFComparer
{
    static class Generator
    {
        public static Function GetCurrentFunction(int count, int number)
        {
            string result = "";
            while (number != 0)
            {
                result = (number % 2).ToString() + result;
                number /= 2;
            }
            while (result.Length < Math.Pow(2, count))
                result = '0' + result;
            return InputData.GenerateFunctionFromString(result);
        }
    }
}
