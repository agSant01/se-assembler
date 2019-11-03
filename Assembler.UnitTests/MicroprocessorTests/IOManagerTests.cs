using Assembler.Core.Microprocessor;
using Assembler.Core.Microprocessor.IO;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Assembler.UnitTests.MicroprocessorTests
{
    [TestClass]
    public class IOManagerTests
    {

        [TestMethod]
        public void IOManagerTests_AddAnIODevice_Success()
        {
            IOManager io = new IOManager();

            io.AddIODevice(80, new Device() { Id = 1 });
            io.AddIODevice(82, new Device() { Id = 2 });

            Console.WriteLine(io);

            Assert.AreEqual(2, io.ConnectedDevices);

            Assert.IsTrue(io.RemoveIODevice(80));

            Assert.AreEqual(1, io.ConnectedDevices);
        }
    }

    class Device : IIODevice
    {
        public int Id { get; set; }

        short IIODevice.IOPortLength => 1;

        bool IIODevice.HasData => true;

        string IIODevice.ReadFromPort(int port)
        {
            return "01100110";
        }

        bool IIODevice.Reset()
        {
            return true;
        }

        bool IIODevice.WriteInPort(int port, string contentInHex)
        {
            return true;
        }

        public override string ToString()
        {
            return $"Device[Id:{Id}]";
        }
    }
}