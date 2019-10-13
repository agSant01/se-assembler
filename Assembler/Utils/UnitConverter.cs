using System;

namespace Assembler.Utils
{
    public static class UnitConverter
    {
        public static string DecimalToHex(int decimalNumber)
        {
            return String.Format("0x{0:X}", decimalNumber);
        }

        public static int HexToDecimal(string hexNumber)
        {
            return Convert.ToInt32(Convert.ToInt32(hexNumber, 16));
        }

        public static string HexToBinary(string hexNumber)
        {
            return Convert.ToString(Convert.ToInt32(hexNumber, 16), 2);
        }

        public static string DecimalToBinary(int decimalNumber)
        {
            return Convert.ToString(decimalNumber, 2);
        }

        public static int BinaryToDecimal(string binaryNumber)
        {
            return Convert.ToInt32(binaryNumber, 2);
        }

        public static string BinaryToHex(string binaryNumber)
        {
            return Convert.ToString(Convert.ToInt32(binaryNumber, 2), 16);
        }
    }
}
