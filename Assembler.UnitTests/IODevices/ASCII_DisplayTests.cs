using Assembler.Core.Microprocessor;
using Assembler.Core.Microprocessor.IO.IODevices;
using Assembler.Microprocessor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Provided an overflowable port number!\n")]
        public void ASCIIDisplayTests_OverflowPortNumber()
        {
            ASCII_Display display = new ASCII_Display(ushort.MaxValue);
        }

        [TestMethod]
        public void ASCIIDisplayTests_ExecuteOneInstruction_Success()
        {
            ASCII_Display display = new ASCII_Display(80, true);
            //Assert.AreEqual("00", display.ReadFromPort(0));

            display.WriteInPort(80, "66");
            display.WriteInPort(81, "67");
            display.WriteInPort(82, "68");
            display.WriteInPort(83, "69");
            display.WriteInPort(84, "70");
            display.WriteInPort(85, "71");
            display.WriteInPort(86, "72");
            display.WriteInPort(87, "73");


            Console.WriteLine(display);

            Assert.AreEqual("66", display.ReadFromPort(80));

            Console.WriteLine(display);
        }

        [TestMethod]
        public void ASCIIDisplayTests_Reset_After_ExecuteOneInstruction_Success()
        {
            ASCII_Display display = new ASCII_Display(80, true);
            //Assert.AreEqual("00", display.ReadFromPort(0));

            display.WriteInPort(80, "66");
            display.WriteInPort(81, "67");
            display.WriteInPort(82, "68");
            display.WriteInPort(83, "69");
            display.WriteInPort(84, "70");
            display.WriteInPort(85, "71");
            display.WriteInPort(86, "72");
            display.WriteInPort(87, "73");


            Console.WriteLine(display.DisplaySlots);

            //Assert.AreEqual("66", display.ReadFromPort(80));

            //Console.WriteLine(display);
            string[] empty = new string[]{"","", "", "", "", "" , "", "" };

            Assert.IsTrue(display.Reset());
            Console.WriteLine(empty);
            Console.WriteLine(display.DisplaySlots);

            //Lazy comparison because a deep comparison will result in false
            //since they are obviously not meant to be the same objects
            for(int i = 0; i < empty.Length; i++)
            {
                Assert.IsTrue(empty[i] == display.DisplaySlots[i]);
            }
            

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

            Assert.AreEqual("00", display.ReadFromPort(5));

            display.WriteInPort(5, "48");
            display.WriteInPort(6, "6F");
            display.WriteInPort(7, "6C");
            display.WriteInPort(8, "61");

            Console.WriteLine(display);

            string h = micro.ReadFromMemory(5);
            string o = micro.ReadFromMemory(6);
            string l = micro.ReadFromMemory(7);
            string a = micro.ReadFromMemory(8);

            Assert.AreEqual("48", h);
            Assert.AreEqual("6F", o);
            Assert.AreEqual("6C", l);
            Assert.AreEqual("61", a);

            Console.WriteLine($"\nContent read in Hex: {h} {o} {l} {a}");
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

            ushort start_port = 5;
            ASCII_Display display = new ASCII_Display(start_port, true);

            manager.AddIODevice(start_port, display);

            Console.WriteLine("\nAfter adding ASCII Display:");
            Console.WriteLine(manager);

            Console.WriteLine($"\nIO Device: {display}");

            Assert.AreEqual("00", display.ReadFromPort(5));

            //Write To Valid port range
            Assert.IsTrue(display.WriteInPort(5, "48"));
            Assert.IsTrue(display.WriteInPort(6, "6F"));
            Assert.IsTrue(display.WriteInPort(7, "6C"));
            Assert.IsTrue(display.WriteInPort(8, "61"));

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

            display.WriteInPort(5, "48");
            display.WriteInPort(6, "6F");
            display.WriteInPort(7, "6C");
            display.WriteInPort(8, "61");

            Console.WriteLine(display);

            string h = micro.ReadFromMemory(5);
            string o = micro.ReadFromMemory(6);
            string l = micro.ReadFromMemory(7);
            string a = micro.ReadFromMemory(8);

            Assert.AreEqual("48", h);
            Assert.AreEqual("6F", o);
            Assert.AreEqual("6C", l);
            Assert.AreEqual("61", a);

            Console.WriteLine($"\nContent read in Hex: {h} {o} {l} {a}");

            //Verify that it wrote to memory correctly
            Assert.AreEqual(display.ReadFromPort(5), micro.ReadFromMemory(5));

            //Reading from invalid before Port Range
            var ex = Assert.ThrowsException<ArgumentException>(() => display.ReadFromPort(4));
            Assert.AreEqual(ex.Message, "Invalid port \n");

            var ex3 = Assert.ThrowsException<ArgumentException>(() => display.ReadFromPort(15));
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

            ushort start_port = 5;
            ASCII_Display display = new ASCII_Display(start_port, true);//Starting port

            manager.AddIODevice(start_port, display);

            Console.WriteLine("\nAfter adding ASCII Display:");
            Console.WriteLine(manager);

            Console.WriteLine($"\nIO Device: {display}");

            Assert.AreEqual("00", display.ReadFromPort(5));

            //Write To Valid port range
            Assert.IsTrue(display.WriteInPort(5, "48"));
            Assert.IsTrue(display.WriteInPort(6, "6F"));
            Assert.IsTrue(display.WriteInPort(7, "6C"));
            Assert.IsTrue(display.WriteInPort(8, "61"));

            Console.WriteLine(display);


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