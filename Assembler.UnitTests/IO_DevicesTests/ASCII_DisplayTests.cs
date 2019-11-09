using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using Assembler.Core.Microprocessor.IO.IODevices;
using Assembler.Microprocessor;
using Assembler.Utils;

namespace Assembler.UnitTests.IO_DevicesTests
{
    [TestClass]
    class ASCII_DisplayTests
    {

        private VirtualMemory mem;
        private ASCII_Display display;
        private FileManager manager;

        [TestInitialize]
        public void TestInit()
        {
            //string[] lines = manager.ToReadFile("C:\\Users\\Alejandro Natal\\Documents\\" +
            //                                    "GitHub\\se-assembler\\Assembler.UnitTests" +
            //                                    "\\CompilerTests\\AssemblyFiles\\assembly_test.txt");
            ////string[] lines = { };
            //mem = new VirtualMemory(lines);
            //display = new ASCII_Display(mem);

        }

        [TestMethod]
        public void TestReservedAddressess()
        {
           int[] addresses = display.ReservedAddresses();
            Assert.AreEqual(8, addresses.Length);

            foreach(int address in addresses)
            {
                Console.WriteLine($"Reserved Addressess\n--------------\n");
                Console.WriteLine($"Address:{address}\n"); 
            }
        }

        [TestMethod]
        public void TestValidIndexes()
        {

        }

        [TestMethod]
        public void TestInvalidIndexes()
        {

        }

    }
}
