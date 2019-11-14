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
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Invalid port \n")]
        public void ASCIIDisplayTests_ReadFromInvalidPort()
        {
            ASCII_Display display = new ASCII_Display(80);
            Assert.AreEqual("00", display.ReadFromPort(0));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException),"Provided a negative port number!\n")]
        public void ASCIIDisplayTests_NegativePortNumber_Display_Initialization()
        {
            ASCII_Display display = new ASCII_Display(-80);
            //Assert.AreEqual("00", display.ReadFromPort(0));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Provided an overflowable port number!\n")]
        public void ASCIIDisplayTests_OverflowPortNumber()
        {
            ASCII_Display display = new ASCII_Display(short.MaxValue);
        }

        [TestMethod]
        public void ASCIIDisplayTests_ExecuteOneInstruction_Success()
        {
            ASCII_Display display = new ASCII_Display(80, true);
            //Assert.AreEqual("00", display.ReadFromPort(0));

            display.WriteInPort(80, "B");
            display.WriteInPort(81, "A");
            display.WriteInPort(82, "C");
            display.WriteInPort(83, "K");
            display.WriteInPort(84, "T");
            display.WriteInPort(85, "O");
            display.WriteInPort(86, "N");
            display.WriteInPort(87, "m");


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

            ASCII_Display display = new ASCII_Display(5, true);

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

        [TestMethod]
        public void ASCIIDisplayTests_TestPorts_Write()
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

            short start_port = 5;
            ASCII_Display display = new ASCII_Display(start_port, true);

            manager.AddIODevice(start_port, display);

            Console.WriteLine("\nAfter adding ASCII Display:");
            Console.WriteLine(manager);

            Console.WriteLine($"\nIO Device: {display}");

            Assert.AreEqual(null, display.ReadFromPort(5));

            //Write To Valid port range
            Assert.IsTrue(display.WriteInPort(5, "H"));
            Assert.IsTrue(display.WriteInPort(6, "E"));
            Assert.IsTrue(display.WriteInPort(7, "L"));
            Assert.IsTrue(display.WriteInPort(8, "L"));
            Assert.IsTrue(display.WriteInPort(9, "O"));

            Console.WriteLine(display);

            //Writing into invalid Ports

            //Positive before valid range
            Assert.AreEqual(display.WriteInPort(4, "X"), false);

            //Positive after valid range
            Assert.AreEqual(display.WriteInPort(13, "X"), false);

            //Maximum Port Number
            Assert.AreEqual(display.WriteInPort(short.MaxValue, "X"), false);

            //Negative Port Number
            Assert.AreEqual(display.WriteInPort(short.MinValue, "X"), false);
        }

        [TestMethod]
        public void ASCIIDisplayTests_TestPorts_Read()
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

            ASCII_Display display = new ASCII_Display(5, true);

            manager.AddIODevice(5, display);

            Console.WriteLine("\nAfter adding ASCII Display:");
            Console.WriteLine(manager);

            Console.WriteLine($"\nIO Device: {display}");


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

            //Verify that it wrote to memory correctly
            Assert.AreEqual(display.ReadFromPort(5), micro.ReadFromMemory(5));


            //Reading from invalid before Port Range
            var ex = Assert.ThrowsException<ArgumentException>(() => display.ReadFromPort(4));
            Assert.AreEqual(ex.Message, "Invalid port \n");

            var ex3 = Assert.ThrowsException<ArgumentException>(() => display.ReadFromPort(13));
            Assert.AreEqual(ex3.Message, "Invalid port \n");

            //Maximum Port Number
            var ex1 = Assert.ThrowsException<ArgumentException>(() => display.ReadFromPort(short.MaxValue));
            Assert.AreEqual(ex1.Message, "Invalid port \n");

            //Minimum Port Number
            var ex2 = Assert.ThrowsException<ArgumentException>(() => display.ReadFromPort(short.MinValue));
            Assert.AreEqual(ex2.Message, "Invalid port \n");

        }

        [TestMethod]
        public void ASCIIDisplayTests_TestPorts_Read_Write()
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

            short start_port = 5;
            ASCII_Display display = new ASCII_Display(start_port, true);//Starting port

            manager.AddIODevice(start_port, display);

            Console.WriteLine("\nAfter adding ASCII Display:");
            Console.WriteLine(manager);

            Console.WriteLine($"\nIO Device: {display}");

            Assert.AreEqual(null, display.ReadFromPort(5));

            //Write To Valid port range
            Assert.IsTrue(display.WriteInPort(5, "H"));
            Assert.IsTrue(display.WriteInPort(6, "E"));
            Assert.IsTrue(display.WriteInPort(7, "L"));
            Assert.IsTrue(display.WriteInPort(8, "L"));
            Assert.IsTrue(display.WriteInPort(9, "O"));

            Console.WriteLine(display);


            //Verify that it wrote to memory correctly
            Assert.AreEqual(display.ReadFromPort(5) , micro.ReadFromMemory(5));


            //Reading from invalid before Port Range
            var ex = Assert.ThrowsException<ArgumentException>(() => display.ReadFromPort(4));
            Assert.AreEqual(ex.Message, "Invalid port \n");

            var ex3 = Assert.ThrowsException<ArgumentException>(() => display.ReadFromPort(13));
            Assert.AreEqual(ex3.Message, "Invalid port \n");

            //Maximum Port Number
            var ex1 = Assert.ThrowsException<ArgumentException>(() => display.ReadFromPort(short.MaxValue));
            Assert.AreEqual(ex1.Message, "Invalid port \n");

            //Minimum Port Number
            var ex2 = Assert.ThrowsException<ArgumentException>(() => display.ReadFromPort(short.MinValue));
            Assert.AreEqual(ex2.Message, "Invalid port \n");


            //Writing into invalid Ports

            //Positive before valid range
            Assert.AreEqual(display.WriteInPort(4, "X"), false);

            //Positive after valid range
            Assert.AreEqual(display.WriteInPort(13, "X"), false);

            //Maximum Port Number
            Assert.AreEqual(display.WriteInPort(short.MaxValue, "X"), false);

            //Negative Port Number
            Assert.AreEqual(display.WriteInPort(short.MinValue, "X"), false);


        }

    }
}