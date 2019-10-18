using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Utils
{
    public static class ArithmeticOperations
    {
        /// <summary>
        /// Add two Hexadecimal Numbers
        /// </summary>
        /// <param name="hex1"></param>
        /// <param name="hex2"></param>
        /// <returns></returns>
        public static string HexAdd(string hex1, string hex2)
        {
            sbyte element1 = UnitConverter.HexToSByte(hex1);
            sbyte element2 = UnitConverter.HexToSByte(hex2);

            return UnitConverter.ByteToHex((byte)(element1 + element2));
        }

        /// <summary>
        /// Multiply two decimal numbers
        /// </summary>
        /// <param name="hex1">Multiplier</param>
        /// <param name="hex2">Multiplicand</param>
        /// <returns>Product</returns>
        public static string HexMultiply(string hex1, string hex2)
        {
            sbyte element1 = UnitConverter.HexToSByte(hex1);
            sbyte element2 = UnitConverter.HexToSByte(hex2);

            return UnitConverter.ByteToHex((byte)(element1 * element2));
        }

        /// <summary>
        /// Substract two Hexadecimal numbers.
        /// </summary>
        /// <param name="minuend">The number it is subtracted from</param>
        /// <param name="subtrahend">The number being subtracted</param>
        /// <returns>The difference between the minuend and subtrahend</returns>
        public static string HexSubstract(string minuend, string subtrahend)
        {
            sbyte element1 = UnitConverter.HexToSByte(minuend);
            sbyte element2 = UnitConverter.HexToSByte(subtrahend);

            return UnitConverter.ByteToHex((byte) (element1 - element2));
        }

        /// <summary>
        /// Division of two Hexadecimal Numbers
        /// </summary>
        /// <param name="dividend">The number being divided</param>
        /// <param name="divisor">The number divided by</param>
        /// <returns>The quotient</returns>
        public static string HexDivide(string dividend, string divisor)
        {
            sbyte element1 = UnitConverter.HexToSByte(dividend);
            sbyte element2 = UnitConverter.HexToSByte(divisor);

            return UnitConverter.ByteToHex((byte)(element1 - element2));
        }
    }
}
