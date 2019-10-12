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
            string test1 = "A3";
            string test2 = "F09";

            int expected1 = (10 * 16) + 3;
            int expected2 = 15 * (int)Math.Pow(16, 2) + 9;

            int result1 = UnitsConverter.HexToDecimal(test1);
            int result2 = UnitsConverter.HexToDecimal(test2);

            Console.WriteLine($"Test: {test1}, Decimal: {result1}");
            Console.WriteLine($"Test: {test2}, Decimal: {result2}");

            Assert.AreEqual(expected1, result1);
            Assert.AreEqual(expected2, result2);
        }

        [TestMethod]
        public void UntsConverterTests_DecimalToHex_Success()
        {
            int test1 = 10 * 16 + 3;
            int test2 = 15 * (int) Math.Pow(16, 2) + 9;

            string expected1 = "0xA3";
            string expected2 = "0xF09";


            string result1 = UnitsConverter.DecimalToHex(test1);
            string result2 = UnitsConverter.DecimalToHex(test2);

            Console.WriteLine($"Test: {test1}, Decimal: {result1}");
            Console.WriteLine($"Test: {test2}, Decimal: {result2}");

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


            string result1 = UnitsConverter.HexToBinary(test1);
            string result2 = UnitsConverter.HexToBinary(test2);

            Console.WriteLine($"Test: {test1}, Binary: {result1}");
            Console.WriteLine($"Test: {test2}, Binary: {result2}");

            Assert.AreEqual(expected1, result1);
            Assert.AreEqual(exp2, result2);
        }
    }
}
