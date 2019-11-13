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
    }
}