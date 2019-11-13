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
    public class ASCII_DisplayTests
    {
        short port = 0;

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Invalid port \n")]
        public void ASCIIDisplayTests_InvalidPort()
        {
            ASCII_Display display = new ASCII_Display(80);
            Assert.AreEqual("00", display.ReadFromPort(0));
        }

        [TestMethod]
        public void ASCIIDisplayTests_ExecuteOneInstruction_Success()
        {
            ASCII_Display display = new ASCII_Display(80);
            //Assert.AreEqual("00", display.ReadFromPort(0));

            display.WriteInPort(80, "B");
            display.WriteInPort(81, "A");
            display.WriteInPort(82, "C");
            display.WriteInPort(83, "K");
            display.WriteInPort(84, "T");
            display.WriteInPort(85, "O");
            display.WriteInPort(86, "N");
            display.WriteInPort(87, "m");

            //Assert.AreEqual(4, kb.BufferSize);

            Console.WriteLine(display);

            Assert.AreEqual("B", display.ReadFromPort(80));

            Console.WriteLine(display);
        }

        [TestMethod]
        public void ASCIIDisplayTests_ReadFromMicro_Success()
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

            Console.WriteLine("Starting State:");
            Console.Write(vm);
            Console.WriteLine(manager);
            Console.WriteLine(micro);

            ASCII_Display display = new ASCII_Display(5);

            manager.AddIODevice(5, display);

            Console.WriteLine("\nAfter adding ASCII Display:");
            Console.WriteLine(manager);

            Console.WriteLine($"\nIO Device: {display}");

            Assert.AreEqual(null, display.ReadFromPort(5));

            display.WriteInPort(5, "H");
            display.WriteInPort(6, "E");
            display.WriteInPort(7, "L");
            display.WriteInPort(8, "L");
            display.WriteInPort(9, "O");

            Console.WriteLine(display);

            string h = micro.ReadFromMemory(5);
            string e = micro.ReadFromMemory(6);
            string l = micro.ReadFromMemory(7);
            string l2 = micro.ReadFromMemory(8);
            string o = micro.ReadFromMemory(9);
            Assert.AreEqual("H", h);
            Assert.AreEqual("E", e);
            Assert.AreEqual("L", l);
            Assert.AreEqual("L", l2);
            Assert.AreEqual("O", o);

            Console.WriteLine($"\nContent read in Hex: {h} {e} {l} {l2} {o}");
        }
    }
}