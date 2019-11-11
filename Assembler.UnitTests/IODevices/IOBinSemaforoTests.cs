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
    public class IOBinSemaforoTests
    {
        [TestMethod]
        public void IOBinSemaforoTests_ExecuteOneInstruction_Success()
        {
            IOBinSemaforo semaforo = new IOBinSemaforo(4, "#Debug");

            Assert.AreEqual(false, semaforo.HasData);
            Assert.AreEqual(null, semaforo.BitContent);

            semaforo.WriteInPort(4, "FF");

            foreach (char c in semaforo.BitContent)
                Assert.AreEqual('1', c);

            Console.WriteLine(semaforo);


        }

        [TestMethod]
        public void IOBinSemaforoTests_WriteFromMicro_Success()
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

            IOBinSemaforo semaforo = new IOBinSemaforo(4, "#Debug");

            manager.AddIODevice(4, semaforo);

            Console.WriteLine("\nAfter adding IO Hex Keyboard:");
            Console.WriteLine(manager);

            for (int i = 0; i < 7; i++)
                micro.NextInstruction();

            foreach (char c in semaforo.BitContent)
                Assert.AreEqual('1', c);

            Console.WriteLine(semaforo);


        }
    }
}
