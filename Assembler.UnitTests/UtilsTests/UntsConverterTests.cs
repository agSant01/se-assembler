using Assembler.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Assembler.UnitTests.UtilsTests
{
    [TestClass]
    public class UntsConverterTests
    {
        [TestMethod]
        public void UntsConverterTests_HexToDecimal_Success()
        {
            string test1 = "53";
            string test2 = "19";

            int expected1 = (5 * 16) + 3;
            int expected2 = 16 + 9;

            int result1 = UnitConverter.HexToInt(test1);
            int result2 = UnitConverter.HexToInt(test2);

            Console.WriteLine($"Test: {test1}, Decimal: {result1}");
            Console.WriteLine($"Test: {test2}, Decimal: {result2}");

            Assert.AreEqual(expected1, result1);
            Assert.AreEqual(expected2, result2);
        }

        [TestMethod]
        public void UntsConverterTests_DecimalToHex_Success()
        {
            int test1 = 10 * 16 + 3;
            int test2 = 15 * (int)Math.Pow(16, 2) + 9;

            string expected1 = "A3";
            string expected2 = "F09";


            string result1 = UnitConverter.IntToHex(test1);
            string result2 = UnitConverter.IntToHex(test2);

            Console.WriteLine($"Test: {test1}, Decimal: {result1}");
            Console.WriteLine($"Test: {test2}, Decimal: {result2}");

            Assert.AreEqual(expected1, result1);
            Assert.AreEqual(expected2, result2);
        }

        [TestMethod]
        public void UntsConverterTests_DecimalToHexSbyte_Success()
        {
            sbyte test1 = (5 * 16 + 3);
            sbyte test2 = 25;

            string expected1 = "53";
            string expected2 = "19";


            string result1 = UnitConverter.IntToHex(test1);
            string result2 = UnitConverter.IntToHex(test2);

            Console.WriteLine($"Test: {test1}, Decimal: {result1}");
            Console.WriteLine($"Test: {test2}, Decimal: {result2}");

            Assert.AreEqual(expected1, result1);
            Assert.AreEqual(expected2, result2);
        }

        [TestMethod]
        public void UntsConverterTests_DecimalToBinary_Success()
        {
            int test1 = 10 * 16 + 3;
            int test2 = 15 * (int)Math.Pow(16, 2) + 9;

            string expected1 = "10100011";
            string expected2 = "111100001001";


            string result1 = UnitConverter.IntToBinary(test1);
            string result2 = UnitConverter.IntToBinary(test2);

            Console.WriteLine($"Decimal: {test1}, Binary: {result1}");
            Console.WriteLine($"Decimal: {test2}, Binary: {result2}");

            Assert.AreEqual(expected1, result1);
            Assert.AreEqual(expected2, result2);
        }

        [TestMethod]
        public void UntsConverterTests_HexToBinary_Success()
        {

            string test1 = "A3";
            string test2 = "F09";

            string expected1 = "10100011";
            string exp2 = "111100001001";


            string result1 = UnitConverter.HexToBinary(test1);
            string result2 = UnitConverter.HexToBinary(test2);

            Console.WriteLine($"Test: {test1}, Binary: {result1}");
            Console.WriteLine($"Test: {test2}, Binary: {result2}");

            Assert.AreEqual(expected1, result1);
            Assert.AreEqual(exp2, result2);
        }

        [TestMethod]
        public void UntsConverterTests_HexToDecimal_NullHexInput_Success()
        {
            string test1 = null;

            int result = UnitConverter.HexToInt(test1);

            Console.WriteLine($"Result: {result}");

            Assert.AreEqual(0, result);

        }
    }
}
