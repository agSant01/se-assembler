using Assembler.Core.Microprocessor;
using Assembler.Core.Microprocessor.IO.IODevices;
using Assembler.Microprocessor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.UnitTests.IODevices
{
    [TestClass]
    public class SevenSegmentTests
    {
        [TestMethod]
        public void SevenSegmentTestsTests_ExecuteOneInstruction_Success()
        {
            var display = new IOSevenSegmentDisplay(4);

            Assert.AreEqual(false, display.HasData);
            Assert.AreEqual(string.Empty, display.Data);

            display.WriteInPort(4, "F6");

            Assert.AreEqual("11110110", display.Data);

            Console.WriteLine(display);
        }

        [TestMethod]
        public void SevenSegmentTestsTests_WriteFromMicro_Success()
        {
            VirtualMemory vm = new VirtualMemory(new string[] {
                "a806",
                "ff07",
                "0000",
                "0102",
                "0203",
                "c940",
                "a812",
                "1a04",
                "a816",
                "1904",
                "0b08",
                "a816"
            });

            IOManager manager = new IOManager(100);

            MicroSimulator micro = new MicroSimulator(vm, manager);

            Console.WriteLine("Initial State:");
            Console.Write(vm);
            Console.WriteLine(manager);
            Console.WriteLine(micro);

            IOSevenSegmentDisplay segment = new IOSevenSegmentDisplay(9);

            manager.AddIODevice(9, segment);

            Console.WriteLine("\nAfter adding IO Hex Keyboard:");
            Console.WriteLine(manager);

            for (int i = 0; i < 7; i++)
                micro.NextInstruction();

            foreach (char c in segment.Data)
                Assert.AreEqual("11110110", segment.Data);

            Console.WriteLine(segment);


        }
    }
}