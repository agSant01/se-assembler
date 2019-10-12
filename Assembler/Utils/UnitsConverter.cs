using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Utils
{
    public static class UnitsConverter
    {
        public static string DecimalToHex(int decimalNumber)
        {
            return String.Format("0x{0:X}", decimalNumber);
        }

        public static int HexToDecimal(string hexNumber)
        {
            return Convert.ToInt32(Convert.ToInt64(hexNumber, 16));
        }

        public static string HexToBinary(string hexNumber)
        {
            return Convert.ToString(Convert.ToInt32(hexNumber, 16), 2);
        }
    }
}
