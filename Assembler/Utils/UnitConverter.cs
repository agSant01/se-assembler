using System;

namespace Assembler.Utils
{
    public static class UnitConverter
    {
        public static string DecimalToHex(int decimalNumber, byte defaultWidth = 2)
        {
            return Convert.ToString(decimalNumber, 16).PadLeft(defaultWidth, '0');
        }

        public static int HexToDecimal(string hexNumber)
        {
            return Convert.ToInt32(Convert.ToInt32(hexNumber, 16));
        }

        public static string HexToBinary(string hexNumber, byte defaultWidth = 8)
        {
            return Convert.ToString(Convert.ToInt32(hexNumber, 16), 2).PadLeft(defaultWidth, '0');
        }

        public static string DecimalToBinary(int decimalNumber, byte defaultWidth = 8)
        {
            return Convert.ToString(decimalNumber, 2).PadLeft(defaultWidth, '0');
        }

        public static int BinaryToDecimal(string binaryNumber)
        {
            return Convert.ToInt32(binaryNumber, 2);
        }

        public static string BinaryToHex(string binaryNumber, byte defaultWidth = 2)
        {
            return Convert.ToString(Convert.ToInt32(binaryNumber, 2), 16).PadLeft(defaultWidth, '0');
        }
    }
}
