using Assembler.Core.Microprocessor.IO.IODevices;
using Assembler.Microprocessor;
using Assembler.Microprocessor.InstructionFormats;
using Assembler.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Assembler.UnitTests.IODevices
{
    [TestClass]
    public class IOHexKeyboardTests
    {
        [TestMethod]
        public void MCLoaderTests_ExecuteOneInstruction_Success()
        {
            IOHexKeyboard kb = new IOHexKeyboard();

            Assert.AreEqual("00000000", kb.ReadFromPort(0));

            kb.KeyPress("A");
            kb.KeyPress("A");
            kb.KeyPress("A");
            kb.KeyPress("F");
            kb.KeyPress("A");
            kb.KeyPress("A");

            Assert.AreEqual(4, kb.BufferSize);

            Console.WriteLine(kb);

            Assert.AreEqual("10100001", kb.ReadFromPort(0));

            Console.WriteLine(kb);
        }
    }
}