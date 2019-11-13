using Assembler.Core.Microprocessor;
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
        public void IOHexKeyboardTests_ExecuteOneInstruction_Success()
        {
            IOHexKeyboard kb = new IOHexKeyboard(90);

            Assert.AreEqual("00", kb.ReadFromPort(0));

            kb.KeyPress("A");
            kb.KeyPress("A");
            kb.KeyPress("A");
            kb.KeyPress("F");
            kb.KeyPress("A");
            kb.KeyPress("A");

            Assert.AreEqual(4, kb.BufferSize);

            Console.WriteLine(kb);

            Assert.AreEqual("A1", kb.ReadFromPort(0));

            Console.WriteLine(kb);
        }

        [TestMethod]
        public void IOHexKeyboardTests_ReadFromMicro_Success()
        {
            VirtualMemory vm = new VirtualMemory(new string[] { 
                "0000",
                "0000",
                "0000",
                "0000",
                "0000",
                "0000",
                "0000",
                "0000"
            });

            IOManager manager = new IOManager(100);

            MicroSimulator micro = new MicroSimulator(vm, manager);

            Console.WriteLine("Initial State:");
            Console.Write(vm);
            Console.WriteLine(manager);
            Console.WriteLine(micro);

            IOHexKeyboard kb = new IOHexKeyboard(5);

            manager.AddIODevice(5, kb);

            Console.WriteLine("\nAfter adding IO Hex Keyboard:");
            Console.WriteLine(manager);

            Console.WriteLine($"\nIO Device: {kb}");

            Assert.AreEqual("00", kb.ReadFromPort(5));

            kb.KeyPress("A");
            kb.KeyPress("A");
            kb.KeyPress("A");
            kb.KeyPress("F");
            kb.KeyPress("A");

            Console.WriteLine(kb);

            string contentHex = micro.ReadFromMemory(5);

            Assert.AreEqual("A1", contentHex);

            Console.WriteLine($"\nContent read in Hex: {contentHex}");
        }
    }
}