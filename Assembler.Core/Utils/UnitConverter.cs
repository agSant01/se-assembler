using System;

namespace Assembler.Utils
{
    public static class UnitConverter
    {
        public static int BinaryToInt(string binaryNumber)
        {
            return Convert.ToInt32(binaryNumber, 2);
        }

        public static sbyte BinaryToSByte(string binaryNumber)
        {
            return Convert.ToSByte(binaryNumber, 2);
        }

        public static byte BinaryToByte(string binaryNumber)
        {
            return Convert.ToByte(binaryNumber, 2);
        }

        public static string BinaryToHex(string binaryNumber, byte defaultWidth = 2)
        {
            return Convert.ToString(Convert.ToInt32(binaryNumber, 2), 16).ToUpper().PadLeft(defaultWidth, '0');
        }

        public static string ByteToHex(byte decimalNumber, byte defaultWidth = 2)
        {
            return Convert.ToString(decimalNumber, 16).ToUpper().PadLeft(defaultWidth, '0');
        }

        public static string ByteToHex(sbyte decimalNumber, byte defaultWidth = 2)
        {
            return ByteToHex((byte)decimalNumber, defaultWidth);
        }

        public static string ByteToBinary(byte decimalNumber, byte defaultWidth = 8)
        {
            return Convert.ToString(decimalNumber, 2).PadLeft(defaultWidth, '0');
        }

        public static string ByteToBinary(sbyte decimalNumber, byte defaultWidth = 8)
        {
            return ByteToBinary((byte)decimalNumber, defaultWidth);
        }

        public static string IntToHex(int decimalNumber, byte defaultWidth = 2)
        {
            return Convert.ToString(decimalNumber, 16).ToUpper().PadLeft(defaultWidth, '0');
        }

        public static string IntToBinary(int decimalNumber, byte defaultWidth = 8)
        {
            return Convert.ToString(decimalNumber, 2).PadLeft(defaultWidth, '0');
        }

        public static int HexToInt(string hexNumber)
        {
            return Convert.ToInt32(Convert.ToInt32(hexNumber, 16));
        }

        public static sbyte HexToSByte(string hexNumber)
        {
            return Convert.ToSByte(hexNumber, 16);
        }

        public static byte HexToByte(string hexNumber)
        {
            return Convert.ToByte(hexNumber, 16);
        }

        public static string HexToBinary(string hexNumber, byte defaultWidth = 8)
        {
            return Convert.ToString(Convert.ToInt32(hexNumber, 16), 2).PadLeft(defaultWidth, '0');
        }

        public static ushort HexToU16Bit(string hexNumber)
        {
            return Convert.ToUInt16(hexNumber, 16);
        }

        public static string U16BitToHex(ushort decimalNumber, byte defaultWidth = 2)
        {
            return Convert.ToString(decimalNumber, 16).ToUpper().PadLeft(defaultWidth, '0');
        }
    }
}
