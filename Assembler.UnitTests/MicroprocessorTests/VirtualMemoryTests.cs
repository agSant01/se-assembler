using Assembler.Microprocessor;
using Assembler.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Assembler.UnitTests.MicroprocessorTests
{
    [TestClass]
    public class VirtualMemoryTests
    {
        private readonly string machineCodeFile = Path.Combine(
            Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
            @"MicroprocessorTests\TestFiles\assembly_test_OBJ_FILE.txt");

        private readonly string machineCodeFileInvalid = Path.Combine(
                   Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
                   @"MicroprocessorTests\TestFiles\assembly_test_OBJ_FILE_WRONGFORMAT.txt");

        [TestMethod]
        public void VirtualMemoryTest_ReadStringLines_Success()
        {
            string[] expectedLines = {
             //  evenAddress, oddAddress
             //   0     1
                "a8", "06",
            //    2     3
                "05", "07",
                "00", "00",
                "01", "02",
                "02", "03",
                "c9", "40",
                "a8", "12",
                "1a", "04",
                "a8", "16",
                "19", "04",
                "0b", "08",
                "a8", "16"
            };

            string[] lines = FileManager.Instance.ToReadFile(machineCodeFile);

            Assert.IsNotNull(lines);

            VirtualMemory vm = new VirtualMemory(lines);

            // complete VirtualMemery
            Console.WriteLine(vm);

            for (int i = 0; i < expectedLines.Length; i++)
            {
                Console.WriteLine($"Expected: {expectedLines[i]}, Result: {vm.GetContentsInHex(i)}");

                Assert.AreEqual(expectedLines[i], vm.GetContentsInHex(i));
            }

        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException), "More than 16 bit block size")]
        public void VirtualMemoryTest_ReadStringLines_BiggerThan16Bit_Fail()
        {
            string[] lines = FileManager.Instance.ToReadFile(machineCodeFileInvalid);

            Assert.IsNotNull(lines);

            VirtualMemory vm = new VirtualMemory(lines);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException), "Invalid DecimalAddress")]
        public void VirtualMemoryTest_ReadStringLines_InvalidDecimalAddress()
        {
            string[] lines = FileManager.Instance.ToReadFile(machineCodeFile);

            Assert.IsNotNull(lines);

            VirtualMemory vm = new VirtualMemory(lines);

            vm.GetContentsInHex(4200);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException), "Invalid DecimalAddress")]
        public void VirtualMemoryTest_ReadStringLines_InvalidHexAddress()
        {
            string[] lines = FileManager.Instance.ToReadFile(machineCodeFile);

            Assert.IsNotNull(lines);

            VirtualMemory vm = new VirtualMemory(lines);

            vm.GetContentsInHex("1194");
        }

        [TestMethod]
        public void VirtualMemoryTest_ReadStringLines_ValidHexAddress()
        {
            string[] lines = FileManager.Instance.ToReadFile(machineCodeFile);

            Assert.IsNotNull(lines);

            VirtualMemory vm = new VirtualMemory(lines);

            string expected = "02";

            string result = vm.GetContentsInHex("07");

            Assert.AreEqual(expected, result);

            Console.WriteLine($"Expected: {expected}, Result: {result}");
        }

        [TestMethod]
        public void VirtualMemoryTest_ReadStringLines_ValidDecimalAddress()
        {
            string[] lines = FileManager.Instance.ToReadFile(machineCodeFile);

            Assert.IsNotNull(lines);

            VirtualMemory vm = new VirtualMemory(lines);

            string expected = "07";

            string result = vm.GetContentsInHex(3);

            Assert.AreEqual(expected, result);

            Console.WriteLine($"Expected: {expected}, Result: {result}");
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException), "Invalid Hexadecimal Address")]
        public void VirtualMemoryTest_InvalidData_InvalidHexAddress()
        {
            string[] lines = FileManager.Instance.ToReadFile(machineCodeFile);

            Assert.IsNotNull(lines);

            VirtualMemory vm = new VirtualMemory(lines);

            vm.SetContentInMemory(45, "1194");
        }
    }
}
